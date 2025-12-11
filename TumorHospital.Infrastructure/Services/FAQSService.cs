using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.FAQs;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class FAQSService : IFAQSService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FAQSService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddFAQ(NewFAQsDto dto)
        {
            var faq = _mapper.Map<FAQ>(dto);
            await _unitOfWork.FAQs.AddAsync(faq);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteFAQ(int id)
        {
            var faq = _unitOfWork.FAQs.GetByIdAsync(id);
            if(faq is null) throw new Exception("FAQ not found");

            _unitOfWork.FAQs.Delete(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<FAQsDto>> GetAllFAQs()
            => await _unitOfWork.FAQs.GetAllAsync(
                selector: faq => new FAQsDto
                {
                    Id = faq.Id,
                    Question = faq.Question,
                    Answer = faq.Answer
                }
                );

        public async Task UpdateFAQ(int id, NewFAQsDto dto)
        {
            var faq = await _unitOfWork.FAQs.GetByIdAsync(id);
            if (faq is null) throw new Exception("FAQ not found");

            faq.Question = dto.Question;
            faq.Answer = dto.Answer;
            await _unitOfWork.CompleteAsync();
        }
    }
}
