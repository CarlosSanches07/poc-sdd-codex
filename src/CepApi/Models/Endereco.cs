namespace CepApi.Models;

public sealed record Endereco(
    string Cep,
    string Logradouro,
    string Bairro,
    string Cidade,
    string Estado);
