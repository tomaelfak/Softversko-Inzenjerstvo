import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {BehaviorSubject, catchError, take, tap, throwError} from "rxjs";
import {User} from "../models/user.model";
import {SignInUser} from "../interfaces/signInUser";
import {RegisterUser} from "../interfaces/registerUser";
import {AuthResponseData} from "../interfaces/authResponseData";
import {jwtDecode, JwtPayload} from "jwt-decode";
import {DecodedToken} from "../interfaces/decodedToken";
import {LocalStorageData} from "../interfaces/localStorageData";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  user = new BehaviorSubject<User | null>(null);
  private tokenExpirationTimer: any;
  private baseUrl = "http://localhost:5000/api/Account/"

  constructor(private http: HttpClient, private router: Router) { }

  login(userData: SignInUser){
    return this.http.post<AuthResponseData>(this.baseUrl + "login",
      {
        email: userData.email,
        password: userData.password
      }
      ).pipe(
        catchError(this.handleError),
        tap({next: resData => {
          this.handleAuthentication(resData.result.username, resData.result.image, resData.result.displayName, resData.result.address, resData.result.token);
        }})
    )
  }

  register(userData: RegisterUser){
    return this.http.post<AuthResponseData>(this.baseUrl + "register",
      {
        email: userData.email,
        password: userData.password,
        displayName: userData.firstName + " " + userData.lastName,
        username: userData.username,
        address: userData.address
      }).pipe(
        catchError(this.handleError),
        tap({next: resData => {
          this.handleAuthentication(resData.result.username, resData.result.image, resData.result.displayName, resData.result.address, resData.result.token);
        }})
    )
  }

  private handleAuthentication(username: string, image: string, displayName: string, address: string, token: string){
    const decodedToken: DecodedToken = jwtDecode(token);

    const expirationDate = new Date(decodedToken.exp * 1000);

    const user: User = new User(username, decodedToken.nameid, decodedToken.role, image, displayName, address, token, expirationDate);
    this.user.next(user);
    const expirationTime = (decodedToken.exp * 1000 -(new Date().getTime()));
    this.autoLogout(expirationTime);//in ms
    localStorage.setItem('userData', JSON.stringify(user));
  }


  private handleError(errorResponse: HttpErrorResponse){
    let errorMessage = 'An error occurred';
    if(!errorResponse.error ) {
      return throwError(() => errorMessage);
    }
    if(errorResponse.statusText === 'Unauthorized'){
      errorMessage = 'Email or password is incorrect';
    }
    else{
      switch (errorResponse.error){
        case 'Email is already taken':
          errorMessage = 'This email already exists';
          break;
        case 'Username is already taken':
          errorMessage = 'This username already exists';
          break;
        default:
          errorMessage = 'An error occurred';
          break;
      }
    }

    return throwError(() => errorMessage);
  }

  logout() {
    this.user.next(null);
    this.router.navigate(['/auth']);
    localStorage.removeItem('userData');
    if(this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }
    this.tokenExpirationTimer = null;
  }

  autoLogout(expirationDuration: number){
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration)
  }

  autoLogin(){
    const userData = localStorage.getItem('userData');
    if(!userData){
      return;
    }
    const userDataObject: LocalStorageData = JSON.parse(userData);
    const loadedUser = new User(
      userDataObject.username,
      userDataObject.id,
      userDataObject.role,
      userDataObject.image,
      userDataObject.displayName,
      userDataObject.address,
      userDataObject._token,
      new Date(userDataObject._tokenExpirationDate)
    )
    if(loadedUser.token){
      this.user.next(loadedUser);
      const expirationTime = new Date(userDataObject._tokenExpirationDate).getTime() - new Date().getTime();
      this.autoLogout(expirationTime);
    }
  }

  isAdmin(){
    if(!this.user){
      return false;
    }
    return this.user.value?.role === 'Admin';

  }
  isPlayer(){
    if(!this.user){
      return false;
    }
    return this.user.value?.role === 'Player';
  }
  isManager(){
    if(!this.user){
      return false;
    }
    return this.user.value?.role === 'Manager';
  }





}
