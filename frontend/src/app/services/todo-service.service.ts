import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {TodoElementComponent} from '../todo-element/todo-element.component';
import {TodolistModel} from '../models/Todolist.model';
import {AuthenticationService} from './auth-service.service';
import {TodoModel} from '../models/Todo.model';
import {GraphQLClient} from 'graphql-request';
//import {variable} from '@angular/compiler';

@Injectable({
  providedIn: 'root',
})
export class TodoService {

  private client: GraphQLClient;

  constructor(private httpServ: HttpClient, private authServ: AuthenticationService) {
    this.client = new GraphQLClient(this.apiUrl);

  }

  private apiUrl = "https://localhost:7188/graphql";

  currentTodolists: TodolistModel[] = [];

  currentTodos: TodoModel[] = [];

  currentTodolistId: string | undefined;
  currentTodoId: string | undefined;

  todoErrors: string | null = null;

  async deleteTodolist() {
    const mutation = `
     mutation DeleteTodolist($todolistId: String!, $password: String!) {
      deleteTodolist($todolistId)
     } {
     success
     message
       }
     }
    `;

    const variables = {
      todolistId: this.currentTodolistId
    }

    try {
      const response: any = await this.client.request(mutation, variables);
      return response;
    } catch (error) {
      console.error('Fehler bei der Mutation DeleteTodolist:', error);
      throw error;
    }

  }


  async deleteTodo() {
    const mutation = `
    mutation DeleteTodo($todoId: String!) {
      deleteTodo($todoId){
        success
        message
        todolistId
      }
    }
    `;


    const variables = {
      todoId: this.currentTodoId
    };

    try {
      const response: any = await this.client.request(mutation, variables);
      return response;
    } catch (error) {
      console.error('Fehler bei der Query');
      throw error;
    }

  }


  /*
  deleteTodolist() {
    return this.httpServ.delete<any>(`${this.apiUrl}/delete-todolist/${this.currentTodolistId}`);
  }

  deleteTodo() {
    return this.httpServ.delete<any>(`${this.apiUrl}/delete-todo/${this.currentTodoId}`);
  }
  */

  async addTodo(todo: TodoModel){
    const mutation = `
      mutation AddTodo($content: String!, $date: String!, $todolistId: String!) {
        addTodo(todo: {
          content: $content,
          date: $date,
          todolistId: $todolistId
        }) {
          success
          message
        }
      }
    `;

    const variables = {
      content: todo.content,
      todolistId: todo.todolistId,
      date: todo.date
    };

    try {
        const data: any = await this.client.request(mutation, variables);
        return data;
      } catch (error) {
        console.error('Fehler bei der Mutation:', error);
        throw error;
      }

  }

  /*
  addTodo(todo: TodoModel) {
    return this.httpServ.post<any>(`${this.apiUrl}/add-todo`, todo);
  }*/

  async addTodolist(todolist: TodolistModel) {
    const mutation = `
      mutation AddTodolist($userId: String! , $name: String!  , $date: Number!){
        addTodoList(input: {
          userId: $userId,
          name: $name,
          date: $date
          }) {
          success
          message
        }
      }
    `;

    const variables = {
      userId: todolist.userId,
      name: todolist.name,
      date: todolist.date
    };

    try {
      const data: any = await this.client.request(mutation, variables);
      return data;
    } catch (error) {
      console.error('Fehler bei der Mutation:', error);
      throw error;
    }

  }

  /*
  addTodolist(todo: TodolistModel) {
      // Add logic here
      return this.httpServ.post<any>(`${this.apiUrl}/add-todolist`, todo);
  }*/



  async getAllTodoLists() {
    const query = `
      query GetAllTodolists {
        getAllTodolists() {

        }
      }
    `;
    try {
      const data: any = await this.client.request(query);
      return data;
    } catch (error) {
      console.error('Fehler bei der Query:', error);
      throw error;
    }

  }

  /*
  getAllTodoLists() {
      return this.httpServ.get<any>(`${this.apiUrl}/get-all-todolists`);
  }*/

  getAllTodos() {
    const payload = {todolistId: this.currentTodolistId};

    return this.httpServ.post<any>(`${this.apiUrl}/get-todos`, payload);
  }


}
