using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO.CrmDtos.CrmReadDtos;
using Yonetim360Business.DTO.CrmReadDtos;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CustomerSupportRequests.Queries.GetCustomerSupportRequests
{
    public class GetCustomerSupportRequestsQueryHandler : IQueryHandler<GetCustomerSupportRequestsQuery, List<ReadCustomerSupportRequestDto>>
    {
        private readonly IRepository<CustomerSupportRequest> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCustomerSupportRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CustomerSupportRequest>();
            _mapper = mapper;
        }

        public async Task<List<ReadCustomerSupportRequestDto>> Handle(GetCustomerSupportRequestsQuery request, CancellationToken cancellationToken)
        {
            var supports = await _repository.GetAllAsync(
                filter: null,
                tracked: false,
                pageSize: request.PageSize,
                pageNumber: request.PageNumber,
                include: q => q
                    .Include(r => r.Representatives)
                    .Include(c => c.Customer)
            ) ?? throw new InvalidDataException("Not found any customer support request records");

            // Manuel projection ile sadece Id ve FullName
            var result = supports.Select(s => new ReadCustomerSupportRequestDto
            {
                Id = s.Id,
                CreatedBy = s.CreatedBy,
                CustomerId = s.CustomerId,
                CustomerCompanyName = s.Customer.CompanyName,
                Subject = s.Subject,
                Explanation = s.Explanation,
                Priority = s.Priority,
                SupportRequestStatus = s.SupportRequestStatus,
                Representatives = s.Representatives
                    .Select(r => new ReadCrmLightRepresentativeDto
                    {
                        Id = r.Id,
                        FullName = (r.FirstName + " " + r.LastName).Trim()
                    })
                    .ToList()
            }).ToList();

            return result;
        }
    }
}
