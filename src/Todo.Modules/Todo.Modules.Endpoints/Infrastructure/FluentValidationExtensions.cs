using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Todo.Modules.Endpoints.Infrastructure;

internal static class FluentValidationExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationResult result)
    {
        return new ProblemDetails
        {
            Title = "Request validation error",
            Status = 400,
            Detail = "One or more validation errors occured during the request validation. See extensions for more details.",
            Extensions = result.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key,
                    x => x.Select(y => y.ErrorMessage).ToList() as object)!
        };
    }
}