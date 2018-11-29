import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserState } from './UserState';

@Injectable()
export class RequireLoginGuard implements CanActivate {
    constructor(private router: Router, protected user: UserState) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.user.isLogin) {
            return true;
        }
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url === '/' ? undefined : state.url } });
        return false;
    }
}
