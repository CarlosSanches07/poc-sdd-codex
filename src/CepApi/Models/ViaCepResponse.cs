using System.Text.Json.Serialization;

namespace CepApi.Models;

public sealed record ViaCepResponse
{
    public string? Cep { get; init; }
    public string? Logradouro { get; init; }
    public string? Bairro { get; init; }

    [JsonPropertyName("localidade")]
    public string? Cidade { get; init; }

    [JsonPropertyName("uf")]
    public string? Estado { get; init; }

    [JsonPropertyName("erro")]
    public bool Erro { get; init; }
}
