using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Todo.Modules.Endpoints.Infrastructure;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal sealed record UpdateTodoRequest(string? Title, string? Description);

internal sealed class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
    public UpdateTodoRequestValidator()
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

internal static class UpdateTodoEndpoint
{
    public static void AddUpdateTodoEndpoint(this IServiceCollection services) =>
        services.AddScoped<IValidator<UpdateTodoRequest>, UpdateTodoRequestValidator>();

    public static void UseUpdateTodoEndpoint(this IEndpointRouteBuilder route) =>
        route.MapPut("api/v1/todo-lists/{listId:int}/todos/{todoId:int}", UpdateTodo);

    private static async Task<IResult> UpdateTodo(
        [FromRoute]int listId,
        [FromRoute]int todoId,
        [FromBody]UpdateTodoRequest request,
        [FromServices]ITodoItemRepository repository,
        [FromServices]IValidator<UpdateTodoRequest> validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (validationResult.IsValid == false)
            return Results.Problem(validationResult.ToProblemDetails());

        var updatedTodo = await repository.UpdateTodoAsync(listId, todoId, request.Title, request.Description, ct);

        return Results.Ok(updatedTodo);
    }
}