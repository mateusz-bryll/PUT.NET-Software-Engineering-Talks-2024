using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal static class RemoveTodoEndpoint
{
    public static void UseRemoveTodoEndpoint(this IEndpointRouteBuilder route) =>
        route.MapDelete("api/v1/todo-lists/{listId:int}/todos/{todoId:int}", RemoveTodo);

    private static async Task<IResult> RemoveTodo(
        [FromRoute]int listId,
        [FromRoute]int todoId,
        [FromServices]ITodoItemRepository repository,
        CancellationToken ct)
    {
        await repository.RemoveTodoAsync(listId, todoId, ct);

        return Results.NoContent();
    }
}