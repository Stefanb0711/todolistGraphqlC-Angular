import { Component } from '@angular/core';
import {TodoService} from '../../services/todo-service.service';
import {TodoModel} from '../../models/Todo.model';

@Component({
  selector: 'app-todo-input-field',
  standalone: false,

  templateUrl: './todo-input-field.component.html',
  styleUrl: './todo-input-field.component.css'
})
export class TodoInputFieldComponent {

  constructor(private todoServ: TodoService) {
  }

  newTodo: TodoModel = {
    id: "",
    content: "",
    todolistId: "",
    date: undefined
  };


  onAddTodo() {

    this.newTodo.todolistId = this.todoServ.currentTodolistId;

    try {
      const response: any = this.todoServ.addTodo(this.newTodo);
      if (response.success) {
        this.todoServ.currentTodolists = response.allTodolists
      } else {
        this.todoServ.todoErrors = response.message;
      }
    } catch (error) {

    }
  }
    /*
    this.todoServ.addTodo(this.newTodo).subscribe({
      next: (res: any) => {

        console.log("Antwort beim hinzufügen von neuem Todo: ", res);
        this.todoServ.currentTodos = res;
        this.newTodo.content = '';
      }, error: (res: any) => {
        console.log("Fehler beim Hinzufügen von neuem Todo");
      }
    })
  }
  */

}
