using CepApi.Exceptions;
using CepApi.Interfaces;
using CepApi.Models;

namespace CepApi.Services;

public sealed class CepService(IViaCepClient viaCepClient) : ICepService
{
    public async Task<Endereco> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken)
    {
        if (cep.Length != 8 || !cep.All(char.IsDigit))
        {
            throw new CepInvalidException();
        }

        if (cep == "00000000")
        {
            throw new CepNotFoundException();
        }

        var endereco = await viaCepClient.BuscarEnderecoAsync(cep, cancellationToken);
        return endereco ?? throw new CepNotFoundException();
    }
}
