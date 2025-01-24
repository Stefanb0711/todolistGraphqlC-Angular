import { Component, Input } from '@angular/core';
import {TodoService} from '../services/todo-service.service';

@Component({
  selector: 'app-todo-element',
  standalone: false,

  templateUrl: './todo-element.component.html',
  styleUrl: './todo-element.component.css'
})
export class TodoElementComponent {

  @Input() content: string | undefined;
  @Input() date: number | undefined;
  @Input() id: string | undefined;

  constructor(private todoServ: TodoService) {
  }



  onRightClick(event: MouseEvent) {
    this.todoServ.currentTodoId = this.id

    event.preventDefault();

    this.contextMenuInfo.pageX = event.pageX;
    this.contextMenuInfo.pageY = event.pageY;
  }

  contextMenuInfo: any = {
    pageX: -1,
    pageY: -1
  };


}
