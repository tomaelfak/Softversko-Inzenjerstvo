import { Component } from '@angular/core';
import {NgForm} from "@angular/forms";
import {SignInUser} from "../../interfaces/signInUser";
import {RegisterUser} from "../../interfaces/registerUser";
import {AuthService} from "../../services/auth.service";
import {Observable} from "rxjs";
import {AuthResponseData} from "../../interfaces/authResponseData";
import {Router} from "@angular/router";

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent {
  isSignInMode = true;
  isLoading = false;
  error: string | null= null;
  passwordMatches = true;
  signInUser: SignInUser = {
    email: "",
    password: ""
  };
  registerUser: RegisterUser ={
    address: "",
    email: "",
    firstName: "",
    lastName: "",
    password: "",
    username: ""
  };



  constructor(private authService: AuthService, private router: Router) { }



  onSwitchMode(){
    this.isSignInMode = !this.isSignInMode;
    this.error = null;
  }

  onSubmit(form: NgForm){
    if(form.invalid){
      return;
    }

    this.isLoading = true;

    let authObs: Observable<AuthResponseData>;

    if(this.isSignInMode){
      this.signInUser.email = form.value.login.email;
      this.signInUser.password = form.value.login.password;
      authObs = this.authService.login(this.signInUser)
    }else{
      this.registerUser.firstName = form.value.register.firstName;
      this.registerUser.lastName = form.value.register.lastName;
      this.registerUser.email = form.value.register.email;
      this.registerUser.username = form.value.register.username;
      this.registerUser.address = form.value.register.address;
      if(form.value.register.password != form.value.register.repeatPassword){
        this.passwordMatches = false;
        return;
      }
      this.passwordMatches = true;
      this.registerUser.password = form.value.register.password;
      authObs = this.authService.register(this.registerUser);
    }

    authObs.subscribe({
      next: responseData => {
        console.log(responseData);
        this.isLoading = false;
        this.router.navigate(['/events']);
      },
      error: errorMessage => {
        console.log(errorMessage);
        this.error = errorMessage
        this.isLoading = false;
      }
    });


    form.resetForm();


  }

}
