import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Task, Priority, Status } from '../../models/task.model';

function noWhitespaceValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value as string;
  if (value && value.trim().length === 0) {
    return { whitespace: true };
  }
  return null;
}

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html'
})
export class TaskFormComponent implements OnChanges {
  private readonly fb = inject(FormBuilder);

  @Input() task: Task | null = null;
  @Output() saved = new EventEmitter<Omit<Task, 'id'>>();
  @Output() cancelled = new EventEmitter<void>();

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

  form: FormGroup = this.fb.group({
    title: ['', [Validators.required, noWhitespaceValidator]],
    description: [null],
    dueDate: ['', Validators.required],
    priority: [Priority.Media, Validators.required],
    status: [Status.Pendente, Validators.required]
  });

  get isEditing(): boolean {
    return this.task !== null;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['task'] && this.task) {
      this.form.patchValue({
        title: this.task.title,
        description: this.task.description,
        dueDate: this.task.dueDate,
        priority: this.task.priority,
        status: this.task.status
      });
    } else if (changes['task'] && !this.task) {
      this.form.reset({
        title: '',
        description: null,
        dueDate: '',
        priority: Priority.Media,
        status: Status.Pendente
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const formValue = this.form.value;
    this.saved.emit({
      title: formValue.title.trim(),
      description: formValue.description || null,
      dueDate: formValue.dueDate,
      priority: formValue.priority,
      status: formValue.status,
      createdAt: this.task?.createdAt ?? new Date().toISOString()
    });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
