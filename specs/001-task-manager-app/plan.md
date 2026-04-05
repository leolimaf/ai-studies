# Implementation Plan: Gerenciador de Tarefas

**Branch**: `001-task-manager-app` | **Date**: 2026-04-05 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-task-manager-app/spec.md`

## Summary

Aplicação web Angular que permite gerenciar tarefas pessoais (criar, editar, excluir, filtrar e atualizar status). A persistência é feita via json-server, que expõe uma REST API local sobre um arquivo `db.json`. Não há autenticação nem multiusuário — escopo single-user, local.

## Technical Context

**Language/Version**: TypeScript 5.x / Angular 19  
**Primary Dependencies**: Angular 19 (standalone components), json-server ^1.0  
**Storage**: json-server (REST API local sobre `db.json`)  
**Testing**: N/A — no automated tests (Constitution Principle V)  
**Target Platform**: Navegador web moderno (Chrome/Firefox/Edge) + Node.js para o json-server  
**Project Type**: Web application (SPA frontend + API local)  
**Performance Goals**: Resposta de operações CRUD < 200ms (local)  
**Constraints**: Dados locais apenas; sem autenticação; sem sincronização  
**Scale/Scope**: Single-user; dados limitados ao arquivo `db.json` local

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Notes |
|-----------|--------|-------|
| I. Clarity Over Cleverness | ✅ PASS | Componentes, serviços e modelos nomeados por domínio (`TaskListComponent`, `TaskService`, `Task`). Sem lógica comprimida. |
| II. Single Responsibility | ✅ PASS | `TaskService` exclusivo para HTTP; componentes exclusivos para exibição. Nenhum componente acessa o json-server diretamente. |
| III. Simplicity First (YAGNI) | ✅ PASS | json-server é a solução mais simples para persistência local. Standalone components — sem NgModules desnecessários. Sem roteamento (app single-page simples). |
| IV. Consistent Naming and Style | ✅ PASS (com nota) | Convenção TypeScript: PascalCase para classes/interfaces/enums, camelCase para propriedades e métodos — equivalente ao C# da constituição. Formatação via `ng lint` + ESLint/Prettier. |
| V. No Automated Testing | ✅ PASS | `ng new` gera arquivos `.spec.ts` por padrão — esses arquivos DEVEM ser removidos ao criar o projeto. Nenhum framework de testes será instalado. |

## Project Structure

### Documentation (this feature)

```text
specs/001-task-manager-app/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output
│   └── tasks-api.md
└── tasks.md             # Phase 2 output (/speckit.tasks — NOT created here)
```

### Source Code (repository root)

```text
TaskManager/                        # Angular project root (criado via ng new)
├── db.json                         # json-server database (dados das tarefas)
├── package.json                    # npm scripts: start:api, start:app, start
├── angular.json
├── tsconfig.json
└── src/
    ├── main.ts
    ├── app/
    │   ├── app.component.ts        # Root component — renderiza layout e task-filter + task-list
    │   ├── app.component.html
    │   ├── models/
    │   │   └── task.model.ts       # Interface Task + enums Priority, Status
    │   ├── services/
    │   │   └── task.service.ts     # TaskService — todas as chamadas HTTP ao json-server
    │   └── components/
    │       ├── task-list/
    │       │   ├── task-list.component.ts
    │       │   └── task-list.component.html
    │       ├── task-form/
    │       │   ├── task-form.component.ts   # Formulário de criação e edição
    │       │   └── task-form.component.html
    │       └── task-filter/
    │           ├── task-filter.component.ts  # Controles de filtro
    │           └── task-filter.component.html
    └── environments/
        └── environment.ts          # API_URL para o json-server
```

**Structure Decision**: Web application com Angular SPA (standalone components, sem roteamento) + json-server como API REST local. Estrutura plana e direta, sem sub-módulos nem lazy loading — escopo de estudos, complexidade mínima.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

Nenhuma violação identificada. Não há itens nesta seção.
