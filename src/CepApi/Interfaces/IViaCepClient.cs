using CepApi.Models;

namespace CepApi.Interfaces;

public interface IViaCepClient
{
    Task<Endereco?> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken);
}
