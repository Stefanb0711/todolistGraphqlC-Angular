import { Component } from '@angular/core';
//import {AuthenticationService} from '../../services/auth-service.service';
import {LoginModel} from '../../models/login.model';

import { Router } from '@angular/router';
import {LoginResponseModel} from '../../models/LoginResponse.model';
import {log} from '@angular-devkit/build-angular/src/builders/ssr-dev-server';
import {AuthenticationService} from '../../services/auth-service.service';
@Component({
  selector: 'app-login',
  standalone: false,

  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private authServ: AuthenticationService, private router: Router) {
  }

  errorMessage: string = '';

  emptyLogin : LoginModel = {
    usernameOrEmail: "",
    password: ""
  }

  loginData: LoginModel = this.emptyLogin;



  async onLoginSubmit() {

    console.log("loginData: ", this.loginData);

    try {
      const response: any = await this.authServ.loginUser(this.loginData);
      console.log("Response of loginMessage: ", response.loginUser.message);
      this.errorMessage = response.loginUser.message;

    } catch (error) {
      console.error("Fehler:", error);
    }

  }



}
