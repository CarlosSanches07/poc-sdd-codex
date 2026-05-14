# Instruções para o Claude Code

## Metodologia
Este projeto segue SDD (Specification-Driven Development).
Leia o arquivo SPEC.md antes de qualquer ação.
A spec é a fonte da verdade — não invente comportamentos não especificados.

## Ordem de trabalho obrigatória
1. Ler SPEC.md completamente
2. Criar a estrutura de pastas e arquivos
3. Criar os modelos (Models)
4. Criar as interfaces (contratos internos)
5. Escrever os testes baseados nos casos de uso da SPEC
6. Implementar até todos os testes passarem
7. Confirmar com `dotnet test` antes de encerrar

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
