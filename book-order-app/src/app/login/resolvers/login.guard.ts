import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { SsoDataService } from './../services/sso-data.service';

@Injectable()
export class LoginGuard implements CanActivate {

  constructor(private router: Router,
    private ssoDataService: SsoDataService){}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.ssoDataService.isAuthenticated().
      map(result => {
        if(result.json() === true){
          return true;
        } else {
          this.router.navigate(['/login']);
          return false;
        }
      });
  }

}
