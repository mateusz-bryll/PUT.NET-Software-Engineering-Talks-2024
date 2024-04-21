using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Todo.Modules.Endpoints.Infrastructure;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal sealed record UpdateTodoListERequest(string? Title, string? Description);

internal sealed class UpdateTodoListERequestValidator : AbstractValidator<UpdateTodoListERequest>
{
    public UpdateTodoListERequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(255)
            .When(x => string.IsNullOrEmpty(x.Description))
                .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(3000)
            .When(x => string.IsNullOrEmpty(x.Title))
                .NotEmpty();
    }
}

internal static class UpdateTodoListEndpoint
{
    public static void AddUpdateTodoListEndpoint(this IServiceCollection services) =>
        services.AddScoped<IValidator<UpdateTodoListERequest>, UpdateTodoListERequestValidator>();
    
    public static void UseUpdateTodoListEndpoint(this IEndpointRouteBuilder route) =>
        route.MapPut("api/v1/todo-lists/{listId:int}", UpdateTodoList);

    private static async Task<IResult> UpdateTodoList(
        [FromRoute]int listId,
        [FromBody]UpdateTodoListERequest request,
        [FromServices]ITodoListRepository repository,
        [FromServices]IValidator<UpdateTodoListERequest> validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (validationResult.IsValid == false)
            return Results.Problem(validationResult.ToProblemDetails());

        var updatedList = await repository.UpdateList(listId, request.Title, request.Description, ct);

        return Results.Ok(updatedList);
    }
}