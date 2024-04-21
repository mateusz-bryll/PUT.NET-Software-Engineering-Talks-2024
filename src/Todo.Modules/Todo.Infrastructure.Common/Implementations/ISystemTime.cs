using Todo.Infrastructure.Abstractions;

namespace Todo.Infrastructure.Common.Implementations;

internal sealed class SystemTime : ISystemTime
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}