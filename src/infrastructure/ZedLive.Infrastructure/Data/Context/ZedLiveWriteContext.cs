namespace ZedLive.Infrastructure.Data.Context;

public sealed class ZedLiveWriteContext
{


    private static bool IsWrite(Type type) => type.FullName?.Contains("Mapper.Write") ?? false;
}