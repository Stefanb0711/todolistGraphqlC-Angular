import {Component, HostListener, Input, OnInit} from '@angular/core';
import {TodoService} from '../../services/todo-service.service';
import {TodoModel} from '../../models/Todo.model';

@Component({
  selector: 'app-todolist-contextmenu',
  standalone: false,

  templateUrl: './todolist-contextmenu.component.html',
  styleUrl: './todolist-contextmenu.component.css'
})
export class TodolistContextmenuComponent {

  constructor(public todoServ: TodoService) {
  }

  newTodo: TodoModel = {
    id: "",
    content: "",
    todolistId: "",
    date: undefined
  };

  @Input() x: number = 0;
  @Input() y: number = 0;

  @HostListener('document:click', ['$event'])
  clickout(event: any) {
    // Manually added check for outside click to close menu
    this.x = -1;
    this.y = -1;
  }

  @HostListener('document:contextmenu', ['$event'])
  clickoutRightClick(event: any) {
    // Manually added check for outside click to close menu
    this.x = -1;
    this.y = -1;
  }


  contextMenuInfo: any = {
      pageX: 0,
      pageY: 0,
      willContextMenuShow: false
    };


  contextClicked: boolean = false;

  contextMenuOpen = false;

  addNewTodoToTodoList() {

    this.newTodo.todolistId = this.todoServ.currentTodolistId;

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


  onDeleteClick() {
    this.todoServ.deleteTodolist().subscribe({
      next: (res: any) => {
        this.todoServ.currentTodolists = res;
    }, error: (err: any) => {

      }
    })
  }



}
