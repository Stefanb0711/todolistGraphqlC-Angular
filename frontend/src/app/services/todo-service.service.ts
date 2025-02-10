import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {TodoElementComponent} from '../todo-element/todo-element.component';
import {TodolistModel} from '../models/Todolist.model';
import {AuthenticationService} from './auth-service.service';
import {TodoModel} from '../models/Todo.model';

@Injectable({
  providedIn: 'root',
})
export class TodoService {

  constructor(private httpServ: HttpClient, private authServ: AuthenticationService) {
  }

  private apiUrl = "https://localhost:7188/graphql";

  currentTodolists: TodolistModel[] = [];

  currentTodos: TodoModel[] = [];

  currentTodolistId: string | undefined;
  currentTodoId: string | undefined;




  deleteTodolist() {
    return this.httpServ.delete<any>(`${this.apiUrl}/delete-todolist/${this.currentTodolistId}`);
  }

  deleteTodo() {
    return this.httpServ.delete<any>(`${this.apiUrl}/delete-todo/${this.currentTodoId}`);
  }

  addTodo(todo: TodoModel) {
    return this.httpServ.post<any>(`${this.apiUrl}/add-todo`, todo);
  }
  addTodolist(todo: TodolistModel) {
      // Add logic here
      return this.httpServ.post<any>(`${this.apiUrl}/add-todolist`, todo);
  }

  getAllTodoLists() {
      return this.httpServ.get<any>(`${this.apiUrl}/get-all-todolists`);
  }

  getAllTodos() {
    const payload = {todolistId: this.currentTodolistId};

    return this.httpServ.post<any>(`${this.apiUrl}/get-todos`, payload);
  }


}
