# Data Model: Gerenciador de Tarefas

**Phase**: 1 — Design & Contracts  
**Date**: 2026-04-05

---

## Entities

### Task

Unidade central da aplicação. Representa uma tarefa do usuário.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `id` | `number` | Auto (json-server) | Identificador único gerado pelo json-server |
| `title` | `string` | ✅ | Título da tarefa. Não pode ser vazio ou apenas espaços. |
| `description` | `string \| null` | ❌ | Descrição detalhada. Opcional. |
| `dueDate` | `string` (ISO 8601 date) | ✅ | Data de conclusão. Formato `YYYY-MM-DD`. |
| `priority` | `Priority` | ✅ | Nível de prioridade. Valores fixos. |
| `status` | `Status` | ✅ | Estado atual da tarefa. Padrão: `Pendente`. |
| `createdAt` | `string` (ISO 8601 datetime) | Auto | Data/hora de criação. Gerada no momento do POST. |

---

### Priority (enum)

Valores fixos, sem extensão pelo usuário.

| Value | Label |
|-------|-------|
| `Baixa` | Baixa |
| `Media` | Média |
| `Alta` | Alta |

> Note: valor interno `Media` (sem acento) para evitar problemas em URLs de query string. Label exibida ao usuário: "Média".

---

### Status (enum)

| Value | Label |
|-------|-------|
| `Pendente` | Pendente |
| `EmAndamento` | Em andamento |
| `Concluida` | Concluída |

> Note: valores sem espaços/acentos para compatibilidade com query params do json-server.

---

### TaskFilter

Objeto de filtro composto. Todos os campos são opcionais — apenas os campos preenchidos são enviados como query params.

| Field | Type | Description |
|-------|------|-------------|
| `priority` | `Priority \| null` | Filtra por prioridade. Null = sem filtro. |
| `status` | `Status \| null` | Filtra por status. Null = sem filtro. |
| `dueDate` | `string \| null` | Filtra por data exata (YYYY-MM-DD). Null = sem filtro. |

---

## Validation Rules

| Rule | Field | Condition |
|------|-------|-----------|
| Obrigatório | `title` | Não pode ser vazio ou apenas espaços em branco |
| Obrigatório | `dueDate` | Deve ser uma data válida no formato YYYY-MM-DD |
| Obrigatório | `priority` | Deve ser um dos valores do enum `Priority` |
| Obrigatório | `status` | Deve ser um dos valores do enum `Status` |
| Aviso visual | `dueDate` | Exibir indicador visual se a data for anterior à data atual |

---

## State Transitions

```
Pendente → Em andamento → Concluída
    ↑                          ↓
    └──────────────────────────┘  (qualquer transição é permitida via edição direta)
```

> Qualquer transição de status é permitida (sem fluxo obrigatório unidirecional). O usuário pode alterar para qualquer status a qualquer momento.

---

## TypeScript Representation

```typescript
// src/app/models/task.model.ts

export enum Priority {
  Baixa = 'Baixa',
  Media = 'Media',
  Alta = 'Alta'
}

export enum Status {
  Pendente = 'Pendente',
  EmAndamento = 'EmAndamento',
  Concluida = 'Concluida'
}

export interface Task {
  id?: number;
  title: string;
  description: string | null;
  dueDate: string;
  priority: Priority;
  status: Status;
  createdAt: string;
}

export interface TaskFilter {
  priority: Priority | null;
  status: Status | null;
  dueDate: string | null;
}
```

---

## db.json Structure (json-server)

```json
{
  "tasks": []
}
```

> O json-server cria automaticamente a coleção `tasks` e gerencia `id` auto-incremental. O arquivo `db.json` pode ser pré-populado com dados de exemplo para facilitar o desenvolvimento.
