import {Component, HostListener, OnInit} from '@angular/core';
import {AuthenticationService} from '../services/auth-service.service';
import {TodoService} from '../services/todo-service.service';
import {TodolistModel} from '../models/Todolist.model';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { ChangeDetectorRef } from '@angular/core';
import {TodoModel} from '../models/Todo.model';
import {ContextmenuComponent} from '../context-menues/contextmenu/contextmenu.component';
import {Apollo, gql} from 'apollo-angular';


@Component({
  selector: 'app-home',
  standalone: false,

  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{


  todoLists: any[] = [];

  errorMessage: string = "";

  contextMenuInfo: any = {
    pageX: -1,
    pageY: -1
  };

  helloQueryAnswer: any = "";

  constructor(public authServ: AuthenticationService,
              public todoServ: TodoService, private cdr: ChangeDetectorRef, private apollo: Apollo) {}



  ngOnInit() {


    /*
    fetch('https://localhost:7188/graphql', { // Ersetzen Sie dies durch Ihre GraphQL-Endpoint-URL
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        // Fügen Sie hier bei Bedarf Autorisierungsheader hinzu (z. B. 'Authorization': 'Bearer <token>')
      },
      body: JSON.stringify({ query })
    })
      .then(response => response.json())
      .then(data => {
        console.log('Daten vom Server:', data); // Hier sind Ihre Daten
        // Verarbeiten Sie die Daten hier
      })
      .catch(error => {
        console.error('Fehler:', error);
      });

    */


    this.authServ.getHello().then(data => {
      this.helloQueryAnswer = data.hello; // Zugriff auf das Ergebnis der Anfrage

    })
      .catch(error => {
        console.error('Fehler:', error);
        // Fehlerbehandlung hier implementieren
      });




    /*
    if(!this.authServ.isTokenValid()) {
      this.errorMessage = "Sie müssen Sich einloggen, um ihre Todos zu sehen";
      this.authServ.removeToken();
    }

    this.authServ.currentToken = localStorage.getItem('token');

    console.log("CurrentToken beim aufrufen von Home: ", this.authServ.currentToken);

    console.log("CurrentUserId beim aufrufen von Home: ",this.authServ.currentUserId);

    this.authServ.getCurrentUserId().subscribe({
      next: (res: any) => {
        this.authServ.currentUserId = res["currentUserId"];
      },
      error: (err: any) => {
        console.log("Fehler bei nutzerid bekommen");
      }
    });


    this.todoServ.getAllTodoLists().subscribe({
      next: (res: any) => {
        //console.log("Die Todos der aktuellen Userid: ", res["todoList"]["result"]);
        this.todoServ.currentTodolists = res;

        console.log(res);

      }, error: (err: any) => {
        if ( err.response && err.response.status === 401) {
          this.authServ.homePageErrorMessage = "Um auf die Inhalte zugreifen zu können müssen Sie authentifiziert sein";

        }
        console.log("Fehler beim bekomen der Todos");
      }
    });

    */

  }



  dropTodolist(event: CdkDragDrop<TodolistModel[]>) {
    moveItemInArray(this.todoServ.currentTodolists, event.previousIndex, event.currentIndex);
      this.cdr.detectChanges();

  }

  dropTodo(event: CdkDragDrop<TodoModel[]>) {
    moveItemInArray(this.todoServ.currentTodos, event.previousIndex, event.currentIndex);
      this.cdr.detectChanges();
  }


  onRightClick(event: MouseEvent) {
    event.preventDefault();

    this.contextMenuInfo.pageX = event.pageX;
    this.contextMenuInfo.pageY = event.pageY;
  }


}
