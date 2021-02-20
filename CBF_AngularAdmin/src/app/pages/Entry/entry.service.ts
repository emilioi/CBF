import { Injectable } from "@angular/core";
import {
  HttpHeaders,
  HttpClient,
  HttpErrorResponse
} from "@angular/common/http";
import { Config } from "src/app/utility/config";
import { throwError, Observable } from "rxjs";
import { catchError } from "rxjs/operators";

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};

let loginKey = "";

@Injectable({
  providedIn: "root"
})
export class EntryService {
 
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

  getEntryMenu(): Observable<any> {
    const url =
      this.config.APIUrl + "Entries/GetEntryMenu?LoginKey=" + loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetEntryWeeksList(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/GetEntriesWeeksList?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  SearchEntry(name: any, pool_ID): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/SearchEntry?LoginKey=" +
      loginKey +
      "&EntryName=" +
      name +
      "&Pool_ID=" +
      pool_ID;;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetPoolEntriesById(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/GetEntryById?EntryID=" +
      Id +
      "&LoginKey=" +
      loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  getWeekNumber(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Weeks/GetWeekNumbers?LoginKey=" +
      loginKey +
      "&Pool_ID=" +
      data.pool_ID;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  updateEntry(data): Observable<any> {
    const url = this.config.APIUrl + "Entries/UpdateEntries?LoginKey=" + this.config.loginKey;
    return this.http.post(url, data, httpOptions).pipe(catchError(this.handleError));
  }


  GetPickReportWithLogo(Id): Observable<any> {
 
    const url = this.config.APIUrl + "Picks/PickReportWithLogo?LoginKey=" + this.config.loginKey +"&EntryID="+  Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetEntriesWeeksListByReferral(referral): Observable<any> {
    const url = this.config.APIUrl + "Entries/GetEntriesWeeksListByReferral?LoginKey=" + this.config.loginKey +"&Referral="+  referral;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }


  deletEntryPool(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Entries/DeletePoolEntriesById?EntryID=" +
      Id +
      "&LoginKey=" +
      loginKey;
      console.log("URL="+url);
      return this.http.delete<any[]>(url);
  }

  JoinClubAddTickets(PoolId, MemberEmail, NoOfTickets): Observable<any> {
    console.log(PoolId),'pojahs';
    const url = this.config.APIUrl + "Entries/JoinClubAddTicketsFromAdmin?LoginKey=" + this.config.loginKey+ "&MemberEmail=" + MemberEmail + "&PoolId=" + PoolId + "&NoOfTickets="+ NoOfTickets;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
}
