using MediatR;
using Product.Core.Data.IRepository;
using Product.Core.Infrastructure.Commands;

namespace Product.Core.Infrastructure.Handlers
{
    public class UnitOfWorkHandler : IRequestHandler<UnitOfWorkCommand, Task>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Task> Handle(UnitOfWorkCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_unitOfWork.CompleteAsync());
        }
    }
}
