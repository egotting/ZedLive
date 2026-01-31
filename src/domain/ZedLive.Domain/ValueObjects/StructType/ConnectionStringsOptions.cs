namespace ZedLive.Domain.ValueObjects.StructType;

public record ConnectionStringsOptions
{
    public string DbConnection { get; set; } = string.Empty;
}