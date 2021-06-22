using Abstraction.Repository;

namespace Service.BaseService
{
    public abstract class Service
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly AutoMapper.IMapper _mapper;

        protected Service(IUnitOfWork unitOfWork, AutoMapper.IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}