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
export class MemberService {
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

  checkUserNameExist(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/UserNameExist/" +
      data +
      "?LoginKey=" +
      loginKey;

    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  checkEmailIdExist(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/EmailExist/Member?email=" +
      data +
      "&LoginKey=" +
      loginKey;

    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  upLoadBase64(memeberid, data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/MemberFileUploadBase64?LoginKey=" +
      loginKey +
      "&Member_Id=" +
      memeberid;

    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  GetAllUser(reference): Observable<any> {
    //Member/GetAll?LoginKey=debuig&PageNo=1&reference=RA
    const url =
      this.config.APIUrl +
      "Member/GetAll/" +
      "?LoginKey=" +
      loginKey +
      "&PageNo=1&reference=" +
      reference;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetMemberById(Id): Observable<any> {
    const url = this.config.APIUrl + "Member/" + Id + "?LoginKey=" + loginKey;

    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  GetMemberBySearch(name): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/MemberSearch?LoginKey=" +
      loginKey +
      "&name=" +
      name;
    console.log("URL: " + url);
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  MemberFilter(data, reference): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/MemberFilter?LoginKey=" +
      loginKey +
      "&reference=" +
      reference;
    console.log("URL: " + url);
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  getAllMemberByGroupAdminID(GroupAdminID): Observable<any> {
    const url =
      this.config.APIUrl +
      "Admin/getAllMemberByGroupAdminID?LoginKey=" +
      loginKey +
      "&GroupAdminID=" +
      GroupAdminID;
    return this.http.post(url, httpOptions).pipe(catchError(this.handleError));
  }
  AssignGroupAdmin(GroupAdminID, members, remove): Observable<any> {
    const url =
      this.config.APIUrl +
      "Admin/AssignGroupAdmin?LoginKey=" +
      loginKey +
      "&GroupAdminID=" +
      GroupAdminID +
      "&remove=" +
      remove;
    return this.http
      .post(url, members, httpOptions)
      .pipe(catchError(this.handleError));
  }
  createUser(data): Observable<any> {
    const url = this.config.APIUrl + "Member?LoginKey=" + loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  verify(data, Id, Is_Verified): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/VerifyMember?LoginKey=" +
      this.config.loginKey +
      "&Member_Id=" +
      Id +
      "&IsVerified=" +
      Is_Verified;
    console.log("URL " + url);
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  postMailinglist(data): Observable<any> {
    const url =
      this.config.APIUrl +
      "Email/EditMailingList?LoginKey=" +
      this.config.loginKey;
    console.log("URL " + url);
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }

  GetStates(CountryId): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/GetStateListByCountries?LoginKey=" +
      this.config.loginKey +
      "&Country_ID=" +
      CountryId; //+ this.config.loginKey +
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetDefaultStates(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/GetStateListByCountries?LoginKey=" +
      this.config.loginKey +
      "&Country_ID=38"; //+ this.config.loginKey +
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  GetCountry(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Common/GetCountriesList?LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  deleteMember(Id): Observable<any> {
    const url = this.config.APIUrl + "Member/" + Id + "?LoginKey=" + loginKey;
    return this.http.delete<any[]>(url);
  }

  getReferenceList(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Member/GetReferenceList?LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  getVerifiedMembers(name, verified): Observable<any> {
    let Check = verified ? 1 : 0;
    let Mambername = name ? name : "";
    const url =
      this.config.APIUrl +
      "Member/MemberSearch?LoginKey=" +
      this.config.loginKey +
      "&name=" +
      Mambername +
      "&IsVerified=" +
      Check;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }

  //** Alert Services Start **/
  GetAlertList(): Observable<any> {
    const url =
      this.config.APIUrl +
      "Alerts/GetAllAlerts?LoginKey=" +
      this.config.loginKey;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  saveAlert(data): Observable<any> {
    const url = this.config.APIUrl + "Alerts?LoginKey=" + this.config.loginKey;
    return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
  }
  getById(Id): Observable<any> {
    const url =
      this.config.APIUrl +
      "Alerts/GetById?LoginKey=" +
      this.config.loginKey +
      "&Alert_Id=" +
      Id;
    return this.http.get(url, httpOptions).pipe(catchError(this.handleError));
  }
  deleteAlert(Id): Observable<any> {
    const url = this.config.APIUrl + "Alerts/DeleteAlert?id=" + Id + "&LoginKey=" + loginKey;
    return this.http.delete<any[]>(url);
  }
}
