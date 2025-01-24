import { Component } from '@angular/core';
import {TodoService} from '../../services/todo-service.service';
import {TodolistModel} from '../../models/Todolist.model';
import {AuthenticationService} from '../../services/auth-service.service';

@Component({
  selector: 'app-input-field',
  standalone: false,

  templateUrl: './input-field.component.html',
  styleUrl: './input-field.component.css'
})
export class InputFieldComponent {

  newTodoList: TodolistModel = {
    id: "",
    userId: "",
    name: "",
    date: Date.now()
  };

  /*
  emptyTodolist: TodolistModel = {
    userId: this.authServ.currentUserId,
    name: "",
    date: Date.now()
  }*/

  constructor(private todoServ: TodoService, private authServ: AuthenticationService) {
  }

  //USerid to String
  onAddTodo(){


    this.newTodoList.userId = this.authServ.currentUserId;


    console.log("Todo welches hinzugefügt werden soll: ", this.newTodoList);
    this.todoServ.addTodolist(this.newTodoList).subscribe({
      next: (res: any) => {
        console.log("Antwort von Api beim hinzufügen von Todo: ", res);
        this.todoServ.currentTodolists = res;
        this.newTodoList.name = "";

      }, error: () => {
        console.error('Error adding todo');
      }
    })

  }

}
