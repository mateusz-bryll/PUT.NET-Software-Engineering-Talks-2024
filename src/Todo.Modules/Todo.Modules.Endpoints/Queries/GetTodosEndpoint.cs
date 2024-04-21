using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Queries;

internal static class GetTodosEndpoint
{
    public static void UseGetTodosEndpoint(this IEndpointRouteBuilder route) =>
        route.MapGet("api/v1/todo-lists/{listId:int}/todos", GetTodos);

    private static async Task<IResult> GetTodos(
        [FromRoute]int listId,
        [FromServices]ITodoItemRepository repository,
        CancellationToken ct)
    {
        var todos = await repository.GetTodosAsync(listId, ct);

        return Results.Ok(todos);
    }
}