# Especificação — CEP API

## Visão Geral
API REST em .NET 8 que recebe um CEP e retorna o endereço completo
consultando a API pública do ViaCEP (https://viacep.com.br).

## Stack
- .NET 8 (ASP.NET Core Web API)
- xUnit para testes
- Sem banco de dados (stateless)

## Endpoint

GET /endereco/{cep}

### Parâmetros
| Nome | Tipo   | Local | Regra                        |
|------|--------|-------|------------------------------|
| cep  | string | path  | Exatamente 8 dígitos numéricos |

### Respostas

| Status | Quando                              | Body schema  |
|--------|-------------------------------------|--------------|
| 200    | CEP válido e encontrado             | Endereco     |
| 400    | CEP com formato inválido            | Erro         |
| 404    | CEP não existe no ViaCEP            | Erro         |
| 502    | ViaCEP indisponível ou com erro     | Erro         |

### Schema: Endereco
```json
{
  "cep": "85801-000",
  "logradouro": "Rua XV de Novembro",
  "bairro": "Centro",
  "cidade": "Cascavel",
  "estado": "PR"
}
```

### Schema: Erro
```json
{
  "codigo": "CEP_INVALIDO",
  "mensagem": "O CEP informado não possui formato válido"
}
```

### Códigos de erro
| codigo                  | status | situação                        |
|-------------------------|--------|---------------------------------|
| CEP_INVALIDO            | 400    | formato incorreto               |
| CEP_NAO_ENCONTRADO      | 404    | CEP não existe no ViaCEP        |
| SERVICO_INDISPONIVEL    | 502    | falha na chamada ao ViaCEP      |

## Comportamentos esperados (casos de uso)

1. CEP "85801000" → 200 com endereço preenchido
2. CEP "1234" (< 8 dígitos) → 400 CEP_INVALIDO
3. CEP "123456789" (> 8 dígitos) → 400 CEP_INVALIDO
4. CEP "8580100A" (contém letra) → 400 CEP_INVALIDO
5. CEP "00000000" (não existe) → 404 CEP_NAO_ENCONTRADO
 - Early return: o ViaCEP não deve ser chamado
6. ViaCEP não responde em 1 segundo → 502 SERVICO_INDISPONIVEL
   - O HttpClient deve ter timeout configurado para 1000ms
   - Timeout deve ser tratado como SERVICO_INDISPONIVEL
7. O Serviço deve validar uma arquivo estático chamado blackList.json -> 400 CEP EM BLACKLIST
8. o Serviço deve mockar um resultado padrão para CEPs que estõa no arquivo estático whiteList.json
  - O resultado padrão deve ser no modelo de response com os dados: CEP: 66666666, Endereço: Rua Dale, Numero 66, UF: TT, Cidade: Dale
  - Invente o que faltar

## Regras de implementação

- A chamada ao ViaCEP deve ser feita via HttpClient com injeção de dependência
- O ViaCEP retorna `{ "erro": true }` quando o CEP não existe — tratar isso como 404
- Usar middleware global para tratamento de exceções (não try/catch no controller)
- Todos os casos de uso acima devem ter testes automatizados com mocks do ViaCEP
- O projeto deve compilar e todos os testes devem passar com `dotnet test`
