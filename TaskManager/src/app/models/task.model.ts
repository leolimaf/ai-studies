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
