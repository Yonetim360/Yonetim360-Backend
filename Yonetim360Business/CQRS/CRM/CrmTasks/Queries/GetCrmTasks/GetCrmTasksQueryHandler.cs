using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.DataAccess.Repository.Abstract;
using Yonetim360.DataAccess.UnitOfWorks.Abstract;
using Yonetim360.Entity.CRM;
using Yonetim360Business.DTO;
using Yonetim360Business.Mediator;

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Queries.GetCrmTasks
{
    public class GetCrmTasksQueryHandler : IQueryHandler<GetCrmTasksQuery, List<CrmTaskDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CrmTask> _repository;
        public GetCrmTasksQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository= _unitOfWork.GetRepository<CrmTask>();
        }

        public async Task<List<CrmTaskDto>> Handle(GetCrmTasksQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsync(
                filter: null,
                tracked: false,
                pageSize: request.PageSize,
                pageNumber: request.PageNumber,
                include: q => q.Include(x => x.Representative)
                ) ?? throw new InvalidDataException("Not found tasks");
            return _mapper.Map<List<CrmTaskDto>>(query);
        }
    }
}
