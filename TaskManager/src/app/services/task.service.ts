import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Task, TaskFilter, Status } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tasks`;

  getTasks(filter: TaskFilter | null): Observable<Task[]> {
    let params = new HttpParams()
      .set('_sort', 'dueDate')
      .set('_order', 'asc');

    if (filter?.priority) {
      params = params.set('priority', filter.priority);
    }
    if (filter?.status) {
      params = params.set('status', filter.status);
    }
    if (filter?.dueDate) {
      params = params.set('dueDate', filter.dueDate);
    }

    return this.http.get<Task[]>(this.baseUrl, { params });
  }

  createTask(task: Omit<Task, 'id'>): Observable<Task> {
    return this.http.post<Task>(this.baseUrl, task);
  }

  updateTask(id: number, task: Omit<Task, 'id'>): Observable<Task> {
    return this.http.put<Task>(`${this.baseUrl}/${id}`, task);
  }

  patchStatus(id: number, status: Status): Observable<Task> {
    return this.http.patch<Task>(`${this.baseUrl}/${id}`, { status });
  }

  deleteTask(id: number): Observable<Task> {
    return this.http.delete<Task>(`${this.baseUrl}/${id}`);
  }
}
