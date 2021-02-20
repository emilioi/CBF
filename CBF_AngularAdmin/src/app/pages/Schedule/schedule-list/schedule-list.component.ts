import { Component, OnInit } from "@angular/core";
import { ScheduleService } from "../schedule.service";
import { FormGroup, FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, NavigationExtras } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { map } from "rxjs/operators";
import Swal from "sweetalert2";
import { Config } from "src/app/utility/config";

@Component({
  selector: "app-schedule-list",
  templateUrl: "./schedule-list.component.html",
  styleUrls: ["./schedule-list.component.scss"]
})
export class ScheduleListComponent implements OnInit {
  poolFrm: FormGroup;
  menus: any;
  scheduleList: any;
  poolId: any;
  Weeks: any;
  FirstWeeks: any;
  pooName: any;
  show: boolean;
  weekNumber: any;
  weekNumberText: string;
  SelectedPoolId: any;
  collection = [];

  constructor(
    private api: ScheduleService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private config: Config,
    private route: ActivatedRoute
  ) {
    // this.route.queryParamMap
    //   .pipe(map(params => params.get("page")))
    //   .subscribe(page => {
    //     console.log(page);
    //     this.config.currentPage = page;
    //     if (typeof page != "undefined" && page != null)
    //       this.getNFLDataByWeek(page);
    //   });
    // for (let i = 1; i <= 100; i++) {
    //   this.collection.push(`item ${i}`);
    // }
  }

  ngOnInit() {
    this.getScheduleMenu();
  }
  // pageChange(newPage: number) {
  //   this.router.navigate(["/schedule-list/"], {
  //     queryParams: { page: newPage }
  //   });
  // }

  weekChange(newPage: number) {
    this.router.navigate(["/schedule-list/"], {
      queryParams: { page: newPage }
    });
  }

  getScheduleMenu() {
    this.config.startLoader();
    this.api.getScheduleMenu().subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.menus = res.scheduleMenus;
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getWeekListforPAger() {
    this.config.startLoader();
    this.Weeks = null;
    this.api.GetWeeksListByPoolForPager(this.SelectedPoolId).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.Weeks = res.weekNumbers;
          //this.weekNumberText = "Week " + res.weekNumbers[0];
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getNFLDataByWeek(obj) {
    this.weekNumber = obj;
    this.weekNumberText = "Week " + this.weekNumber;
    let data = {
      pool_ID: this.SelectedPoolId,
      week: this.weekNumber
    };
    this.config.startLoader();
    this.api.GetNFLScheduleWeeksListByWeek(data).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.scheduleList = res.scheduleWeekLists;
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  selectedWeek(menu) {
    this.SelectedPoolId = menu.pool_ID;
    this.pooName = menu.pool_Name;
    let data = {
      pool_ID: this.SelectedPoolId,
      week: this.weekNumberText
    };
    this.config.startLoader();
    this.api.GetNFLScheduleWeeksListByWeek(data).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.scheduleList = res.scheduleWeekLists;
          this.getWeekListforPAger();
          this.getNFLDataByWeek(0);
          this.show = true;
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  addNewWeek() {
    let data = {
      poolID: this.SelectedPoolId,
      pool_Name: this.pooName,
      week_Number: this.weekNumber,
      cut_Off_Date: "",
      start: ""
    };
    if (
      this.pooName == "" ||
      this.pooName == null ||
      this.pooName == undefined
    ) {
      Swal.fire("Oops...", "Please select any pool!", "error");
    } else {
      let navigationExtras: NavigationExtras = {
        queryParams: {
          userInfo: data
        }
      };
      this.router.navigate(["/add-schedule"], navigationExtras);
    }
  }
  //DeleteSchedule
  DeleteSchedule(GameSchedule) {
    console.log(GameSchedule);
  }
  EditSchedule(GameSchedule) {
    this.router.navigate(["/edit-schedule"], {
      queryParams: {
        scheduleID: GameSchedule.scheduleID,
        poolID: GameSchedule.poolID,
        pool_Name: GameSchedule.pool_Name
      }
    });
  }
}
