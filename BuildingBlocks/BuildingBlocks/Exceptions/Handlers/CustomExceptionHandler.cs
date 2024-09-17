
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace eCommerceApp.BuildingBlocks.Exceptions.Handlers;

public class CustomExceptionHandler
    (ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError($"Error message: {exception.Message}, Time of occurence: {DateTime.UtcNow}");

        switch (exception)
        {
            case InternalServerException:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;

            case ValidationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;

            case BadRequestException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;

            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        var problemDetails = new ProblemDetails
        {
            Title = exception.GetType().Name,
            Status = context.Response.StatusCode,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException)
        {
            var validationException = (ValidationException)exception;
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}