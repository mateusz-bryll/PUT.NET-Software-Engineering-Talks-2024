using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Todo.Modules.Endpoints.Commands;
using Todo.Modules.Endpoints.Queries;

namespace Todo.Modules.Endpoints;

public static class Extensions
{
    public static IServiceCollection AddTodoEndpoints(this IServiceCollection services)
    {
        services.AddCreateTodoListEndpoint();
        services.AddUpdateTodoListEndpoint();
        services.AddCreateTodoEndpoint();
        services.AddUpdateTodoEndpoint();
        
        return services;
    }

    public static void UseTodoEndpoints(this IEndpointRouteBuilder route)
    {
        route.UseCreateTodoListEndpoint();
        route.UseGetTodoListsEndpoint();
        route.UseUpdateTodoListEndpoint();
        route.UseRemoveTodoListEndpoint();
        route.UseCreateTodoEndpoint();
        route.UseGetTodosEndpoint();
        route.UseUpdateTodoEndpoint();
        route.UseToggleTodoStateEndpoint();
        route.UseRemoveTodoEndpoint();
    }
}