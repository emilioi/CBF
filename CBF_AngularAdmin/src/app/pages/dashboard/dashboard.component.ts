import { Component, OnInit } from "@angular/core";
import { DashboardService } from "./dashboard.service";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"]
})
export class DashboardComponent implements OnInit {
  fullName: string;
  Tasks: any;
  testUser: object;
  counts: any;
  latestLogins: Array<object>;
  userObj: any;
  constructor(
    private api: DashboardService,
     private config : Config 
  ) {
    this.latestLogins = [];
    this.counts = {};
  }

  ngOnInit() {
    //console.log(
      //"dashboard-load-key: " + JSON.parse(localStorage.getItem("userObj")).key
    //);

    this.dashboardCounts();
  }

  //Dashboard Comnts
  dashboardCounts() {
    this.config.startLoader();
    this.api.DasboardInfo().subscribe(
      res => {
        this.counts = res.dashboard;
        this.latestLogins = res.dashboard.latestLogins;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
}
