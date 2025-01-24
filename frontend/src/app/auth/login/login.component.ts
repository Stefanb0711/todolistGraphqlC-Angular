import { Component } from '@angular/core';
import {AuthenticationService} from '../../services/auth-service.service';
import {LoginModel} from '../../models/login.model';

import { Router } from '@angular/router';
import {LoginResponseModel} from '../../models/LoginResponse.model';
import {log} from '@angular-devkit/build-angular/src/builders/ssr-dev-server';
@Component({
  selector: 'app-login',
  standalone: false,

  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  constructor(private authServ: AuthenticationService,
              private router: Router) {
  }

  errorMessage: string = '';

  emptyLogin : LoginModel = {
    usernameOrEmail: "",
    password: ""
  }

  loginData: LoginModel = this.emptyLogin;



  onLoginSubmit() {

    console.log("loginData: ", this.loginData);

    this.authServ.loginUser(this.loginData).subscribe({
      next: (res: any) => {

        /*
        console.log("Login erfolgreich");
        console.log("ApiResponse: " + res);
        console.log("Der aktuelle Token: ", res.token);
        console.log("Die aktuelle Message: " + res.message);
        */

        localStorage.setItem('token', res.token)

        console.log(res.message);

        this.router.navigate(['/']);
      }, error: (err: any) => {
        this.errorMessage = err.message;
        console.log("Error: ", err.message);
        console.log("Login fehlgeschlagen");
      }

    });

  }



}
