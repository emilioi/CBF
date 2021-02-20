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
export class TicketService {
 
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
  GetTicketsByMemberId(): Observable<any> {
    console.log(this.config.loginKey );
    const url = this.config.APIUrl + "Entries/GetTicketsByMemberId?LoginKey=" + this.config.loginKey ;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetTicketsByMemberIdAndPoolId(PoolId): Observable<any> {
    console.log(this.config.loginKey );
    const url = this.config.APIUrl + "Entries/GetTicketsByMemberIdPoolId?LoginKey=" + this.config.loginKey + "&PoolId=" + PoolId ;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
}
