import { Injectable } from "@angular/core";
import { throwError, Observable } from "rxjs";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { catchError } from "rxjs/operators";
import { Config } from "src/app/utility/config";
import { debug } from "util";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
//Login key get by local storage using parse
let loginKey = "";
@Injectable({
  providedIn: "root"
})
export class EmailService {
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
  GetAllEmailTemplate(): Observable<any> {
    const url = this.config.APIUrl + "Email";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetEmailTemplateByID(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Email/GetEmail_Templates?id=" +
      Id +
      "&LoginKey=" +
      loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  UpdateTemplate(data): Observable<any> {
    const url =
      this.config.APIUrl + "Email/PostEmailTemplates?LoginKey=" + loginKey;

    return this.http.post(url, data, httpOptions).pipe(
      //tap((data) => console.log(`added product w/ id=`,data)),
      catchError(this.handleError)
    );
  }

  UpdateEmail(data): Observable<any> {
    //console.log("This is update email", data);
    const url =
      this.config.APIUrl + "Email/EditMailingList?LoginKey=" + loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  GetEmailList(ID): Observable<any> {
    const url = this.config.APIUrl + "Email/GetMailingList";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  DownloadEmailList(): Observable<any> {
    const url = this.config.APIUrl + "Email/DownloadEmailList?LoginKey=" + loginKey;;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetEmail(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Email/MailingListById?id=" +
      Id +
      "&LoginKey=" +
      loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  deleteEmail(Id): Observable<any> {
    const url =
      this.config.APIUrl + "Email/DeleteMailing?id=" + Id + "&LoginKey=" + loginKey;
    return this.http.delete<any[]>(url);
  }

  deleteTemplate(Id): Observable<any> {
    const url = this.config.APIUrl + "Email/" + Id + "?LoginKey=" + loginKey;
    return this.http.delete<any[]>(url);
  }
}
