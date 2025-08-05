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

namespace Yonetim360Business.CQRS.CRM.CrmTasks.Queries.GetCrmTaskById
{
    public class GetCrmTaskByIdQueryHandler : IQueryHandler<GetCrmTaskByIdQuery, CrmTaskDto>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CrmTask> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public GetCrmTaskByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<CrmTask>();
        }

        public async Task<CrmTaskDto> Handle(GetCrmTaskByIdQuery request, CancellationToken cancellationToken)
        {
           var query = await _repository.GetFirstOrDefaultAsync(
               filter: x => x.Id == request.Id,
               tracked: false,
               include:x=>x.Include(q=>q.Representative)
               )??throw new InvalidDataException("Task cannot found");

            return _mapper.Map<CrmTaskDto>(query);
        }
    }
}
