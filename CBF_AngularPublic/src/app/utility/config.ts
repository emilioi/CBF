import { NgxUiLoaderService } from "ngx-ui-loader";

export class Config {
  APIUrl: string;
  loginKey: string;
  memberId: string;

  constructor(private ngxService: NgxUiLoaderService) {
    //this.resolveLoginKeyPromise();
    if (
      this.loginKey !== "" &&
      this.loginKey !== "undefined" &&
      this.loginKey !== null
    ) {
    } else {
      this.loginKey =
        JSON.parse(localStorage.getItem("userObj")) !== null
          ? JSON.parse(localStorage.getItem("userObj")).key
          : "";
    }
    this.memberId =
      JSON.parse(localStorage.getItem("userObj")) !== null
        ? JSON.parse(localStorage.getItem("userObj")).member_Id
        : "";
    //this.APIUrl = "http://cbf-nowgray-com.nt1-p2stl.ezhostingserver.com/Api/";
    this.APIUrl = "http://uat-api.clubbigfun.com/api/";
    //this.APIUrl = "https://api.clubbigfun.com/Api/";
    //this.APIUrl = "http://localhost:65171/api/";
    //this.APIUrl = "https://localhost:44320/api/";
  }
  updateGlobalKey(key) {
    this.loginKey = key;
  }

  resolveLoginKeyPromise() {
    return new Promise(resolve => {
      this.loginKey =
        JSON.parse(localStorage.getItem("userObj")) !== null
          ? JSON.parse(localStorage.getItem("userObj")).key
          : "";
      resolve();
    });
  }

  startLoader() {
    this.ngxService.start();
  }
  stopLoader() {
    this.ngxService.stopAll();
  }
}
