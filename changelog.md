# Changelog

## incremento anterior

### Alterado

**Caso de uso 5 — Early return para CEP inválido semanticamente**
- CEPs compostos apenas por zeros ("00000000") devem ser rejeitados
  antes de qualquer chamada ao ViaCEP
- Retornar 404 com codigo "CEP_NAO_ENCONTRADO" diretamente na camada de validação
- Atualizar o teste existente do caso 5 para garantir que o ViaCEP
  NÃO é chamado nessa situação

**Caso de uso 6 — Timeout no HttpClient**
- Configurar timeout de 1 segundo no HttpClient usado pelo ViaCepService
- Se o ViaCEP não responder em 1 segundo, lançar exceção mapeada para
  502 SERVICO_INDISPONIVEL
- Atualizar o teste existente do caso 6 para simular timeout
  (e não apenas indisponibilidade)

### Não alterar
- Casos de uso 1, 2, 3, 4, 7 — já implementados e com testes passando
- Estrutura de Controllers, Models e Exceptions — não refatorar
- Testes existentes dos casos acima — não remover nem modificar

## incremento anterior

### Alterado

**Caso de uso 5 - Garantir retorno de CEP não encontrado o teste está validando somente chamadas ao VIACEP

### Não alterar
- Casos de usos diferente de 5 - já estão corretos
- Controllers, Models, Exceptions e Interfaces - não refatorar
- Não alterar nenhum teste que esteja fora do caso do uso 5


## incremento anterior

### Alterado

**Caso de uso 8 - implmentar teste e arquivo de withelist caso não exista
**O cep na whitelist deve ser 66666666

### Não alterar
- Casos de usos diferente de 8  - já estão corretos
- Controllers, Models - não refatorar
- Não alterar nenhum teste que esteja fora do caso do uso 8
