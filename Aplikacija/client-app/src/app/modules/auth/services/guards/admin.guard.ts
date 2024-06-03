import {Injectable} from "@angular/core";
import {AuthService} from "../auth.service";
import {ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable, take} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({providedIn: 'root'})
export class AdminGuard{
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(this.authService.isAdmin()){
      return true;
    }
    else if(this.authService.isManager()){
      return this.router.createUrlTree(['/events/halls'])
    }
    else {
      return this.router.createUrlTree(['/events'])
    }
  }
}
