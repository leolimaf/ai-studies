# Feature Specification: Gerenciador de Tarefas

**Feature Branch**: `001-task-manager-app`  
**Created**: 2026-04-05  
**Status**: Draft  
**Input**: User description: "crie uma aplicação que auxilie seus usuários a gerenciar suas tarefas. As tarefas devem ter título, descrição, data de conclusão, prioridade e status (pendente, em andamento, concluída). Inclua uma opção de filtros para visualizar as tarefas por prioridade, status ou data de conclusão."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Gerenciar Tarefas (Criar, Editar, Excluir) (Priority: P1)

O usuário acessa a aplicação e pode criar novas tarefas informando título, descrição, data de conclusão, prioridade e status inicial. Ele também pode editar qualquer campo de uma tarefa existente e excluir tarefas que não são mais necessárias.

**Why this priority**: Sem a capacidade de criar e manter tarefas, a aplicação não entrega nenhum valor. Esta é a funcionalidade central que torna tudo o mais possível.

**Independent Test**: Abrir a aplicação, criar uma tarefa com todos os campos preenchidos, verificar que ela aparece na listagem, editar o título, salvar e confirmar que o novo título é exibido; depois excluir a tarefa e confirmar que ela desaparece da lista.

**Acceptance Scenarios**:

1. **Given** a aplicação está aberta, **When** o usuário preenche título, descrição, data de conclusão, prioridade e status e confirma a criação, **Then** a nova tarefa aparece na listagem com todos os dados informados.
2. **Given** uma tarefa existe na listagem, **When** o usuário edita o título e salva, **Then** a tarefa exibe o título atualizado.
3. **Given** uma tarefa existe na listagem, **When** o usuário solicita a exclusão e confirma, **Then** a tarefa é removida permanentemente da listagem.
4. **Given** o usuário tenta criar uma tarefa sem título, **When** ele confirma a criação, **Then** a aplicação exibe uma mensagem de erro indicando que o título é obrigatório.

---

### User Story 2 - Filtrar Tarefas (Priority: P2)

O usuário pode aplicar filtros para visualizar apenas as tarefas que atendem a um critério específico: por prioridade, por status ou por data de conclusão. Filtros podem ser combinados.

**Why this priority**: Com uma lista crescente de tarefas, encontrar rapidamente o que precisa de atenção é essencial para a produtividade. Filtros transformam a listagem de um arquivo morto em uma ferramenta de trabalho ativa.

**Independent Test**: Com pelo menos três tarefas criadas com diferentes prioridades e status, aplicar o filtro de "Alta prioridade" e confirmar que apenas as tarefas com essa prioridade são exibidas. Remover o filtro e confirmar que todas as tarefas voltam a ser exibidas.

**Acceptance Scenarios**:

1. **Given** existem tarefas com diferentes prioridades, **When** o usuário seleciona o filtro de prioridade "Alta", **Then** apenas tarefas com prioridade Alta são exibidas.
2. **Given** existem tarefas com diferentes status, **When** o usuário seleciona o filtro de status "Em andamento", **Then** apenas tarefas com status "Em andamento" são exibidas.
3. **Given** existem tarefas com diferentes datas de conclusão, **When** o usuário filtra por uma data específica, **Then** apenas tarefas com aquela data de conclusão são exibidas.
4. **Given** filtros estão ativos, **When** o usuário limpa todos os filtros, **Then** a listagem retorna a todas as tarefas.
5. **Given** nenhuma tarefa corresponde aos filtros aplicados, **When** o filtro é aplicado, **Then** a listagem exibe uma mensagem informando que nenhuma tarefa foi encontrada.

---

### User Story 3 - Atualizar Status de uma Tarefa (Priority: P3)

O usuário pode alterar rapidamente o status de uma tarefa (de "Pendente" para "Em andamento", ou de "Em andamento" para "Concluída") diretamente na listagem, sem precisar abrir o formulário de edição completo.

**Why this priority**: Atualizar o status é a ação mais frequente no dia a dia. Facilitar esse fluxo aumenta a agilidade de uso. Essa história depende da P1 para existir, mas agrega conveniência significativa.

**Independent Test**: Com uma tarefa de status "Pendente" na listagem, alterá-la para "Em andamento" diretamente na lista e confirmar que o status é atualizado imediatamente sem navegação adicional.

**Acceptance Scenarios**:

1. **Given** uma tarefa está com status "Pendente", **When** o usuário altera o status diretamente na listagem para "Em andamento", **Then** o status da tarefa é atualizado e refletido imediatamente na interface.
2. **Given** uma tarefa está com status "Em andamento", **When** o usuário a marca como "Concluída", **Then** o status é atualizado e a tarefa pode ser visualmente distinguida das ainda ativas.

---

### Edge Cases

- O que acontece quando o usuário tenta criar uma tarefa com uma data de conclusão no passado? → A aplicação deve aceitar (datas passadas são válidas para registro de tarefas atrasadas), mas pode exibir um aviso visual.
- O que acontece quando não há nenhuma tarefa cadastrada? → A listagem exibe uma mensagem de boas-vindas orientando o usuário a criar a primeira tarefa.
- O que acontece se o título tiver apenas espaços em branco? → A aplicação deve tratar como campo vazio e exibir mensagem de erro.
- O que acontece quando um filtro por data não corresponde a nenhuma tarefa? → Exibir mensagem informando que nenhuma tarefa foi encontrada para os critérios selecionados.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: O sistema DEVE permitir criar uma tarefa com os campos: título (obrigatório), descrição (opcional), data de conclusão (obrigatória), prioridade (obrigatória) e status (obrigatório, padrão: Pendente).
- **FR-002**: O sistema DEVE validar que o título não está vazio ou composto apenas por espaços em branco antes de salvar.
- **FR-003**: O sistema DEVE permitir editar qualquer campo de uma tarefa existente.
- **FR-004**: O sistema DEVE permitir excluir uma tarefa, solicitando confirmação antes da remoção.
- **FR-005**: O sistema DEVE exibir a lista de todas as tarefas cadastradas, mostrando título, prioridade, status e data de conclusão.
- **FR-006**: O sistema DEVE oferecer filtros independentes e combináveis por: prioridade (Baixa, Média, Alta), status (Pendente, Em andamento, Concluída) e data de conclusão.
- **FR-007**: O sistema DEVE permitir limpar todos os filtros ativos, retornando à visualização completa.
- **FR-008**: O sistema DEVE permitir alterar o status de uma tarefa diretamente na listagem.
- **FR-009**: O sistema DEVE persistir as tarefas entre sessões (os dados não se perdem ao fechar e reabrir a aplicação).
- **FR-010**: O sistema DEVE exibir uma mensagem informativa quando a listagem estiver vazia (sem tarefas ou sem resultados para os filtros aplicados).

### Key Entities

- **Tarefa**: Unidade central de gerenciamento. Atributos: título, descrição, data de conclusão, prioridade (Baixa / Média / Alta), status (Pendente / Em andamento / Concluída), data de criação (gerada automaticamente).
- **Filtro**: Critério de seleção aplicado à listagem de tarefas. Pode ser composto por prioridade, status ou data de conclusão, individualmente ou em combinação.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: O usuário consegue criar uma nova tarefa em menos de 1 minuto.
- **SC-002**: O usuário consegue localizar uma tarefa específica usando filtros em menos de 30 segundos.
- **SC-003**: 100% das tarefas criadas persistem corretamente entre sessões; nenhum dado é perdido ao reabrir a aplicação.
- **SC-004**: A atualização de status de uma tarefa na listagem reflete na interface sem nenhuma navegação adicional (zero cliques extras).
- **SC-005**: A aplicação exibe mensagens de erro claras para 100% dos casos de validação (campo obrigatório em branco).

## Assumptions

- A aplicação é de uso pessoal (single-user); não há necessidade de autenticação ou múltiplos perfis de usuário nesta versão.
- A interface será uma aplicação console ou web simples, adequada ao contexto de estudos do projeto.
- A persistência dos dados será local (arquivo ou banco de dados local), sem dependência de serviços externos.
- Não há requisito de sincronização entre dispositivos.
- As prioridades possíveis são fixas: Baixa, Média e Alta — sem criação de prioridades customizadas.
- Os status possíveis são fixos: Pendente, Em andamento e Concluída — sem criação de status personalizados.
- A ordenação padrão da listagem (quando sem filtros) é por data de conclusão crescente.

