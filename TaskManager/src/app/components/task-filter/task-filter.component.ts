import { Component, Output, EventEmitter, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { TaskFilter, Priority, Status } from '../../models/task.model';

@Component({
  selector: 'app-task-filter',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-filter.component.html'
})
export class TaskFilterComponent {
  private readonly fb = inject(FormBuilder);

  @Output() filterChange = new EventEmitter<TaskFilter>();

  readonly priorityOptions = [
    { value: Priority.Baixa, label: 'Baixa' },
    { value: Priority.Media, label: 'Média' },
    { value: Priority.Alta, label: 'Alta' }
  ];

  readonly statusOptions = [
    { value: Status.Pendente, label: 'Pendente' },
    { value: Status.EmAndamento, label: 'Em andamento' },
    { value: Status.Concluida, label: 'Concluída' }
  ];

  form = this.fb.group({
    priority: [null as Priority | null],
    status: [null as Status | null],
    dueDate: [null as string | null]
  });

  constructor() {
    this.form.valueChanges.subscribe((values) => {
      this.filterChange.emit({
        priority: values.priority ?? null,
        status: values.status ?? null,
        dueDate: values.dueDate ?? null
      });
    });
  }

  clearFilters(): void {
    this.form.reset({ priority: null, status: null, dueDate: null });
  }
}
