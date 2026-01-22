namespace ZedLive.Domain.ValueObjects.StructType;

public record ConnectionStringsOptions
{
    // WARNING: THIS FILE CANNOT BE SHOWING TO ANYONE
    public string DbConnection { get; set; } =
        "User ID=postgres;Password=postgres123;Host=database;Port=5555;Database=ZedLiveDatabase;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
}