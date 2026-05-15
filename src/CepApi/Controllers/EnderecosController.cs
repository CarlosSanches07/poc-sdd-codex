using CepApi.Interfaces;
using CepApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CepApi.Controllers;

[ApiController]
[Route("endereco")]
public sealed class EnderecosController(ICepService cepService) : ControllerBase
{
    [HttpGet("{cep}")]
    public async Task<ActionResult<Endereco>> BuscarEnderecoAsync(string cep, CancellationToken cancellationToken)
    {
        var endereco = await cepService.BuscarEnderecoAsync(cep, cancellationToken);
        return Ok(endereco);
    }
}
