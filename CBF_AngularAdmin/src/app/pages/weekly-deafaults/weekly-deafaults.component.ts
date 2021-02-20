import { Component, OnInit } from "@angular/core";

import { FormGroup, FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, NavigationExtras } from "@angular/router";
import { map } from "rxjs/operators";
import Swal from "sweetalert2";
import { WeeklyDeafaultsService } from "./weekly-deafaults.service";
import { Config } from "src/app/utility/config";

interface QrySchedule {
  id: number;
  pool_Id: number;
  weekNumber: number;
  schedule_Id: number;
  team_Id: number;
  rank: number;
}
@Component({
  selector: "app-weekly-deafaults",
  templateUrl: "./weekly-deafaults.component.html",
  styleUrls: ["./weekly-deafaults.component.scss"]
})
export class WeeklyDeafaultsComponent implements OnInit {
  poolFrm: FormGroup;
  menus: any;
  scheduleList: any;
  poolId: any;
  Weeks: any;
  pagerConfig: any;
  pooName: any;
  show: boolean;
  weekNumber: any;
  weekNumberText: string;
  SelectedPoolId: any;
  QryScheduleCollection = Array<QrySchedule>();
  listStyle: any;
  CurrentWeeklyDefaults: any;
  constructor(
    private api: WeeklyDeafaultsService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private config: Config,
    private route: ActivatedRoute
  ) {
    this.pagerConfig = {
      currentPage: 1,
      itemsPerPage: 10
    };

    this.listStyle = {
      width: "100%", //width of the list defaults to 300,
      height: "650px", //height of the list defaults to 250,
      dropZoneHeight: "50px" // height of the dropzone indicator defaults to 50
    };
  }

  ngOnInit() {
    this.getScheduleMenu();
  }

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
    this.Weeks = null;
    this.config.startLoader();
    this.api.GetWeeksListByPoolForPager(this.SelectedPoolId).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.Weeks = res.weekNumbers;
          console.log(this.Weeks);
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
    this.api.GetWeeklyDefaultsSchedule(data).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.scheduleList = res.weeklyDefaults;
          this.CurrentWeeklyDefaults = this.scheduleList;
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
      week: 1
    };
    this.getWeekListforPAger();
    this.getNFLDataByWeek(0);

    this.show = true;
    //this.weekNumberText = 'Week 1';
    // this.api.GetWeeklyDefaultsSchedule(data).subscribe(
    //   res => {
    //     if (res.status == 1) {
    //       this.scheduleList = res.weeklyDefaults;
    //       this.CurrentWeeklyDefaults= this.scheduleList;

    //       this.show = true;
    //       this.weekNumberText = 'Week 1';
    //     }
    //   },
    //   err => {
    //     throw new Error(err);
    //   }
    // );
  }

  listSorted(list) {
    this.updateItem(list);
    console.log(list);
    this.CurrentWeeklyDefaults = list;
  }
  updateItem(list) {
    for (let i = 0; i < list.length; i++) {
      list[i].rank = i;
    }
    return list;
  }

  SaveAllWeeklyDefaults() {
    this.config.startLoader();
    this.api.SetDefaultPicks(this.CurrentWeeklyDefaults).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
        } else {
          Swal.fire("Oops...", res.message, "error");
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
}
