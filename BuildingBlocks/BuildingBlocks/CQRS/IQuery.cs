using MediatR;

namespace eCommerceApp.BuildingBlocks.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{ }