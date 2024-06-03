import {Injectable} from "@angular/core";
import {AuthService} from "../auth.service";
import {ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";

@Injectable({providedIn: 'root'})
export class PlayerAdminGuard{
  constructor(private authService: AuthService, private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(this.authService.isPlayer() || this.authService.isAdmin()){
      return true;
    }
    else{
      return this.router.createUrlTree(['/events/halls'])
    }

  }
}
