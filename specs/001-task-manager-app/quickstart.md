# Quickstart: Gerenciador de Tarefas

**Date**: 2026-04-05  
**Stack**: Angular 19 + json-server ^1.0

---

## Pré-requisitos

- Node.js 20+ (`node -v`)
- Angular CLI 19 (`npm install -g @angular/cli`)
- npm 10+ (`npm -v`)

---

## 1. Criar o projeto Angular

```bash
ng new TaskManager --style=css --ssr=false
cd TaskManager
```

> Quando o CLI perguntar sobre SSR, selecione **No**. Quando perguntar sobre roteamento, também selecione **No** — este é um single-page sem rota.

### Remover arquivos de teste gerados (Princípio V — Sem testes automatizados)

```bash
# Remover todos os arquivos .spec.ts
Get-ChildItem -Recurse -Filter "*.spec.ts" | Remove-Item
```

### Remover configuração de karma/jasmine do angular.json (se presente)

No `angular.json`, remover o bloco `"test"` dentro de `projects > TaskManager > architect`.

---

## 2. Instalar dependências

```bash
# json-server — API local
npm install json-server --save-dev

# concurrently — rodar API e app em paralelo
npm install concurrently --save-dev
```

---

## 3. Configurar scripts no package.json

Adicionar ao `package.json`:

```json
"scripts": {
  "start": "concurrently \"npm run start:api\" \"npm run start:app\"",
  "start:api": "json-server --watch db.json --port 3000",
  "start:app": "ng serve --proxy-config proxy.conf.json",
  "build": "ng build"
}
```

---

## 4. Criar db.json

Na raiz do projeto `TaskManager/`, criar `db.json` com dados de exemplo:

```json
{
  "tasks": [
    {
      "id": 1,
      "title": "Estudar Angular 19",
      "description": "Focar em standalone components e signals",
      "dueDate": "2026-04-15",
      "priority": "Alta",
      "status": "EmAndamento",
      "createdAt": "2026-04-05T09:00:00.000Z"
    },
    {
      "id": 2,
      "title": "Ler documentação do json-server",
      "description": null,
      "dueDate": "2026-04-08",
      "priority": "Media",
      "status": "Pendente",
      "createdAt": "2026-04-05T09:05:00.000Z"
    }
  ]
}
```

---

## 5. Configurar proxy Angular

Na raiz do projeto `TaskManager/`, criar `proxy.conf.json`:

```json
{
  "/api": {
    "target": "http://localhost:3000",
    "secure": false,
    "pathRewrite": { "^/api": "" }
  }
}
```

---

## 6. Configurar environment

Em `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: '/api'
};
```

---

## 7. Iniciar a aplicação

```bash
npm start
```

- App Angular: `http://localhost:4200`  
- API json-server: `http://localhost:3000`

---

## Verificação manual

1. Abrir `http://localhost:4200` — a listagem de tarefas deve exibir as tarefas do `db.json`.
2. Criar uma nova tarefa — ela deve aparecer na lista e ser gravada no `db.json`.
3. Editar o título de uma tarefa — o novo título deve ser exibido na lista.
4. Aplicar filtro por prioridade "Alta" — apenas tarefas Alta devem aparecer.
5. Alterar o status de uma tarefa diretamente na listagem — o campo deve atualizar sem navegação.
6. Excluir uma tarefa — ela deve desaparecer da lista.
7. Fechar e reabrir a aplicação — as tarefas devem persistir (lidas do `db.json`).
