using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Queries;

internal static class GetTodoListsEndpoint
{
    public static void UseGetTodoListsEndpoint(this IEndpointRouteBuilder route) =>
        route.MapGet("api/v1/todo-lists", GetTodoLists);

    private static async Task<IResult> GetTodoLists(
        [FromServices] ITodoListRepository repository,
        CancellationToken ct)
    {
        var lists = await repository.GetListsAsync(ct);

        return Results.Ok(lists);
    }
}