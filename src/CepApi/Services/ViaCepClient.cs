using System.Net.Http.Json;
using System.Text.Json;
using CepApi.Exceptions;
using CepApi.Interfaces;
using CepApi.Models;

namespace CepApi.Services;

public sealed class ViaCepClient(HttpClient httpClient) : IViaCepClient
{
    public async Task<Endereco?> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync($"ws/{cep}/json/", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new ViaCepUnavailableException();
            }

            var viaCepResponse = await response.Content.ReadFromJsonAsync<ViaCepResponse>(cancellationToken);
            if (viaCepResponse is null)
            {
                throw new ViaCepUnavailableException();
            }

            if (viaCepResponse.Erro)
            {
                return null;
            }

            return new Endereco(
                viaCepResponse.Cep ?? string.Empty,
                viaCepResponse.Logradouro ?? string.Empty,
                viaCepResponse.Bairro ?? string.Empty,
                viaCepResponse.Cidade ?? string.Empty,
                viaCepResponse.Estado ?? string.Empty);
        }
        catch (ViaCepUnavailableException)
        {
            throw;
        }
        catch (HttpRequestException exception)
        {
            throw new ViaCepUnavailableException(exception);
        }
        catch (TaskCanceledException exception)
        {
            throw new ViaCepUnavailableException(exception);
        }
        catch (JsonException exception)
        {
            throw new ViaCepUnavailableException(exception);
        }
    }
}
