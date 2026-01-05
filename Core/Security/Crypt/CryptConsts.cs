namespace Core.Security.Crypt;

internal readonly record struct CryptConsts
{
    public const int KeySize = 32;
    public const int Iterations = 500_000;
    public const int SaltSize = 16;
}
