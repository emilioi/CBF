import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Config } from './config';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
    isValid: boolean;
    constructor(private router: Router, private config: Config, private api: AuthService) {

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        this.config.resolveLoginKeyPromise();
        if (this.config.loginKey !== "" && typeof this.config.loginKey !== undefined && this.config.loginKey !== null) {
            console.log('auth ' + this.config.loginKey);
            this.ValidateLoginKeyPromise();
            return true;
        }

        localStorage.setItem("LoginMessage", "Session expired, Please login again.");
        this.router.navigate(['/'], { queryParams: { returnUrl: state.url } });
        // not logged in so redirect to login page with the return url
        return false;

    }

    ValidateLoginKeyPromise() {
        // return new Promise(resolve => {
        this.api.ValidateToken(this.config.loginKey).subscribe(
                res => {
               
                if (res.status == "1") {

                } else {
                    localStorage.setItem("LoginMessage", res.message);
                    this.router.navigate(['/']);
                }
            },
            err => {
                
                this.router.navigate(['/']);
                 throw new Error(err);
            }
        );
        return "0";

    }
    updateAuth(res) {
        this.isValid = res;
    }


}