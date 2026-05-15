using System.Net;
using System.Net.Http.Json;
using CepApi.Exceptions;
using CepApi.Interfaces;
using CepApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace CepApi.Tests;

public sealed class EnderecoEndpointTests
{
    [Fact]
    public async Task GetEndereco_ComCepValidoEncontrado_RetornaEndereco()
    {
        using var factory = CreateFactory((cep, _) => Task.FromResult<Endereco?>(new Endereco(
            "85801-000",
            "Rua XV de Novembro",
            "Centro",
            "Cascavel",
            "PR")));

        var response = await factory.CreateClient().GetAsync("/endereco/85801000");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var endereco = await response.Content.ReadFromJsonAsync<Endereco>();
        Assert.Equal(new Endereco("85801-000", "Rua XV de Novembro", "Centro", "Cascavel", "PR"), endereco);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("123456789")]
    [InlineData("8580100A")]
    public async Task GetEndereco_ComCepInvalido_RetornaCepInvalido(string cep)
    {
        using var factory = CreateFactory((_, _) => Task.FromResult<Endereco?>(null));

        var response = await factory.CreateClient().GetAsync($"/endereco/{cep}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var erro = await response.Content.ReadFromJsonAsync<Erro>();
        Assert.Equal(new Erro("CEP_INVALIDO", "O CEP informado não possui formato válido"), erro);
    }

    [Fact]
    public async Task GetEndereco_ComCepNaoExistente_RetornaNaoEncontrado()
    {
        using var factory = CreateFactory((_, _) => Task.FromResult<Endereco?>(null));

        var response = await factory.CreateClient().GetAsync("/endereco/00000000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var erro = await response.Content.ReadFromJsonAsync<Erro>();
        Assert.Equal("CEP_NAO_ENCONTRADO", erro?.Codigo);
    }

    [Fact]
    public async Task GetEndereco_ComViaCepIndisponivel_RetornaServicoIndisponivel()
    {
        using var factory = CreateFactory((_, _) => throw new ViaCepUnavailableException());

        var response = await factory.CreateClient().GetAsync("/endereco/85801000");

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
        var erro = await response.Content.ReadFromJsonAsync<Erro>();
        Assert.Equal("SERVICO_INDISPONIVEL", erro?.Codigo);
    }

    private static WebApplicationFactory<Program> CreateFactory(
        Func<string, CancellationToken, Task<Endereco?>> buscarEndereco)
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IViaCepClient>();
                services.AddSingleton<IViaCepClient>(new FakeViaCepClient(buscarEndereco));
            });
        });
    }

    private sealed class FakeViaCepClient(
        Func<string, CancellationToken, Task<Endereco?>> buscarEndereco) : IViaCepClient
    {
        public Task<Endereco?> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken)
        {
            return buscarEndereco(cep, cancellationToken);
        }
    }
}
