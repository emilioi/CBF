import { Injectable } from "@angular/core";
import { Config } from "src/app/utility/config";
import {
  HttpErrorResponse,
  HttpHeaders,
  HttpClient
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
@Injectable({
  providedIn: "root"
})
export class ClubHouseService {
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

  GetClubDetails(): Observable<any> {
    const url =
      this.config.APIUrl +
      "PoolMaster/PoolListForMemberClubHouse?LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetClubDetailsAll(): Observable<any> {
    const url =
      this.config.APIUrl +
      "PoolMaster/PoolListForMemberClubHouse?LoginKey=" +
      this.config.loginKey +
      "&AllClubs=true";
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  JoinClubAddTickets(NoOfTickets, PoolId): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/JoinClubAddTickets?LoginKey=" +
      this.config.loginKey +
      "&PoolId=" +
      PoolId +
      "&NoOfTickets=" +
      NoOfTickets;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetPoolByID(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "PoolMaster/GetPool_Master?id=" +
      Id +
      "&LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetMemberAlert(): Observable<any> {
    const url = this.config.APIUrl + "Alerts?LoginKey=" + this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
}
