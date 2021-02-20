import { Injectable } from "@angular/core";

import { Observable, of, throwError } from "rxjs";
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";
import { catchError, tap, map } from "rxjs/operators";

import { BehaviorSubject } from "rxjs";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};

@Injectable({
  providedIn: "root"
})
export class RootService {
  constructor(private http: HttpClient) {}

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error("An error occurred:", error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        "Backend returned code ${error.status}, " + "body was: ${error.error}"
      );
    }
    // return an observable with a user-facing error message
    return throwError("Something bad happened; please try again later.");
  }

  private extractData(res: Response) {
    let body = res;
    return body || {};
  }

  GetAllUser(data): Observable<any> {
    const url =
      "http://cbf-nowgray-com.nt1-p2stl.ezhostingserver.com/api/Users/GetAll/" +
      data;
    return this.http.get(url, httpOptions).pipe(
      //tap((data) => console.log(`added product w/ id=`,data)),
      catchError(this.handleError)
    );
  }

  GetUserById(data): Observable<any> {
    const url =
      `http://cbf-nowgray-com.nt1-p2stl.ezhostingserver.com/api/Users/` + data;
    return this.http.get(url, httpOptions).pipe(
      //tap((data) => console.log(`added product w/ id=`,data)),
      catchError(this.handleError)
    );
  }

  createUser(data): Observable<any> {
   // console.log("This is create user", data);
    const url =
      "http://cbf-nowgray-com.nt1-p2stl.ezhostingserver.com/api/Users";
    return this.http.post(url, data, httpOptions).pipe(
      //tap((data) => console.log(`added product w/ id=`,data)),
      catchError(this.handleError)
    );
  }
}
