# API Contract: Tasks

**Provider**: json-server ^1.0  
**Base URL (dev)**: `http://localhost:3000` (acessado via proxy Angular em `/api`)  
**Proxy prefix**: `/api/tasks` → `http://localhost:3000/tasks`  
**Date**: 2026-04-05

---

## Resource: `/tasks`

### GET /tasks — Listar tarefas (com filtros opcionais)

Retorna todas as tarefas. Filtros são aplicados via query parameters. Múltiplos parâmetros são combinados com AND implícito.

**Query Parameters** (todos opcionais):

| Parameter | Type | Example | Description |
|-----------|------|---------|-------------|
| `priority` | string | `Alta` | Filtra por prioridade exata |
| `status` | string | `Pendente` | Filtra por status exato |
| `dueDate` | string | `2026-04-10` | Filtra por data de conclusão exata (YYYY-MM-DD) |

**Request**:
```
GET /api/tasks?priority=Alta&status=Pendente
```

**Response 200 OK**:
```json
[
  {
    "id": 1,
    "title": "Estudar Angular",
    "description": "Estudar standalone components",
    "dueDate": "2026-04-10",
    "priority": "Alta",
    "status": "Pendente",
    "createdAt": "2026-04-05T10:00:00.000Z"
  }
]
```

> Retorna array vazio `[]` quando nenhuma tarefa satisfaz os filtros.

---

### POST /tasks — Criar tarefa

**Request Body**:
```json
{
  "title": "Estudar Angular",
  "description": "Foco em standalone components",
  "dueDate": "2026-04-10",
  "priority": "Alta",
  "status": "Pendente",
  "createdAt": "2026-04-05T10:00:00.000Z"
}
```

> `id` não deve ser enviado — gerado automaticamente pelo json-server.  
> `createdAt` deve ser gerado pelo Angular no momento da criação (`new Date().toISOString()`).

**Response 201 Created**:
```json
{
  "id": 1,
  "title": "Estudar Angular",
  "description": "Foco em standalone components",
  "dueDate": "2026-04-10",
  "priority": "Alta",
  "status": "Pendente",
  "createdAt": "2026-04-05T10:00:00.000Z"
}
```

---

### PUT /tasks/:id — Atualizar tarefa completa

Substitui todos os campos da tarefa. Usado no formulário de edição completa.

**Request**:
```
PUT /api/tasks/1
```

**Request Body**: objeto `Task` completo (todos os campos, exceto `id`).

**Response 200 OK**: objeto `Task` atualizado completo.

---

### PATCH /tasks/:id — Atualizar status (inline)

Atualiza apenas o campo `status`. Usado na atualização inline da listagem (US3).

**Request**:
```
PATCH /api/tasks/1
```

**Request Body**:
```json
{
  "status": "EmAndamento"
}
```

**Response 200 OK**: objeto `Task` completo com o campo atualizado.

---

### DELETE /tasks/:id — Excluir tarefa

**Request**:
```
DELETE /api/tasks/1
```

**Response 200 OK**: objeto `Task` excluído.

---

## Error Handling Notes

json-server retorna:
- `404 Not Found` quando o `id` não existe.
- `400 Bad Request` em body malformado.

O Angular `TaskService` deve tratar esses casos e propagar mensagem amigável ao componente.

---

## Sorting (padrão)

json-server 1.x suporta ordenação via `_sort` e `_order`:

```
GET /api/tasks?_sort=dueDate&_order=asc
```

A listagem padrão (sem filtros ativos) deve usar `_sort=dueDate&_order=asc`, conforme assumption da spec.
