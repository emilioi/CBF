import { Injectable } from "@angular/core";
import { throwError, Observable } from "rxjs";
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders
} from "@angular/common/http";
import { catchError } from "rxjs/operators";
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
export class PoolService {
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

  GetAllPools(): Observable<any> {
    const url = this.config.APIUrl + "PoolMaster?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetPoolByID(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "PoolMaster/GetPool_Master?id=" +
      Id +
      "&LoginKey=" +
      loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  UpdatePool(data, build_schedule, thursdayGames, saturdayGames,sundayGames:boolean=false): Observable<any> {
    //console.log("This is update Pool", data);
    const url =
      this.config.APIUrl +
      "PoolMaster/PostPool_Master?LoginKey=" +
      loginKey +
      "&build_schedule=" +
      build_schedule +
      "&thursdayGames=" + thursdayGames + "&saturdayGames=" + saturdayGames + "&sundayGames=" + sundayGames;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  deletePool(Id): Observable<any> {
    const url =
      this.config.APIUrl + "PoolMaster/" + Id + "?LoginKey=" + loginKey;
    return this.http.delete<any[]>(url);
  }

  getClosePool(Id): Observable<any> {
    const url = this.config.APIUrl + "Common/GetClosePoolType"; // + Id + "?LoginKey=" + this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetClosePoolType(): Observable<any> {
    const url = this.config.APIUrl + "Common/GetClosePoolType";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetSportsType(): Observable<any> {
    const url = this.config.APIUrl + "SportsType";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  upLoadBase64(Pool_Id, data): Observable<any> {
    const url =
      this.config.APIUrl +
      "PoolMaster/PoolFileUploadBase64?LoginKey=" +
      loginKey +
      "&Pool_Id=" +
      Pool_Id;

    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
}
