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


  registerUserResponse : any | null = null;

  constructor(private authServ: AuthenticationService,
              private router: Router) {
  }


  async submitRegistration() {
    console.log("Submit wird ausgeführt");

    try {
      const response: any = await this.authServ.registerUser(this.registrationData);
      console.log("Response of registrationMessage: ");
      this.registerUserResponse = response.registerUser.message;
    } catch (error) {
      console.error("Fehler:", error);
    }

      //data.registerUser.message ? this.router.navigate(['/']) : this.errorMessage = data.registerUser.message;

      /*if (data.registerUser && data.registerUser.message) {


        this.router.navigate(['/'])
      }*/



    /*
      .subscribe({
      next: (res : any) => {
        console.log("Response of registration", res);
        if (res && res.success) {
            this.router.navigate(['/'])
        } else {
            console.log('Registrierungsfehler', res.message);
        }
      },
      error: (error: any) => {
        console.log("Fehler bei der Registreirung");
        this.errorMessage = error.message;
      }

    });
    */

  }

}
