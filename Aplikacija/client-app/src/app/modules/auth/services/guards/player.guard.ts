import {Injectable} from "@angular/core";
import {AuthService} from "../auth.service";
import {ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";

@Injectable({providedIn: 'root'})
export class PlayerGuard{
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        if(this.authService.isPlayer()){
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
