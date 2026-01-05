using Core.Errors;
using Core.Idempotencia;
using Core.Infrastructure.Idempotencia;
using Core.Infrastructure.Idempotencia.Repositories;
using Core.Response;
using Core.Security.Crypt;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Core.Infrastructure.Middlewares;

public sealed class IdempotenciaMiddleware(
        RequestDelegate next,
        IIdempotenciaRepository idempotenciaRepository,
        ICryptService cryptoService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var ct = context.RequestAborted;

        if (!HttpMethods.IsPost(context.Request.Method) &&
            !HttpMethods.IsPut(context.Request.Method) &&
            !HttpMethods.IsPatch(context.Request.Method) &&
            !HttpMethods.IsDelete(context.Request.Method))
        {
            await next(context);
            return;
        }

        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<SkipIdempotencyAttribute>() is not null)
        {
            await next(context);
            return;
        }

        // Busca a chave de idempotência no header
        if (!context.Request.Headers.TryGetValue(IdempotenciaConsts.HeaderKeyName, out var chaveIdempotencia)
            || string.IsNullOrWhiteSpace(chaveIdempotencia))
        {
            context.Response.StatusCode = ApiStatusCode.BadRequest;
            var response = ApiResponse.Failure(CoreErrors.Idempotency.HeaderIdempotencia);

            await context.Response.WriteAsJsonAsync(response);

            return;
        }

        var chave = chaveIdempotencia.ToString().ToLower();

        if (chave.Length > IdempotenciaConsts.MaxLength)
        {
            context.Response.StatusCode = ApiStatusCode.BadRequest;
            var response = ApiResponse.Failure(CoreErrors.Idempotency.InvalidLength);

            await context.Response.WriteAsJsonAsync(response);

            return;
        }

        var idempotencia = await idempotenciaRepository.GetAsync(chave);

        if (idempotencia is not null)
        {
            // Retorna o resultado armazenado
            context.Response.StatusCode = idempotencia.StatusCode;
            context.Response.ContentType = "application/json";

            var resultadoDescriptografado = cryptoService.DecryptAES(idempotencia.Resultado);

            await context.Response.WriteAsync(resultadoDescriptografado);

            return;
        }

        // Captura o corpo da requisição
        context.Request.EnableBuffering();
        var requisicao = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        // Captura a resposta original
        var respostaOriginal = context.Response.Body;
        using var respostaMemory = new MemoryStream();
        context.Response.Body = respostaMemory;

        try
        {
            // Executa a requisição
            await next(context);

            // Só armazena se foi sucesso (2xx)
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                respostaMemory.Position = 0;
                var resultado = await new StreamReader(respostaMemory).ReadToEndAsync();

                // CRIPTOGRAFA os dados sensíveis antes de salvar
                var requisicaoCriptografada = cryptoService.EncryptAES(requisicao);
                var resultadoCriptografado = cryptoService.EncryptAES(resultado);

                // Salva no banco
                var novoRegistro = new IdempotenciaEntity
                {
                    ChaveIdempotencia = chave,
                    Requisicao = requisicaoCriptografada,
                    Resultado = resultadoCriptografado,
                    StatusCode = context.Response.StatusCode,
                    DataCriacao = DateTime.Now.ToString()
                };

                await idempotenciaRepository.CreateAsync(novoRegistro);

                Log.Information("Requisição idempotente armazenada: {Chave}", chave);
            }

            // Copia a resposta de volta
            respostaMemory.Position = 0;
            await respostaMemory.CopyToAsync(respostaOriginal);
        }
        finally
        {
            context.Response.Body = respostaOriginal;
        }
    }
}
