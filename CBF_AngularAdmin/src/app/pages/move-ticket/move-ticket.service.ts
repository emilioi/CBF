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
export class MoveTicketService {
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

  getPoolDD(): Observable<any> {
    const url = this.config.APIUrl + "PoolMaster?LoginKey=" + this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetTicketandOwnerByPoolId(poolId): Observable<any> {
    const url =
      this.config.APIUrl + "Entries/GetEntriesWeeksList?LoginKey=" + this.config.loginKey + "&Pool_ID=" + poolId;
    console.log(url);
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetMember(poolID): Observable<any> {
    const url =
      this.config.APIUrl + "Member/GetAllByPoolID?LoginKey=" + this.config.loginKey + "&PoolID="+poolID;
    console.log(url);
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  TransferTickets(data): Observable<any> {
    const url =
      this.config.APIUrl + "Entries/TransferTickets/?LoginKey=" + this.config.loginKey + "&EntryId=" + data.EntryId + "&NewOwnerId=" + data.NewOwnerId;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }

  // TransferMultipleTickets(OldMemberID, NewOwnerId, data): Observable<any> {
  //   const url = 
  //     this.config.APIUrl + "Entries/TransferTicketsMultiple/?LoginKey=" + this.config.loginKey + "&OldMemberID=" + OldMemberID + "&NewOwnerId=" + NewOwnerId;
  //   return this.http.post(url, data, httpOptions).pipe(catchError(this.handleError));
  // }
}
