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
export class AdministratorService {
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

  createUser(data): Observable<any> {
    const url = this.config.APIUrl + "Admin?LoginKey=" + loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  checkloginId(data): Observable<any> {
    let userName = JSON.parse(localStorage.getItem("userObj")).userInfo
      .first_Name;
    const url =
      this.config.APIUrl +
      "Admin/LoginIDExist?AdminName=" +
      data +
      "&LoginKey=" +
      loginKey;

    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  checkEmailId(data): Observable<any> {
    let userName = JSON.parse(localStorage.getItem("userObj")).userInfo
      .first_Name;

    const url =
      this.config.APIUrl +
      "Admin/EmailExist/" +
      data +
      "?AdminName=" +
      userName +
      "&LoginKey=" +
      loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  upLoadBase64(adminid, data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Admin/AdministratorsFileUploadBase64?LoginKey=" +
      loginKey +
      "&Administrators_Id=" +
      adminid;

    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  GetAllUser(data): Observable<any> {
    const url =
      this.config.APIUrl + "Admin/GetAll/" + data + "?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetAdminById(Id): Observable<any> {
    const url = this.config.APIUrl + "Admin/" + Id + "?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  DasboardInfo(): Observable<any> {
    const url =
      this.config.APIUrl + "Dashboard/GetDashboard?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  deleteAdmin(Id): Observable<any> {
    const url = this.config.APIUrl + "Admin/" + Id + "?LoginKey=" + loginKey;
    return this.http.delete<any[]>(url);
  }
}
