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
const httpOptionsFile = {
  headers: new HttpHeaders({
    "Content-Type": "multipart/form-data"
  })
};
let loginKey = "";

@Injectable({
  providedIn: "root"
})
export class TeamService {
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

  getTeams(): Observable<any> {
    const url = this.config.APIUrl + "Teams?loginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  getTeamList(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Teams/GetTeamList=" +
      data.GetTeamList +
      "loginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  CreateUpdateTeams(data): Observable<any> {
    const url =
      this.config.APIUrl + "Teams/CreateUpdateTeams?loginKey=" + loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  LogoUpload(teamID, data: File): Observable<any> {
    const url =
      this.config.APIUrl +
      "Teams/LogoUpload?teamID=" +
      teamID +
      "&loginKey=" +
      loginKey;

    const formData: FormData = new FormData();
    formData.append("file", data);
    //  formData.append("teamID", data.teamID);

    return this.http
      .post(url, formData, httpOptions)
      .pipe(catchError(this.handleError));
  }

  upLoadBase64(teamID, data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Teams/LogoUploadBase64?LoginKey=" +
      loginKey +
      "&TeamID=" +
      teamID;

    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  GetSportsType(): Observable<any> {
    const url = this.config.APIUrl + "SportsType";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  deleteTeamList(id): Observable<any> {
    const url =
      this.config.APIUrl + "Teams/?id=" + id + "&loginKey=" + loginKey;
    return this.http
      .delete(url, httpOptions)
      .pipe(catchError(this.handleError));
  }
}
