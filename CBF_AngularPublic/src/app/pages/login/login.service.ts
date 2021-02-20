import { Injectable } from "@angular/core";
import { Observable, of, throwError } from "rxjs";
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";
import { catchError, tap, map } from "rxjs/operators";
import { BehaviorSubject } from "rxjs";
import { Config } from 'src/app/utility/config';

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};

@Injectable({
  providedIn: "root"
})
export class LoginService {
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

  private extractData(res: Response) {
    let body = res;
    return body || {};
  }

  login(data): Observable<any> {
    const url =
      this.config.APIUrl + "Account/login";
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  RulesAccpted(Id, RulesAccepted): Observable<any> {
    const url = this.config.APIUrl + "Member/AcceptedRules?LoginKey=" + this.config.loginKey + "&Member_Id=" + Id + "&RulesAccepted=" + RulesAccepted;
    console.log("URL: " + url);
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
  getEntryWithoutPick(): Observable<any> {
    const url = this.config.APIUrl + "Entries/EntryWithoutPicks?LoginKey=" + this.config.loginKey;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetRegistrationSetting(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/ClubSettings";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetMaintenanceSetting(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/MaintenanceSettings";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  //////*******************Rule*********/////
  getRulesById(Id): Observable<any> {
    const url = this.config.APIUrl + "Rules/GetRulesByIdPublic?id=" + Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }
  getRules(): Observable<any> {
    const url = this.config.APIUrl + "Rules";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError))
  }
}
