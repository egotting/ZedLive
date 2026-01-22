namespace ZedLive.Tests.StreamToken;

public class StreamKey
{
    [Fact]
    public void ValidToGenerateKey()
    {
        var random = new Random();
        var str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        int size = 6;
        string value = string.Empty;
        for (int i = 0; i < size; i++)
        {
            int x = random.Next(26);
            value = value + str[x];
        }

        Assert.IsType<string>(value);
    }
    
    
    
}