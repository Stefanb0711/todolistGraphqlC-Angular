import {Component, Input, OnInit} from '@angular/core';
import {DatePipe} from '@angular/common';
import {data} from 'autoprefixer';
import {TodoService} from '../services/todo-service.service';
import {TodoModel} from '../models/Todo.model';
//import {error} from '@angular/compiler-cli/src/transformers/util';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-todolist-element',
  standalone: false,

  templateUrl: './todolist-element.component.html',
  styleUrl: './todolist-element.component.css'
})
export class TodolistElementComponent implements OnInit{

  @Input() id: string | undefined;
  @Input() title : string | undefined;
  @Input() date: number | undefined;

  formattedDate: any;
  constructor(private datePipe: DatePipe, private todoServ: TodoService) {
  }

  ngOnInit() {
    this.formattedDate = this.datePipe.transform(this.date, 'yyyy-MM-dd HH:mm');
  }


  openTodos() {
    console.log("OpenTodos aktiviert");
    this.todoServ.currentTodolistId = this.id;
    console.log("CurrentTodoListId: ", this.todoServ.currentTodolistId);
    this.todoServ.getAllTodos().subscribe({
     next: (res: any) => {
      console.log("Alle Todos: ", res);
      this.todoServ.currentTodos = res;
     }, error: (err: any) => {

      }

    });
  }


   contextMenuInfo: any = {
    pageX: -1,
    pageY: -1
  };

  onRightClick(event: MouseEvent) {

      this.todoServ.currentTodolistId = this.id;

      console.log("CurrentTodoListId von Contextmenu: ", this.todoServ.currentTodolistId);


      event.preventDefault();

      this.contextMenuInfo.pageX = event.pageX;
      this.contextMenuInfo.pageY = event.pageY;
  }

}
