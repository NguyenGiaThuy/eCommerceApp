
using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eCommerceApp.BuildingBlocks.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>
    (ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[START] Handling: Request={request} Response={response} - RequestData={requestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();

        timer.Start();
        var response = await next();
        timer.Stop();

        var timeTaken = timer.Elapsed;
        if (timeTaken.Milliseconds > 1500) logger.LogWarning(
            "[PERFORMANCE] The request {request} took {timeTaken}ms.",
            typeof(TRequest).Name, timeTaken.Milliseconds);

        logger.LogInformation(
            "[END] Handled: Request={request} - Response={response}",
            typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}