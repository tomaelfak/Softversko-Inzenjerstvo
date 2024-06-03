import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UserInfo} from "../../interfaces/userInfo";
import {Subject} from "rxjs";
import {AuthService} from "../auth.service";

@Injectable({
  providedIn: 'root'
})
export class AdminPanelService {
  users: UserInfo[] = [];
  usersChanged = new Subject<UserInfo[]>();
  baseUrl = 'http://localhost:5000/api/Admin';


  constructor(private http: HttpClient, private authService: AuthService) { }

  getUsers() {
    if(this.users.length === 0) {
      this.fetchUsers();
    }else {
      this.usersChanged.next(this.users.slice());
    }
  }

  fetchUsers() {
    this.http.get<UserInfo[]>(this.baseUrl + '/users-with-roles').subscribe(users => {
      this.users = users.filter(user => {return user.username !== this.authService.user?.value?.username});
      this.usersChanged.next(this.users.slice());
    });
  }

  upgradeToAdmin(username: string) {
    this.http.post(this.baseUrl + `/edit-roles/${username}?roles=Admin`, {}).subscribe(() => {
      this.users.forEach(user => {
        if(user.username === username) {
          user.roles = ['Admin'];
        }
      })

    });
  }

  upgradeToManager(username: string) {
    this.http.post(this.baseUrl + `/edit-roles/${username}?roles=Manager`, {}).subscribe(() => {
      this.users.forEach(user => {
        if(user.username === username) {
          user.roles = ['Manager'];
        }
      })
    });
  }

  upgradeToPlayer(username: string) {
    this.http.post(this.baseUrl + `/edit-roles/${username}?roles=Player`, {}).subscribe(() => {
      this.users.forEach(user => {
        if(user.username === username) {
          user.roles = ['Player'];
        }
      })
    });
  }

  deleteUser(username: string) {
    this.http.delete(this.baseUrl + `/delete-user/${username}`, {responseType: 'text'}).subscribe({
      next: (data) => {
        this.users = this.users.filter(user => user.username !== username);
        this.usersChanged.next(this.users.slice());
      },
      error: (error) => {
        console.log(error);
      }
    });
  }





}
