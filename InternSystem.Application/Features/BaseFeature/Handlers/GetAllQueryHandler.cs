using AutoMapper;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.BaseFeature.Queries;
using MediatR;

namespace InternSystem.Application.Features.BaseFeature.Handlers
{
    public class GetAllQueryHandler<TEntity, TResponse> : IRequestHandler<GetAllQuery<TResponse>, IEnumerable<TResponse>> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TResponse>> Handle(GetAllQuery<TResponse> request, CancellationToken cancellationToken)
        {
            // var repository = _unitOfWork.CauHoiCongNgheRepository.Update;// _unitOfWork.Repository<TEntity>();
            // var entities = await repository.GetAllAsync();
            object entities = new();
            return _mapper.Map<IEnumerable<TResponse>>(entities);
        }
    }

}
