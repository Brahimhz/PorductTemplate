using MediatR;

namespace Product.Core.Infrastructure.Commands
{
    public record UnitOfWorkCommand() : IRequest<Task>;

}
