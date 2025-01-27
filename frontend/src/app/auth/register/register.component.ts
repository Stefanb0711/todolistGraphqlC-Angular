import { Component } from '@angular/core';
import {RegisterModel} from '../../models/register.model';
import {AuthenticationService} from '../../services/auth-service.service';
import { Router } from '@angular/router';
import {RegisterResponseModel} from '../../models/RegisterResponse.model';
import {Apollo} from 'apollo-angular';

@Component({
  selector: 'app-register',
  standalone: false,

  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  registrationData: RegisterModel = {
    email: '',
    username: '',
    password: '',
    passwordConfirm: ''
  };

  errorMessage: string = "";


  constructor(private authServ: AuthenticationService, private router: Router) {
  }


  submitRegistration() {
    console.log("Submit wird ausgeführt");
    /*
    this.authServ.registerUser(this.registrationData).subscribe({
      next: (res : any) => {
        if (res.success){
          console.log(res);

          this.router.navigate(['/'])
        } else {
          console.log('Registrierungsfehler', res.message);
        }

      },
      error: (error: RegisterResponseModel) => {
        this.errorMessage = error.message;
      }

    });
      */

  }

}
