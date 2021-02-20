import { Injectable } from "@angular/core";

import { Observable, of, throwError } from "rxjs";
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";
import { catchError, tap, map } from "rxjs/operators";

import { BehaviorSubject } from "rxjs";
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
export class DevelopmentService {
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

  private extractData(res: Response) {
    let body = res;
    return body || {};
  }

  GetErrorExceptionsList(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/ErrorExceptionsList";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  ChangeRegistrationSetting(status): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/ChangeRegistrationSetting?LoginKey=" + this.config.loginKey + "&RegistrationOpen=" + status;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetRegistrationSetting(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/ClubSettings";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  ChangeMaintenanceSetting(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/ChangeMaintenanceSettingAndText?LoginKey=" + this.config.loginKey;
    return this.http.post(url, data, httpOptions).pipe(catchError(this.handleError));
  }

  GetMaintenanceSetting(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/MaintenanceSettings";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  CronElimnation(): Observable<any> {
    const url = this.config.APIUrl + "Common/CronElimnation";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }
  CronWinners(): Observable<any> {
    const url = this.config.APIUrl + "Common/CronWinners";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }
  CronPickDefaulted(): Observable<any> {
    const url = this.config.APIUrl + "Common/CronPickDefaulted";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }
  Cron(): Observable<any> {
    const url = this.config.APIUrl + "Common/Cron";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }
  updateScore(): Observable<any> {
    const url = this.config.APIUrl + "Common/UpdateScore?access=0";
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError))
  }

  ///************For NHL ************///
  updateScoreNHL(): Observable<any> {
    const url = this.config.APIUrl + "Common/UpdateNHLScore?access=0";
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError))
  }

  CronPickDefaultedNHL(): Observable<any> {
    const url = this.config.APIUrl + "Common/CronPickDefaulted";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }

  CronElimnationNHL(): Observable<any> {
    const url = this.config.APIUrl + "Common/CronElimnation";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }

  // **********RULES*************///

  getRules(): Observable<any> {
    const url = this.config.APIUrl + "Rules";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }

  getRulesById(Id): Observable<any> {
    const url = this.config.APIUrl + "Rules/GetRulesById?id=" + Id + "&LoginKey=" + this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }

  updateRules(data): Observable<any> {
    const url = this.config.APIUrl + "Rules/PostRules?LoginKey=" + this.config.loginKey;
    return this.http.post(url, data, httpOptions).pipe(catchError(this.handleError))
  }

  DeleteRule(Id): Observable<any> {
    const url = this.config.APIUrl + "Rules/DeleteRule?id=" + Id + "&LoginKey=" + this.config.loginKey;
    return this.http.delete(url, httpOptions).pipe(catchError(this.handleError))
  }
  
}
