namespace Core.Abstractions;

public interface IExceptionApi<in TException, TError> where TException : Exception
{
    TError ToError(TException exception);
}

public interface IExceptionApi<TError> where TError : class
{
    TError ToError();
}
