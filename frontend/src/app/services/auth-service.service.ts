import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {LoginModel} from '../models/login.model';
import {RegisterModel} from '../models/register.model';
import {LoginResponseModel} from '../models/LoginResponse.model';
import {RegisterResponseModel} from '../models/RegisterResponse.model';
import {jwtDecode} from 'jwt-decode';
import {Apollo, gql} from 'apollo-angular';
import {map} from 'rxjs';
import { GraphQLClient } from 'graphql-request';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {

  private client: GraphQLClient;


  private apiUrl = 'https://localhost:7188/graphql';

  constructor(private router: Router, private http: HttpClient, private apollo: Apollo) {
    this.client = new GraphQLClient(this.apiUrl);
  }

  async getHello(): Promise<any> {
    const query = `
    query {
      hello
    }`;
    try {
      const data = await this.client.request(query);
      return data;
    } catch (error) {
      console.error('Fehler bei der GraphQL-Anfrage:', error);
      throw error; // Oder behandeln Sie den Fehler anders
    }

  }

  homePageErrorMessage: string = "";
  currentToken: string | null = "";


  async registerUser(registerData: RegisterModel) {

    const mutation = `
    mutation RegisterUser($username: String!, $email: String!,
     $password: String!, $passwordConfirm: String!) {
      registerUser(registrationData: {
        username: $username,
        email: $email,
        password: $password,
        passwordConfirm: $passwordConfirm
      }) {
        success
        message
      }
    }
    `;

    const variables = {
      username: registerData.username,
      email: registerData.email,
      password: registerData.password,
      passwordConfirm: registerData.passwordConfirm,
    };

    try {
      const data = await this.client.request(mutation, variables);
      return data;
    } catch (error) {
      console.error('Fehler bei der Mutation:', error);
      throw error;

    }
  };



  async loginUser(loginData: LoginModel) {
    const mutation = `
    mutation LoginUser($usernameOrEmail: String!, $password: String!) {
      loginUser(loginData: {
        usernameOrEmail: $usernameOrEmail,
        password: $password
      }) {
      success
      message
      token
      }
    }
    `;

    const variables = {
        usernameOrEmail: loginData.usernameOrEmail,
        password: loginData.password,
      };

    try {
        const data: any = await this.client.request(mutation, variables);

        if (data.loginUser.success) {
          const token = data.loginUser.token;
          this.router.navigate(['/']);
          localStorage.setItem('token', token);
        }

        return data;
    } catch (error) {
      console.error('Fehler bei der Query:', error);
      throw error;

    }
  };



  /*
  loginUser(loginData: LoginModel) {
    return this.apollo.mutate({
      mutation: this.LOGIN_USER,
      variables: {
        loginData: {
          usernameOrEmail: loginData.usernameOrEmail,
          password: loginData.password
        }
      }
    })
  }
  */

  tokenValid: boolean = false;
  currentUserId: string = "";

  emptyLogin: LoginModel = {
    usernameOrEmail : '',
    password: ''
  }

  loginData: LoginModel = this.emptyLogin;

  getUsers() {
    return this.http.get<any>(`${this.apiUrl}/get-users`);
  }



  getToken(): string | null {
    return localStorage.getItem('token');

  }

  removeToken(): void {
    return localStorage.removeItem('token');

  }

  /*
  getCurrentUserId() {

    const body = {
      token: this.currentToken
    };

    return this.http.post<string>(`${this.apiUrl}/get-userid`, body);
  }
  */

  async getCurrentUserId() {
    const mutation = `
      mutation GetUserId($token: String!) {
        getUserId(tokenData: {
          token: $token
      }) {
      success
      message
      userId
      }
    }
    `;

    const variables = {
      token: this.currentToken
    }

    try {
      const data: any = await this.client.request(mutation, variables);
      this.currentUserId = data.getUserId.userId;
      //console.log("currentUserId: ", this.currentUserId);
      //return data;
    } catch (error) {
      console.error('Fehler bei der Mutation GetCurrentUserId:', error);
      throw error;
    }

  }

   // Überprüfen, ob der Token gültig ist
  isTokenValid(): boolean {
    const token = this.getToken();
    if (!token) {
      return false; // Kein Token vorhanden
    }

    try {
      const decodedToken: any = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000); // Aktuelle Zeit in Sekunden
      return decodedToken.exp > currentTime; // Ist der Token gültig?
    } catch (error) {
      console.error('Ungültiger Token:', error);
      return false;
    }
  }




}

