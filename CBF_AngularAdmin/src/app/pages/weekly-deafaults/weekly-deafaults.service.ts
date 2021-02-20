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
  providedIn: "root"
})
export class WeeklyDeafaultsService {
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

  getScheduleMenu(): Observable<any> {
    const url =
      this.config.APIUrl + "NFLSchedule/GetScheduleMenu?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetNFLScheduleWeeksList(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "NFLSchedule/GetNFLScheduleWeeksList?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetWeeklyDefaultsSchedule(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/GetWeeklyDefaultsSchedule?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      data.pool_ID +
      "&Week=" +
      data.week;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  SetDefaultPicks(list): Observable<any> {
    let data = "{ \"weeklyDefaults\": "
    +
    JSON.stringify(list)
    + "}";
    console.log(data);
    const url =
      this.config.APIUrl +
      "Picks/SetDefaultPicks?LoginKey=" +
      loginKey ;
    return this.http.post(url,data, httpOptions).pipe(catchError(this.handleError));
  }

  GetWeeksListByPoolForPager(PoolId): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetWeekNumbers?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      PoolId;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetWithoutPickPool(PoolId, week): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/EntriesListWithoutPickByPoolandWeek?LoginKey=" +
      loginKey +
      "&PoolId=" +
      PoolId +
      "&WeekNumber=" +
      week;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  getWeekNumber(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetWeekNumbers?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      data.pool_ID;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

 
  getTeamList(): Observable<any> {
    const url = this.config.APIUrl + "Teams?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
}
