import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task, Status, Priority } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-list.component.html'
})
export class TaskListComponent {
  @Input() tasks: Task[] = [];
  @Output() editTask = new EventEmitter<Task>();
  @Output() deleteTask = new EventEmitter<number>();
  @Output() statusChanged = new EventEmitter<{ id: number; status: Status }>();

  readonly today = new Date().toISOString().split('T')[0];

  readonly priorityLabels: Record<Priority, string> = {
    [Priority.Baixa]: 'Baixa',
    [Priority.Media]: 'Média',
    [Priority.Alta]: 'Alta'
  };

  readonly statusLabels: Record<Status, string> = {
    [Status.Pendente]: 'Pendente',
    [Status.EmAndamento]: 'Em andamento',
    [Status.Concluida]: 'Concluída'
  };

  readonly statusOptions = Object.values(Status);
  readonly Priority = Priority;
  readonly Status = Status;

  isOverdue(dueDate: string): boolean {
    return dueDate < this.today;
  }

  onEditClick(task: Task): void {
    this.editTask.emit(task);
  }

  onDeleteClick(taskId: number): void {
    this.deleteTask.emit(taskId);
  }

  onStatusChange(task: Task, event: Event): void {
    const newStatus = (event.target as HTMLSelectElement).value as Status;
    this.statusChanged.emit({ id: task.id!, status: newStatus });
  }
}
