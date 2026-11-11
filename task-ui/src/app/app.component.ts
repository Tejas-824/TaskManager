import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from './services/task.service';
import {
  TaskItem,
  TaskPayload,
  TaskPriority,
  TaskStatus,
  TaskSummary
} from './models/task.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule, DatePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  tasks: TaskItem[] = [];
  summary: TaskSummary[] = [];

  filterStatus = '';
  filterPriority = '';
  errorMessage = '';
  showForm = false;

  statuses: TaskStatus[] = ['ToDo', 'InProgress', 'Done'];
  priorities: TaskPriority[] = ['Low', 'Medium', 'High', 'Critical'];
  statusLabels: Record<TaskStatus, string> = {
    ToDo: 'To Do',
    InProgress: 'In Progress',
    Done: 'Done'
  };

  newTask: TaskPayload = this.emptyTask();

  constructor(private taskService: TaskService) {}

  ngOnInit(): void {
    this.refresh();
  }

  loadTasks(): void {
    this.taskService.getTasks(this.filterStatus, this.filterPriority).subscribe({
      next: (data) => (this.tasks = data),
      error: () => (this.errorMessage = 'Could not load tasks. Please check if the API is running.')
    });
  }

  loadSummary(): void {
    this.taskService.getSummary().subscribe({
      next: (data) => (this.summary = data),
      error: () => (this.summary = [])
    });
  }

  createTask(): void {
    if (!this.newTask.title.trim()) {
      this.errorMessage = 'Title is required.';
      return;
    }

    this.taskService.createTask(this.newTask).subscribe({
      next: () => {
        this.newTask = this.emptyTask();
        this.showForm = false;
        this.errorMessage = '';
        this.refresh();
      },
      error: () => (this.errorMessage = 'Could not create the task. Please check the values.')
    });
  }

  changeStatus(task: TaskItem, status: TaskStatus): void {
    const payload: TaskPayload = {
      title: task.title,
      description: task.description ?? '',
      status: status,
      priority: task.priority,
      assignedTo: task.assignedTo ?? ''
    };

    this.taskService.updateTask(task.id, payload).subscribe({
      next: () => this.refresh(),
      error: () => (this.errorMessage = 'Could not update the status.')
    });
  }

  deleteTask(task: TaskItem): void {
    if (!confirm(`Delete "${task.title}"?`)) return;

    this.taskService.deleteTask(task.id).subscribe({
      next: () => this.refresh(),
      error: () => (this.errorMessage = 'Could not delete the task.')
    });
  }

  private refresh(): void {
    this.loadTasks();
    this.loadSummary();
  }

  private emptyTask(): TaskPayload {
    return {
      title: '',
      description: '',
      status: 'ToDo',
      priority: 'Medium',
      assignedTo: ''
    };
  }
}