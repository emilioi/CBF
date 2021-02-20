import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpClient, HttpHeaders } from '@angular/common/http';
import { Config } from 'src/app/utility/config';
import { throwError, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
let loginKey = "";

@Injectable({
  providedIn: 'root'
})
export class ReportService {
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
  GetPickReport(PoolId, week): Observable<any> {
    const url =
      this.config.APIUrl + "Picks/PicksReport?LoginKey=" + this.config.loginKey + "&PoolId=" + PoolId + "&WeekNumber=" + week;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));

  }
  GetDefaultedReport(): Observable<any> {
    const url =
      this.config.APIUrl + "Entries/DefaultedReport?LoginKey=" + this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  getCurrentWeek(): Observable<any> {
    const url =
      this.config.APIUrl + "/Picks/GetCurrentWeek";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
}
