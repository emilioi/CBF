import { Injectable } from "@angular/core";
import {
  HttpErrorResponse,
  HttpHeaders,
  HttpClient
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { Config } from "src/app/utility/config";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
@Injectable({
  providedIn: "root"
})
export class PickAnalysisService {

 
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

  PickAnalysisWithTeam(PoolId): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/PickAnalysisWithTeam?LoginKey=" +
      this.config.loginKey +
      "&PoolId=" +
      PoolId;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  PickAnalysisAliveByMember(
    PoolId,
    IsAll,
    PageNo,
    searchText
  ): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/PickAnalysisAliveByMemberLazyLoad?LoginKey=" +
      this.config.loginKey +
      "&PoolId=" +
      PoolId +
      "&Member_Id=" +
      this.config.memberId +
      "&AllTickets=" +
      IsAll +
      "&PageNo=" +
      PageNo +
      "&searchText=" +
      searchText;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  PickAnalysisEliminatedByMember(PoolId, searchText): Observable<any> {
    const url =
      this.config.APIUrl +
      "Picks/PickAnalysisEliminatedByMember?LoginKey=" +
      this.config.loginKey +
      "&PoolId=" +
      PoolId +
      "&Member_Id=" +
      this.config.memberId +
      "&searchText=" +
      searchText;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
}
