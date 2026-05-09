using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Request.Payment;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public DonationService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }


        public async Task<string> CreateDonation(VolunteerDto dto)
        {
            var need = await _unitOfWork.CharityNeeds.GetByIdAsync(dto.CharityNeedId);

            if (need == null)
                throw new Exception("Charity need not found");

            var donation = new VolunteerDonation
            {
                VolunteerName = dto.VolunteerName,
                Email = dto.Email,
                Phone = dto.Phone,
                AmountDonated = dto.AmountDonated,
                CharityNeedId = dto.CharityNeedId,
                Status = "Pending"
            };

            await _unitOfWork.VolunteerDonations.AddAsync(donation);
            await _unitOfWork.CompleteAsync();

            var invoice = new EInvoiceRequestModel
            {
                Customer = new EInvoiceRequestModel.CustomerModel
                {
                    FirstName = dto.VolunteerName,
                    LastName = "Donor",
                    Email = dto.Email,
                    Phone = dto.Phone
                },
                CartItems = new List<EInvoiceRequestModel.CartItemModel>
                {
                    new()
                    {
                        Name = "Donation For "+need.Title,
                        Price = dto.AmountDonated,
                        Quantity = 1
                    }
                },
                Currency = "EGP",
                PayLoad = new EInvoiceRequestModel.InvoicePayload
                {
                    OrderId = donation.Id.ToString()
                },
                RedirectionUrls = new EInvoiceRequestModel.RedirectionUrlsModel
                {
                    //OnSuccess = "https://tumorhospital.runasp.net/api/Payment/fawaterak/webhooks/success",
                    //OnFailure = "https://tumorhospital.runasp.net/api/Payment/fawaterak/webhooks/fail",
                    //OnPending = "https://tumorhospital.runasp.net/api/Payment/fawaterak/webhooks/pending"
                    OnSuccess = "https://tumor-hospital.vercel.app/donations/successful",
                    OnFailure = "https://tumor-hospital.vercel.app/donations/fail",
                    OnPending = "https://tumorhospital.runasp.net/api/Payment/fawaterak/webhooks/pending"
                }
               
            };

            var result = await _paymentService.CreateEInvoiceAsync(invoice);

            if (result == null)
                throw new Exception("Failed to create invoice");

            donation.PaymentReference = result.InvoiceId;

            await _unitOfWork.CompleteAsync();

            return result.Url;
        }

        public async Task<decimal> SuccessDonation(string invoiceId)
        {
            var donation = await _unitOfWork.VolunteerDonations
                .GetEnhancedAsync(
                filter: d => d.PaymentReference == invoiceId,
                selector: d => d);

            if (donation == null)
                throw new Exception("Donation not found");

            var need = await _unitOfWork.CharityNeeds.GetEnhancedAsync(
                filter: n => n.Id == donation.CharityNeedId,
                selector: n => n);

            if (need == null)
                throw new Exception("Need not found");


            donation.Status = "Paid";
            need.CollectedAmount += donation.AmountDonated;
            if (need.CollectedAmount >= need.NeedAmount)
                need.IsCompleted = true;
            await _unitOfWork.CompleteAsync();
            return donation.AmountDonated;
        }

        public async Task FaildedDonation(string invoiceId)
        {
            var donation = await _unitOfWork.VolunteerDonations
                .GetEnhancedAsync(
                filter: d => d.PaymentReference == invoiceId,
                selector: d => d);

            if (donation == null)
                throw new Exception("Donation not found");

            var need = await _unitOfWork.CharityNeeds.GetEnhancedAsync(
                filter: n => n.Id == donation.CharityNeedId,
                selector: n => n);

            if (need == null)
                throw new Exception("Need not found");


            donation.Status = "Failed";
           
            await _unitOfWork.CompleteAsync();
        }

        public async Task HandleWebhook(WebHookModel model)
        {
            var valid = _paymentService.VerifyWebhook(model);
            if (!valid)
                throw new Exception("Invalid webhook signature");

            var payload = JsonConvert.DeserializeObject<WebhookPayload>(model.PayloadString ?? "");

            if (payload == null)
                throw new Exception("Invalid payload");

            var donationId = Guid.Parse(payload.OrderId);

            var donation = await _unitOfWork.VolunteerDonations.GetByIdAsync(donationId);

            if (donation == null)
                throw new Exception("Donation not found");

            if (model.InvoiceStatus.ToLower() == "paid")
            {
                donation.Status = "Paid";

                var need = await _unitOfWork.CharityNeeds.GetByIdAsync(donation.CharityNeedId.Value);

                need.CollectedAmount += donation.AmountDonated;

                if (need.CollectedAmount >= need.NeedAmount)
                    need.IsCompleted = true;
            }
            else
            {
                donation.Status = "Failed";
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
