import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import {AppRoutingModule} from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { InputFieldComponent } from './Inputfields/input-field/input-field.component';
import { TodoElementComponent } from './todo-element/todo-element.component';
import { NgModule } from '@angular/core';
import { RegisterComponent } from './auth/register/register.component';
import { TodolistElementComponent } from './todolist-element/todolist-element.component';
import { AuthInterceptor} from './architecture/AuthInterceptor';
import {DatePipe} from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { TodoInputFieldComponent } from './Inputfields/todo-input-field/todo-input-field.component';
import {CdkDrag, CdkDropList} from "@angular/cdk/drag-drop";
import { ContextmenuComponent } from './context-menues/contextmenu/contextmenu.component';
import { TodolistContextmenuComponent } from './context-menues/todolist-contextmenu/todolist-contextmenu.component';
import { TodoContextmenuComponent } from './context-menues/todo-contextmenu/todo-contextmenu.component';
//import {ApolloConfigModule} from './apollo-config.module';
import {HttpLink} from 'apollo-angular/http';
import {InMemoryCache} from '@apollo/client/core';
import {ApolloModule, APOLLO_OPTIONS} from 'apollo-angular';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    InputFieldComponent,
    TodoElementComponent,
    RegisterComponent,
    TodolistElementComponent,
    HeaderComponent,
    TodoInputFieldComponent,
    ContextmenuComponent,
    TodolistContextmenuComponent,
    TodoContextmenuComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CdkDropList,
    CdkDrag,
    ApolloModule

  ],
  providers: [
    DatePipe,
    /*
    {
      provide: APOLLO_OPTIONS,
      useFactory: (httpLink: HttpLink) => {
        return {
          cache: new InMemoryCache(),
          link: httpLink.create({
            uri: 'https://localhost:7188/graphql'
          }),
        };
      },
    },
    */
    {
      provide: HTTP_INTERCEPTORS,


      useClass: AuthInterceptor,
      multi: true,
    },
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
