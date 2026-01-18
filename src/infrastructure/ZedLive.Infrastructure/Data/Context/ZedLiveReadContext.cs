namespace ZedLive.Infrastructure.Data.Context;

public sealed class ZedLiveReadContext
{
    private static bool IsRead(Type type) => type.FullName?.Contains("Mapper.Read") ?? false;
}