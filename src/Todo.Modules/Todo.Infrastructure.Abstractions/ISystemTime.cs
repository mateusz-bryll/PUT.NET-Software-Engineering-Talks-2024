namespace Todo.Infrastructure.Abstractions;

public interface ISystemTime
{
    DateTimeOffset Now { get; }
}