namespace Core.Security.Crypt;

public record struct HashResult
{
    public string Hash { get; init; }
    public string Salt { get; init; }
}
