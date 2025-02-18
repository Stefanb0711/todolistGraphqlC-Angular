import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {TodoElementComponent} from '../todo-element/todo-element.component';
import {TodolistModel} from '../models/Todolist.model';
import {AuthenticationService} from './auth-service.service';
import {TodoModel} from '../models/Todo.model';
import {GraphQLClient} from 'graphql-request';
import {data} from 'autoprefixer';
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

  /*
    mutation {
    deleteTodolist(todolistId: "67850407ad53bd22a4ebb71b") {
      message
      success
    }
  }*/



  async deleteTodolist() {
    const mutation = `
    mutation DeleteTodolist($todolistId: String!) {
      deleteTodolist(todolistId: $todolistId) {
        success
        message
        allTodolists{
          id
          userId
          name
          date
        }
      }
    }
    `;

    const variables = {
      todolistId: this.currentTodolistId
    }

    try {
      const response: any = await this.client.request(mutation, variables);
      console.log("ResponseDeleteTodolist: ", response);
      if (response.deleteTodolist.success) {
        console.log("ResponseDeleteTodolist AllTodolists: ", response);

        this.currentTodolists = response.deleteTodolist.allTodolists;
      }
    } catch (error) {
      console.error('Fehler bei der Mutation DeleteTodolist:', error);
      throw error;
    }

  }


  async deleteTodo() {
    const mutation = `
    mutation DeleteTodo($todoInput: DeleteTodoInput!) {
      deleteTodo(todoInput: $todoInput){
        success
        message
        todolistId
        todos {
          id
          content
          date
          todolistId
        }
      }
    }
    `;

    const variables = {
      todoInput: {
        todoId: this.currentTodoId,
        todolistId: this.currentTodolistId
      },
    };

    try {
      const response: any = await this.client.request(mutation, variables);
      console.log("Response DeleteTodo: ", response);
      this.currentTodos = response.deleteTodo.todos;
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
      mutation AddTodo($todo: AddTodoInput!) {
        addTodo(todo: $todo) {
          success
          message
          todos {
            id,
            content,
            todolistId
          }
        }
      }
    `;

    const variables = {
      todo: {
        content: todo.content,
        todolistId: todo.todolistId,
        //date: todo.date
      }
    };

    try {
        const data: any = await this.client.request(mutation, variables);
        console.log("AddTodo Response: ", data);
        if (data.addTodo.success) {
          this.currentTodos = data.addTodo.todos;
          console.log("CurrentTodos: ", this.currentTodos);
        } else {
          this.todoErrors = data.addTodo.message;
        }
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

  /*
     mutation {
    addTodolist(input: { id: "TO_DO_LIST_ID",
    userId: "USER_ID",
     name: "Name der To-Do-Liste", date: 1647312000000 }) {
      success
      message
      allTodolists {
        id
        userId
        name
        date
      }
    }
  }
*/



  async addTodolist(todolist: TodolistModel) {
    const mutation = `
      mutation AddTodolist($input: AddTodolistInput!) {
        addTodolist(input: $input) {
          success
          message
          allTodolists {
            id
            userId
            name
            date
          }
        }
      }
    `;

    const variables = {
      input: {
        id: todolist.id,
        userId: todolist.userId,
        name: todolist.name,
        date: todolist.date
      }

    };

    try {
      const data: any = await this.client.request(mutation, variables);
      console.log("Antwort beim hinzufügen der Todolist: : ", data);
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
    const mutation = `
    mutation GetAllTodolists($currentUserId: String!) {
      getAllTodolists(currentUserId: $currentUserId) {
        id
        userId
        name
        date
      }
    }`;

    const variables = {
      currentUserId: this.authServ.currentUserId
    };

    try {
      const data: any = await this.client.request(mutation, variables);
      this.currentTodolists = data.getAllTodolists;
      console.log("Antwort von GetAllTodolists : ", data);

      return data;
    } catch (error) {
      console.error('Fehler bei der Query:', error);
      throw error;
    }

  }


  async getTodos() {
    const mutation = `
      mutation GetTodos($todolistId: String!) {
        getTodos(todolistId: $todolistId) {
          id
          content
          date
          todolistId
        }
      }
    `;

    const variables = {
      todolistId: this.currentTodolistId
    };

    try {
      const data: any = await this.client.request(mutation, variables);

      this.currentTodos = data.getTodos;
      console.log("Response von GetTodos: ", data);

      return data;

    } catch (error) {

    }

  }

  /*
  getAllTodoLists() {
      return this.httpServ.get<any>(`${this.apiUrl}/get-all-todolists`);
  }*/




  /*
  getAllTodos() {
    const payload = {todolistId: this.currentTodolistId};

    return this.httpServ.post<any>(`${this.apiUrl}/get-todos`, payload);
  }
  */

}
