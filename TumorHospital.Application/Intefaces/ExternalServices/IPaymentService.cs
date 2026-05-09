using TumorHospital.Application.DTOs.Request.Payment;
using static TumorHospital.Application.DTOs.Request.Payment.EInvoiceResponseModel;

namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface IPaymentService
    {
        Task<EInvoiceResponseDataModel?> CreateEInvoiceAsync(EInvoiceRequestModel eInvoice);
        bool VerifyWebhook(WebHookModel webHook);
    }
}
