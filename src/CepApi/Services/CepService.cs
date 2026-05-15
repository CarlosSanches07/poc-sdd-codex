using CepApi.Exceptions;
using CepApi.Interfaces;
using CepApi.Models;
using System.Text.Json;

namespace CepApi.Services;

public sealed class CepService(IViaCepClient viaCepClient) : ICepService
{
    private static readonly string[] BlacklistedCeps = LoadBlacklistedCeps();
    private static readonly string[] WhitelistedCeps = LoadWhitelistedCeps();
    private static readonly Endereco WhitelistedEndereco = new(
        "66666666",
        "Rua Dale",
        "Dale",
        "Dale",
        "TT");

    public async Task<Endereco> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken)
    {
        if (cep.Length != 8 || !cep.All(char.IsDigit))
        {
            throw new CepInvalidException();
        }

        if (BlacklistedCeps.Contains(cep))
        {
            throw new CepBlacklistedException();
        }

        if (cep == "00000000")
        {
            throw new CepNotFoundException();
        }

        if (WhitelistedCeps.Contains(cep))
        {
            return WhitelistedEndereco;
        }

        var endereco = await viaCepClient.BuscarEnderecoAsync(cep, cancellationToken);
        return endereco ?? throw new CepNotFoundException();
    }

    private static string[] LoadBlacklistedCeps()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "blackList.json");
        if (!File.Exists(filePath))
        {
            return [];
        }

        return JsonSerializer.Deserialize<string[]>(File.ReadAllText(filePath)) ?? [];
    }

    private static string[] LoadWhitelistedCeps()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "whiteList.json");
        if (!File.Exists(filePath))
        {
            return [];
        }

        return JsonSerializer.Deserialize<string[]>(File.ReadAllText(filePath)) ?? [];
    }
}
