using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal static class ToggleTodoStateEndpoint
{
    public static void UseToggleTodoStateEndpoint(this IEndpointRouteBuilder route) =>
        route.MapPut("api/v1/todo-lists/{listId:int}/todos/{todoId:int}/state", ToggleTodoState);

    private static async Task<IResult> ToggleTodoState(
        [FromRoute]int listId,
        [FromRoute]int todoId,
        [FromServices]ITodoItemRepository repository,
        CancellationToken ct)
    {
        var currentState = await repository.ToggleTodoStateAsync(listId, todoId, ct);

        return Results.Ok(new { CurrentState = currentState });
    }
}