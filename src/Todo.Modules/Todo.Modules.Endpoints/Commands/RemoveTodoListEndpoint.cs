using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Todo.Persistence.Abstractions;

namespace Todo.Modules.Endpoints.Commands;

internal static class RemoveTodoListEndpoint
{
    public static void UseRemoveTodoListEndpoint(this IEndpointRouteBuilder route) =>
        route.MapDelete("api/v1/todo-lists/{listId:int}", RemoveTodoList);

    
    private static async Task<IResult> RemoveTodoList(
        [FromRoute]int listId,
        [FromServices]ITodoListRepository repository,
        [FromServices]IValidator<CreateTodoListRequest> validator,
        CancellationToken ct)
    {
        await repository.RemoveListAsync(listId, ct);

        return Results.NoContent();
    }
}