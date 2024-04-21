using Todo.Infrastructure.Common;
using Todo.Modules.Endpoints;
using Todo.Persistence.MySql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure();
builder.Services.AddTodoMySqlPersistence(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddTodoEndpoints();

var app = builder.Build();
app.UseInfrastructure();
app.UseTodoMySqlPersistence();
app.UseTodoEndpoints();
app.Run();