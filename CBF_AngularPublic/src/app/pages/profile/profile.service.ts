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

@Injectable({
  providedIn: "root"
})
export class ProfileService {
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

  private extractData(res: Response) {
    let body = res;
    return body || {};
  }
  register(data): Observable<any> {
    const url = this.config.APIUrl + "Account/Register";
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  upLoadBase64(memeberid, data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/MemberFileUploadBase64?LoginKey=" +
      this.config.loginKey +
      "&Member_Id=" +
      memeberid;

    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  createUser(data): Observable<any> {
    const url = this.config.APIUrl + "Member?LoginKey=" +  this.config.loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  GetStates(CountryId): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/GetStateListByCountries?LoginKey=" +
      this.config.loginKey +
      "&Country_ID=" + CountryId; //+ this.config.loginKey +
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
 
  GetCountry(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/GetCountriesList?LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetMemberById(Id): Observable<any> {
    const url = this.config.APIUrl + "Member/" + this.config.memberId + "?LoginKey=" +  this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  UpdateEmailPreference(AddMailing): Observable<any> {
    const url = this.config.APIUrl + "Email/UpdateEmailPreference?LoginKey="+  this.config.loginKey + "&AddToMailingList=" + AddMailing ;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
}
