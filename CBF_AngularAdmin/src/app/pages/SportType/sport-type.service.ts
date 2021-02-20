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
export class SportTypeService {
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

  GetSportsType(): Observable<any> {
    const url = this.config.APIUrl + "SportsType";

    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  updateNewSportType(data): Observable<any> {
    const url =
      this.config.APIUrl + "SportsType/PostSportType?LoginKey=" + loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  deleteSportType(sportType): Observable<any> {
    const url =
      this.config.APIUrl + "SportsType/DeleteSportsType?id=" + sportType + "&LoginKey=" + loginKey;//SportsType/DeleteSportsType?id=0&LoginKey=debug
    return this.http.delete<any[]>(url);
  }
}
