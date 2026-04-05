import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from './services/task.service';
import { Task, TaskFilter } from './models/task.model';
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskFormComponent } from './components/task-form/task-form.component';
import { TaskFilterComponent } from './components/task-filter/task-filter.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, TaskListComponent, TaskFormComponent, TaskFilterComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private readonly taskService = inject(TaskService);

  tasks: Task[] = [];
  showForm = false;
  selectedTask: Task | null = null;
  activeFilter: TaskFilter | null = null;

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getTasks(this.activeFilter).subscribe({
      next: (tasks) => (this.tasks = tasks),
      error: (err) => console.error('Erro ao carregar tarefas:', err)
    });
  }

  onNewTask(): void {
    this.selectedTask = null;
    this.showForm = true;
  }

  onEditTask(task: Task): void {
    this.selectedTask = task;
    this.showForm = true;
  }

  onSaved(taskData: Omit<Task, 'id'>): void {
    if (this.selectedTask?.id != null) {
      this.taskService.updateTask(this.selectedTask.id, taskData).subscribe({
        next: () => {
          this.showForm = false;
          this.selectedTask = null;
          this.loadTasks();
        },
        error: (err) => console.error('Erro ao atualizar tarefa:', err)
      });
    } else {
      this.taskService.createTask(taskData).subscribe({
        next: () => {
          this.showForm = false;
          this.loadTasks();
        },
        error: (err) => console.error('Erro ao criar tarefa:', err)
      });
    }
  }

  onCancelled(): void {
    this.showForm = false;
    this.selectedTask = null;
  }

  onDeleteTask(taskId: number): void {
    if (!confirm('Deseja excluir esta tarefa?')) {
      return;
    }
    this.taskService.deleteTask(taskId).subscribe({
      next: () => this.loadTasks(),
      error: (err) => console.error('Erro ao excluir tarefa:', err)
    });
  }

  onStatusChanged(event: { id: number; status: import('./models/task.model').Status }): void {
    this.taskService.patchStatus(event.id, event.status).subscribe({
      next: (updatedTask) => {
        const index = this.tasks.findIndex((t) => t.id === updatedTask.id);
        if (index !== -1) {
          this.tasks = [
            ...this.tasks.slice(0, index),
            updatedTask,
            ...this.tasks.slice(index + 1)
          ];
        }
      },
      error: (err) => console.error('Erro ao atualizar status:', err)
    });
  }

  onFilterChange(filter: TaskFilter): void {
    this.activeFilter = filter;
    this.loadTasks();
  }
}

