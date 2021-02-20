import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Config } from 'src/app/utility/config';
const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private config: Config) { }
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error("An error occurred:", error.error.message);
    } else {
      console.error(
        "Backend returned code ${error.status}, " + "body was: ${error.error}"
      );
    }
    return throwError("Something bad happened; please try again later.");
  }

  ValidateToken(loginkey): Observable<any> {
    const url = this.config.APIUrl + "Account/ValidateToken?LoginKey=" + loginkey;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
}
