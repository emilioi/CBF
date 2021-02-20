import { Injectable } from "@angular/core";
import { Config } from "src/app/utility/config";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};

let loginKey = "";

@Injectable({
  providedIn: 'root'
})
export class DefaultPickService {

  constructor(private http: HttpClient, private config: Config) {
    loginKey =
      JSON.parse(localStorage.getItem("userObj")) !== null
        ? JSON.parse(localStorage.getItem("userObj")).key
        : "";

    console.log("hey I got this loginkey", loginKey);
  }
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

  GetWeeklyDefaultsSchedule(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/GetWeeklyDefaultsScheduleForPublic?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      data.pool_ID;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
}
