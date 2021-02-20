import { Injectable } from "@angular/core";
import { throwError, Observable } from "rxjs";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { catchError } from "rxjs/operators";
import { Config } from "src/app/utility/config";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};

let loginKey = "";

@Injectable({
  providedIn: "root"
})
export class WeekService {
  constructor(private http: HttpClient, private config: Config) {
    loginKey =
      JSON.parse(localStorage.getItem("userObj")) !== null
        ? JSON.parse(localStorage.getItem("userObj")).key
        : "";
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

  getWeeksMenu(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetWeeksMenu?LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetPoolWeeksList(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetPoolWeeksList?LoginKey=" +
      this.config.loginKey +
      "&Pool_ID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  getWeekNumber(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetWeekNumbers?LoginKey=" +
      this.config.loginKey +
      "&Pool_ID=" +
      data.pool_ID;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  savePoolWeek(data): Observable<any> {
    const url =
      this.config.APIUrl +
      `Weeks/CreatePoolWeek?LoginKey=` +
      this.config.loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  deletPoolWeek(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/DeletePoolWeek?LoginKey=" +
      this.config.loginKey +
      "&Pool_ID=" +
      data.poolID +
      "&WeekNumber=" +
      data.weekNumber;
    return this.http
      .delete(url, httpOptions)
      .pipe(catchError(this.handleError));
  }
}
