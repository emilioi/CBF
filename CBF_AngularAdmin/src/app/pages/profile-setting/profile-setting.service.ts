import { Injectable } from "@angular/core";
import { Observable, of, throwError } from "rxjs";
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from "@angular/common/http";
import { catchError, tap, map } from "rxjs/operators";

import { BehaviorSubject } from "rxjs";
import { Config } from 'src/app/utility/config';

const httpOptions = {
  headers: new HttpHeaders({
    "Content-Type": "application/json-patch+json"
  })
};
 
 
@Injectable({
  providedIn: "root"
})
export class ProfileSettingService {
  MemberId:any;
  constructor(private http: HttpClient,
    private config: Config
    ) {
     this.MemberId= JSON.parse(localStorage.getItem("userObj")).member_Id;
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
  resetPwd(data): Observable<any> {
   
    const url =
      this.config.APIUrl + "Account/AdminResetPassword?LoginKey="+ this.config.loginKey +"&Member_Id=" + this.MemberId;
      console.log("URL "+ url);
      return this.http
      .post(url, data, httpOptions)
      .pipe(catchError(this.handleError));
     
  }
}
