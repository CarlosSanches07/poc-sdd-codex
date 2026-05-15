# Instruções para o Claude Code

## Metodologia
Este projeto segue SDD (Specification-Driven Development).
Leia o arquivo SPEC.md antes de qualquer ação.
A spec é a fonte da verdade — não invente comportamentos não especificados.

## Modo de trabalho: INCREMENTAL

O projeto já possui implementação existente. Siga estas regras:

1. **Antes de qualquer coisa**, rode `dotnet test` e confirme que os testes existentes passam
2. Leia o CHANGELOG.md e identifique apenas o que está em `[Unreleased]`
3. Implemente **somente** o que está listado em "Adicionado"
4. Não altere nada listado em "Não alterar"
5. Ao final, rode `dotnet test` novamente — todos os testes (antigos + novos) devem passar
6. Se um teste antigo quebrar, **pare e reporte** — não tente corrigir silenciosamente

## Ordem de trabalho incremental
1. `dotnet test` → confirmar baseline verde
2. Ler CHANGELOG.md `[Unreleased]`
3. Escrever os novos testes
4. Implementar até os novos testes passarem
5. `dotnet test` → confirmar que tudo ainda passa

## Restrições
- Não usar banco de dados
- Não adicionar dependências além das necessárias
- Não implementar endpoints não descritos na SPEC
- Todo erro deve seguir o schema Erro da SPEC

## Estrutura esperada
CepApi/
  src/CepApi/          ← projeto principal
  tests/CepApi.Tests/  ← projeto de testes
  Dockerfile
  docker-compose.yml

## Regras de implementação — incremento atual

- O early return do caso 5 deve ocorrer na mesma camada
  das demais validações de formato (antes de chamar o service)
- O timeout do HttpClient deve ser configurado via
  `HttpClient.Timeout` no registro do DI (Program.cs),
  não dentro do ViaCepService
- Não criar novos arquivos desnecessários — reaproveite
  o que já existe
