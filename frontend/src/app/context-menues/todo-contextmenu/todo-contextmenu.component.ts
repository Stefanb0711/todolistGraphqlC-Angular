import {Component, HostListener, Input} from '@angular/core';
import {TodoService} from '../../services/todo-service.service';

@Component({
  selector: 'app-todo-contextmenu',
  standalone: false,

  templateUrl: './todo-contextmenu.component.html',
  styleUrl: './todo-contextmenu.component.css'
})
export class TodoContextmenuComponent {

  @Input() x: number = 0;
  @Input() y: number = 0;

  constructor(private todoServ: TodoService) {
  }


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

  onDeleteClick() {
    this.todoServ.deleteTodo().subscribe({
      next : (res: any) => {
        this.todoServ.currentTodos = res;

      }, error: (err: any) => {

      }
    })
  }

}
