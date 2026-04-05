# Research: Gerenciador de Tarefas

**Phase**: 0 — Outline & Research  
**Date**: 2026-04-05  
**Feature**: Gerenciador de Tarefas (Angular + json-server)

---

## 1. Angular Standalone Components vs NgModules

**Decision**: Usar Angular Standalone Components (sem NgModules de feature).  
**Rationale**: Desde Angular 17, standalone é o padrão do CLI (`ng new` gera standalone por padrão). Elimina a cerimônia de NgModules para um app simples. Alinha com Princípio III (Simplicity First).  
**Alternatives considered**: NgModules tradicionais — rejeitado por adicionar arquivos e indireções desnecessárias para um app de estudos de escopo único.

---

## 2. Versão Angular

**Decision**: Angular 19 (LTS disponível em abril 2026).  
**Rationale**: Versão estável mais recente com suporte ativo. Standalone components, signals opcionais, `provideHttpClient()` sem `HttpClientModule`.  
**Alternatives considered**: Angular 17/18 — funcionais, mas 19 é o atual default do CLI.

---

## 3. json-server — Versão e Configuração

**Decision**: json-server ^1.0 (API REST sobre `db.json` local, porta 3000).  
**Rationale**: Versão 1.x é a atual estável. API simples: `GET /tasks`, `POST /tasks`, `PUT /tasks/:id`, `DELETE /tasks/:id`. Filtragem por campo via query params (`?priority=Alta&status=Pendente`) é nativa no json-server 1.x. Sem configuração adicional necessária.  
**Alternatives considered**:  
- json-server 0.17.x — versão anterior ainda amplamente usada, mas 1.x é o atual recomendado.  
- SQLite local via melhor-sqlite3 — rejeitado por excesso de complexidade (Princípio III).  
- localStorage no browser — rejeitado: não permite operações REST reais, dificulta migração futura.

---

## 4. Filtros: Client-side vs Server-side (json-server query params)

**Decision**: Filtros aplicados via query params no json-server (server-side).  
**Rationale**: json-server suporta filtro por campo nativamente (`GET /tasks?status=Pendente&priority=Alta`). Mantém o componente de filtro simples — ele apenas emite os parâmetros; o serviço monta a URL. Evita lógica de filtragem duplicada no cliente.  
**Alternatives considered**:  
- Filtrar no cliente após buscar todos os dados — mais simples de implementar mas ignora a capacidade nativa do json-server e não escala.  
- Usar `_like` para busca parcial — não necessário para os filtros exatos deste requisito.

---

## 5. Comunicação Angular ↔ json-server (CORS)

**Decision**: Usar proxy Angular (`proxy.conf.json`) para redirecionar `/api/*` → `http://localhost:3000/*` durante o desenvolvimento.  
**Rationale**: Evita erros de CORS sem precisar configurar headers no json-server. O proxy é nativo do `ng serve` e transparente para o código da aplicação. A URL base fica `environment.apiUrl = '/api'`.  
**Alternatives considered**:  
- `--cors` no json-server — funciona mas não é o padrão Angular; o proxy é mais idiomático.  
- Configurar CORS manualmente no json-server via middleware — complexidade desnecessária (Princípio III).

---

## 6. Formulários Angular: Reactive Forms vs Template-driven

**Decision**: Reactive Forms (`FormGroup`, `FormControl`).  
**Rationale**: Validação programática mais clara (Princípio I — Clarity Over Cleverness). A lógica de validação fica no `.ts`, não escondida em diretivas no HTML. Melhor para campos com validação obrigatória (título).  
**Alternatives considered**: Template-driven forms — mais simples para forms triviais, mas mistura lógica de validação no template (viola Princípio II).

---

## 7. Atualização de Status Inline (US3)

**Decision**: Dropdown `<select>` diretamente na linha da listagem, com evento `(change)` chamando `TaskService.updateStatus(id, novoStatus)` via `PATCH /tasks/:id`.  
**Rationale**: json-server 1.x suporta `PATCH` para atualização parcial. Implementação direta sem modal, alinhada com a US3 (zero cliques extras).  
**Alternatives considered**:  
- Botão de avanço de status (next-state) — mais elegante mas requer lógica de sequência de estados e não permite retroceder.  
- Modal de edição completa — excede o requisito de P3 que pede atualização rápida sem navegação.

---

## 8. Scripts npm (inicialização paralela)

**Decision**: Usar `concurrently` para rodar `json-server` e `ng serve` com um único comando `npm start`.  
**Rationale**: Experiência de desenvolvimento simples — um comando inicia tudo (Princípio III). `concurrently` é o padrão de mercado para esse caso.  
**Alternatives considered**:  
- Dois terminais separados — funciona, mas é mais trabalhoso e propenso a esquecimentos.  
- `npm-run-all` — alternativa válida, mas `concurrently` é mais difundido.

---

## Resoluções de NEEDS CLARIFICATION

Todos os itens do Technical Context foram definidos antes da pesquisa. Nenhum marcador `NEEDS CLARIFICATION` permanece.
