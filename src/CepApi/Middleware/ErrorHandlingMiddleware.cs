using CepApi.Exceptions;
using CepApi.Models;

namespace CepApi.Middleware;

public sealed class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CepInvalidException)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                new Erro("CEP_INVALIDO", "O CEP informado não possui formato válido"));
        }
        catch (CepNotFoundException)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status404NotFound,
                new Erro("CEP_NAO_ENCONTRADO", "CEP não encontrado"));
        }
        catch (ViaCepUnavailableException)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status502BadGateway,
                new Erro("SERVICO_INDISPONIVEL", "Serviço indisponível"));
        }
        catch (Exception)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status502BadGateway,
                new Erro("SERVICO_INDISPONIVEL", "Serviço indisponível"));
        }
    }

    private static Task WriteErrorAsync(HttpContext context, int statusCode, Erro erro)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(erro);
    }
}
