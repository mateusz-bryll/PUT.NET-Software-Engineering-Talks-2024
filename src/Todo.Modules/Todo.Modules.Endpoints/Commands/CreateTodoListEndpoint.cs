using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Todo.Modules.Endpoints.Infrastructure;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal sealed record CreateTodoListRequest(string? Title, string? Description);

internal sealed class CreateTodoListRequestValidator : AbstractValidator<CreateTodoListRequest>
{
    public CreateTodoListRequestValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(255)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(3000);
    }
}

internal static class CreateTodoListEndpoint
{
    public static void AddCreateTodoListEndpoint(this IServiceCollection services) =>
        services.AddScoped<IValidator<CreateTodoListRequest>, CreateTodoListRequestValidator>();
    
    public static void UseCreateTodoListEndpoint(this IEndpointRouteBuilder route) =>
        route.MapPost("api/v1/todo-lists", CreateTodoList);

    private static async Task<IResult> CreateTodoList(
        [FromBody]CreateTodoListRequest request,
        [FromServices]ITodoListRepository repository,
        [FromServices]IValidator<CreateTodoListRequest> validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (validationResult.IsValid == false)
            return Results.Problem(validationResult.ToProblemDetails());
        
        var listId = await repository.CreateListAsync(request.Title!, request.Description, ct);

        return Results.Ok(new { Id = listId });
    }
}