using System.Net;
using System.Text;
using CepApi.Exceptions;
using CepApi.Services;
using Xunit;

namespace CepApi.Tests;

public sealed class ViaCepClientTests
{
    [Fact]
    public async Task BuscarEnderecoAsync_ComRespostaEncontrada_MapeiaEndereco()
    {
        using var httpClient = CreateHttpClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent("""
                {
                  "cep": "85801-000",
                  "logradouro": "Rua XV de Novembro",
                  "bairro": "Centro",
                  "localidade": "Cascavel",
                  "uf": "PR"
                }
                """)
        });

        var client = new ViaCepClient(httpClient);

        var endereco = await client.BuscarEnderecoAsync("85801000", CancellationToken.None);

        Assert.NotNull(endereco);
        Assert.Equal("85801-000", endereco.Cep);
        Assert.Equal("Cascavel", endereco.Cidade);
        Assert.Equal("PR", endereco.Estado);
    }

    [Fact]
    public async Task BuscarEnderecoAsync_ComErroTrue_RetornaNull()
    {
        using var httpClient = CreateHttpClient(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent("""{ "erro": true }""")
        });

        var client = new ViaCepClient(httpClient);

        var endereco = await client.BuscarEnderecoAsync("00000000", CancellationToken.None);

        Assert.Null(endereco);
    }

    [Fact]
    public async Task BuscarEnderecoAsync_ComStatusNaoSucesso_LancaServicoIndisponivel()
    {
        using var httpClient = CreateHttpClient(new HttpResponseMessage(HttpStatusCode.BadGateway));
        var client = new ViaCepClient(httpClient);

        await Assert.ThrowsAsync<ViaCepUnavailableException>(
            () => client.BuscarEnderecoAsync("85801000", CancellationToken.None));
    }

    private static HttpClient CreateHttpClient(HttpResponseMessage response)
    {
        return new HttpClient(new StubHttpMessageHandler(response))
        {
            BaseAddress = new Uri("https://viacep.com.br/")
        };
    }

    private static StringContent JsonContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private sealed class StubHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}
