# Bernhoeft GRT — Avisos API

Projeto implementado manualmente seguindo os padrões do desafio (MediatR, Repository, DI, FluentValidation).

## O que foi implementado
- **Endpoints:**
  - `GET /api/v1/avisos/{id}`: Obtém um aviso pelo ID.
  - `GET /api/v1/avisos`: Lista avisos ativos.
  - `POST /api/v1/avisos`: Cria um novo aviso.
  - `PUT /api/v1/avisos`: Atualiza um aviso existente (apenas `Mensagem` é editável).
  - `DELETE /api/v1/avisos/{id}`: Remove um aviso (soft delete).
- **Entidade `AvisoEntity`:** Campos `CriadoEm`, `AtualizadoEm` incluídos.
- **Repositório:**
  - `AvisoRepository`: Inclusão de novos métodos como obter, adicionar, atualizar e remover avisos (soft-delete).
- **CQRS**: Foi seguido o padrão adotado pelo MediatR, para cada rota desenvolvida, foi adicionado um handle e uma classe de Request e Renponse + validator.
- **Validação:** Utiliza FluentValidation integrada para garantir regras como `id>0`, e validação dos campos `Titulo` e `Mensagem` na criação e atualização de avisos.
- **Testes:** Testes de integração e testes unitários foram implementados usando xUnit e Moq (total de 45 testes para garantir 100% de cobertura das funcionalidades desenvolvidas).



> Desenvolvido por: Guilherme Duarte.