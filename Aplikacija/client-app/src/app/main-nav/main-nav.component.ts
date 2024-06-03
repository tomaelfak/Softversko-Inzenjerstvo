import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import {Observable, Subscription} from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import {AuthService} from "../modules/auth/services/auth.service";

@Component({
  selector: 'app-main-nav',
  templateUrl: './main-nav.component.html',
  styleUrl: './main-nav.component.css'
})
export class MainNavComponent implements OnInit, OnDestroy{
  private breakpointObserver = inject(BreakpointObserver);
  private userSub?: Subscription;
  isAuthenticated = false;
  role?: string;
  username?: string;

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    this.userSub = this.authService.user.subscribe(user => {
      this.isAuthenticated = !!user;
      this.role = user?.role;
      this.username = user?.username;
    })
  }

  ngOnDestroy() {
    this.userSub?.unsubscribe();
  }

  onLogout(){
    this.authService.logout();
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
}
