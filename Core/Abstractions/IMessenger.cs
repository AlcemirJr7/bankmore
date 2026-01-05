namespace Core.Abstractions;
public interface IMessenger<in TMessege> where TMessege : class
{
    Task EnviarAsync(TMessege messege);
}