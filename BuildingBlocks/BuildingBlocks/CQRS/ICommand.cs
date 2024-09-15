using MediatR;

namespace eCommerceApp.BuildingBlocks.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse> { }

public interface ICommand : ICommand<Unit> { }