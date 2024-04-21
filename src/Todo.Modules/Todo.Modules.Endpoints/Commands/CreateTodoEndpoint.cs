using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Todo.Modules.Endpoints.Infrastructure;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal sealed record CreateTodoRequest(string? Title, string? Description);

internal sealed class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(255)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(3000);
    }
}

internal static class CreateTodoEndpoint
{
    public static void AddCreateTodoEndpoint(this IServiceCollection services) =>
        services.AddScoped<IValidator<CreateTodoRequest>, CreateTodoRequestValidator>();

    public static void UseCreateTodoEndpoint(this IEndpointRouteBuilder route) =>
        route.MapPost("api/v1/todo-lists/{listId:int}/todos", CreateTodo);

    private static async Task<IResult> CreateTodo(
        [FromRoute]int listId,
        [FromBody]CreateTodoRequest request,
        [FromServices]ITodoItemRepository repository,
        [FromServices]IValidator<CreateTodoRequest> validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (validationResult.IsValid == false)
            return Results.Problem(validationResult.ToProblemDetails());

        var todoId = await repository.CreateTodoAsync(listId, request.Title!, request.Description, ct);

        return Results.Ok(new { Id = todoId, ListId = listId });
    }
}