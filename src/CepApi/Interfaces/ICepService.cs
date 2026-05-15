using CepApi.Models;

namespace CepApi.Interfaces;

public interface ICepService
{
    Task<Endereco> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken);
}
