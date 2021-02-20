import { Injectable } from "@angular/core";
import {
  HttpErrorResponse,
  HttpHeaders,
  HttpClient
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { Config } from "src/app/utility/config";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
@Injectable({
  providedIn: "root"
})
export class PickCenterService {
  //Entries/GetEntryById?EntryID=3&LoginKey=debug
  constructor(private http: HttpClient, private config: Config) {}
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
  GetEntryById(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/GetEntryById?LoginKey=" +
      this.config.loginKey +
      "&EntryID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  // GetEntryPickById(Id): Observable<any> {

  //   const url = this.config.APIUrl + "Entries/GetEntryPickById?LoginKey=" + this.config.loginKey +"&EntryID="+  Id;
  //   return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  // }
  GetEntryPickListById(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/GetEntryPickListById?LoginKey=" +
      this.config.loginKey +
      "&EntryID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetPickReportWithLogo(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/PickReportWithLogo?LoginKey=" +
      this.config.loginKey +
      "&EntryID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  getSchedules(Id, WeekNumber): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/GetPickCenterSchedule?LoginKey=" +
      this.config.loginKey +
      "&Pool_ID=" +
      Id +
      "&Week=" +
      WeekNumber;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  getWeeks(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetPoolWeeksList?LoginKey=" +
      this.config.loginKey +
      "&Pool_ID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  makePick(data: any): Observable<any> {
    const url =
      this.config.APIUrl + "Picks/MakePick?LoginKey=" + this.config.loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  MakePickTwiceValidate(data: any): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/MakePickTwiceValidate?LoginKey=" +
      this.config.loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
}
