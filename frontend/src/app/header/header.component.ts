import { Component } from '@angular/core';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {Router} from '@angular/router';
import {AuthenticationService} from '../services/auth-service.service';

@Component({
  selector: 'app-header',
  standalone: false,

  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  constructor(private router: Router, public authServ: AuthenticationService) {
  }

  onLoginClick(){
    this.router.navigate(['login']);
  }

  onRegisterClick() {
    this.router.navigate(['register']);
  }

  onLogoutClick() {
    this.authServ.currentToken = '';
    this.authServ.homePageErrorMessage = 'Sie müssen sich einloggen, um auf die Todos zugreifen zu können';
    this.authServ.removeToken();
  }

}
