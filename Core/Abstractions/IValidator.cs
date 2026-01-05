using Core.Response;

namespace Core.Abstractions;

public interface IValidator<T> where T : class
{
    ApiResponse<T> Validar(T input);
}

public interface IValidator<in TInput, TResult> where TInput : class
{
    TResult Validar(TInput input);
}

public interface IValidator<in TInput, TResult, TData>
    where TInput : class
    where TData : class
{
    TResult Validar(TInput input, TData? data);
}