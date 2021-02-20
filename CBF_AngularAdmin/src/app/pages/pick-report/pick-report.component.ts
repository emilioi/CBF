import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { ReportService } from "./report.service";
import { Router, ActivatedRoute } from "@angular/router";
import { ScheduleService } from "../Schedule/schedule.service";
import Swal from "sweetalert2";
import { Config } from "src/app/utility/config";
import { EntryService } from "../Entry/entry.service";
import { AngularCsv } from "angular-csv-ext/dist/Angular-csv";

@Component({
  selector: "app-pick-report",
  templateUrl: "./pick-report.component.html",
  styleUrls: ["./pick-report.component.scss"]
})
export class PickReportComponent implements OnInit {
  Weeks: any;
  poolId: any;
  show: boolean;
  Reports: any;
  menus: any;
  scheduleList: any;
  pooName: any;
  weekNumber: any;
  currentWeek: any;
  filteredItems: any;
  showTicketTextbox: boolean;
  showPickTextbox: boolean;
  showDateTextbox: boolean;
  showEliminatedTextbox: boolean;
  showDefaultedTextbox: boolean;
  filterData: any = {
    isFilter: false,
    isSorting: false,
    isAscending: true,
    shortByName: "First_Name",
    filterByName: "None",
    filterByValue: ""
  };
  adminType: any;
  currentWeekNo: any;
  showPicks: boolean;
  constructor(
    private api: ScheduleService,
    private service: ReportService,
    private WeekApi: EntryService,
    private config: Config
  ) { }

  ngOnInit() {
    this.weeekMenus();
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
    this.getCurrentWeek();

  }

  weekChange(newPage: number) {
    ({
      queryParams: { page: newPage }
    });
  }

  getCurrentWeek() {
    this.service.getCurrentWeek().subscribe(res => {
      this.currentWeekNo = res.weekNumber;
    })
  }

  //----

  weeekMenus() {
    this.config.startLoader();
    this.WeekApi.getEntryMenu().subscribe(
      res => {
        //console.log("GetEntriesMenu", res);
        if (res.status == 1) {
          this.config.stopLoader();
          this.menus = res.entryMenus;
          //console.log(this.weekMenuList);this.selectedWeek(this.menus);
        }
      },
      err => {
        //console.log(err);
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  selectedWeek(menu) {
    this.show = true;
    this.poolId = menu.pool_ID;
    this.pooName = menu.pool_Name;
    // let data = {
    //   pool_ID: this.poolId,
    //   week: 1
    // };
    // this.config.startLoader();
    // this.api.GetNFLScheduleWeeksListByWeek(data).subscribe(
    //   res => {
    //     this.config.stopLoader();
    //     if (res.status == 1) {
    //       this.scheduleList = res.scheduleWeekLists;
    this.getWeekListforPAger();
    //       this.show = true;
    //       this.weekNumber = "Week 1";
    //     }
    //   },
    //   err => {
    //     this.config.stopLoader();
    //     throw new Error(err);
    //   }
    // );
  }
  getWeekListforPAger() {
    this.config.startLoader();
    this.Weeks = null;
    this.api.GetWeeksListByPoolForPager(this.poolId).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          // console.log(res);
          this.Weeks = res.weekNumbers;
          // this.getNFLDataByWeek(1);
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  //-----
  // weeekMenus() {
  //   this.config.startLoader();
  //   this.api.getScheduleMenu().subscribe(
  //     res => {
  //       this.config.stopLoader();
  //       if (res.status == 1) {
  //         this.menus = res.scheduleMenus;
  //       }
  //     },
  //     err => {
  //       this.config.stopLoader();
  //       throw new Error(err);
  //     }
  //   );
  // }
  // getWeekListforPAger() {
  //   this.config.startLoader();
  //   this.Weeks = null;
  //   this.api.GetWeeksListByPoolForPager(this.poolId).subscribe(
  //     res => {
  //       this.config.stopLoader();
  //       if (res.status == 1) {
  //         this.Weeks = res.weekNumbers;
  //         console.log("Test Week" + this.Weeks);
  //       }
  //     },
  //     err => {
  //       this.config.stopLoader();
  //       throw new Error(err);
  //     }
  //   );
  // }
  // selectedWeek(menu) {
  //   this.poolId = menu.pool_ID;
  //   this.pooName = menu.pool_Name;
  //   let data = {
  //     pool_ID: this.poolId,
  //     week: 1
  //   };
  //   this.config.startLoader();
  //   this.api.GetNFLScheduleWeeksListByWeek(data).subscribe(
  //     res => {
  //       this.config.stopLoader();
  //       if (res.status == 1) {
  //         this.scheduleList = res.scheduleWeekLists;
  //         this.getWeekListforPAger();
  //         this.getNFLDataByWeek(1)
  //         this.show = true;
  //         this.weekNumber = "Week 1";
  //       }
  //     },
  //     err => {
  //       this.config.stopLoader();
  //       throw new Error(err);
  //     }
  //   );
  // }
  getNFLDataByWeek(obj) {
    this.weekNumber = "Week " + obj;
    this.currentWeek = obj;
    this.config.startLoader();

    if (obj >= this.currentWeekNo) {
      this.showPicks = false;
    }
    else {
      this.showPicks = true;
    }
    this.service.GetPickReport(this.poolId, obj).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          //console.log("this.Reports " + JSON.stringify(res));
          this.Reports = res.pickReport;
          this.show = true;
          // console.log(this.Reports);
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  exportTOCSV() {
    var options = {
      fieldSeparator: ",",
      quoteStrings: '"',
      decimalseparator: ".",
      noDownload: false,
      headers: [
        "Ticket ID",
        "Pick",
        "Date",
        "Eliminated",
        "Defaulted",
        "Defaults"
      ],
      nullToEmptyString: true
    };
    var PickReports = [];
    this.Reports.forEach(objReport => {
      let pickReport = {
        ticket: objReport.ticket,
        pick: objReport.pick,
        date: objReport.date,
        eliminated: objReport.eliminated,
        defaulted: objReport.defaulted,
        defaults: objReport.defaults
      };
      PickReports.push(pickReport);
    });
    new AngularCsv(PickReports, "Pick Report", options);
  }
  printReport() {
    Swal.fire("Oops...", "This feature is not enabled!", "error");
  }

  sortPickReportBy(shortByName) {
    this.filterData.isAscending = !this.filterData.isAscending;
    this.filterData.isSorting = true;
    this.filterData.shortByName = shortByName;
    debugger;
    switch (shortByName) {
      case "ticket": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.ticket.localeCompare(b.ticket));
        } else {
          this.Reports.sort((a, b) => b.ticket.localeCompare(a.ticket));
        }
        break;
      }
      case "pick": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.pick.localeCompare(b.pick));
        } else {
          this.Reports.sort((a, b) => b.pick.localeCompare(a.pick));
        }
        break;
      }
      case "date": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.date.localeCompare(b.date));
        } else {
          this.Reports.sort((a, b) => b.date.localeCompare(a.date));
        }
        break;
      }
      case "eliminated": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.eliminated.localeCompare(b.eliminated));
        } else {
          this.Reports.sort((a, b) => b.eliminated.localeCompare(a.eliminated));
        }
        break;
      }
      case "defaulted": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.defaulted.localeCompare(b.defaulted));
        } else {
          this.Reports.sort((a, b) => b.defaulted.localeCompare(a.defaulted));
        }
        break;
      }
      case "defaults": {
        if (this.filterData.isAscending) {
          this.Reports.sort((a, b) => a.defaults.localeCompare(b.defaults));
        } else {
          this.Reports.sort((a, b) => b.defaults.localeCompare(a.defaults));
        }
        break;
      }
    }
  }
  filterByTicket(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.ticket.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Reports = this.filteredItems;
  }

  filterByPick(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.pick.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Reports = this.filteredItems;
  }

  filterBydate(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.date.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Reports = this.filteredItems;
  }

  filterByEliminated(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.eliminated.indexOf(value) > -1
    );
    this.Reports = this.filteredItems;
  }
  filterByDefaulted(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Reports).filter(
      Reports =>
        Reports.defaulted.indexOf(value) > -1
    );
    this.Reports = this.filteredItems;
  }
  EntryFilterChange(filterByName) {
    if (filterByName == "None") {
      this.showTicketTextbox = false;
      this.showPickTextbox = false;
      this.showDateTextbox = false;
      this.showEliminatedTextbox = false;
      this.showDefaultedTextbox = false;
      this.getNFLDataByWeek(this.currentWeek);
    } else if (filterByName == "Ticket") {
      this.showTicketTextbox = true;
      this.showPickTextbox = false;
      this.showDateTextbox = false;
      this.showEliminatedTextbox = false;
      this.showDefaultedTextbox = false;
    } else if (filterByName == "Pick") {
      this.showTicketTextbox = false;
      this.showPickTextbox = true;
      this.showDateTextbox = false;
      this.showEliminatedTextbox = false;
      this.showDefaultedTextbox = false;
    } else if (filterByName == "Date") {
      this.showTicketTextbox = false;
      this.showPickTextbox = false;
      this.showDateTextbox = true;
      this.showEliminatedTextbox = false;
      this.showDefaultedTextbox = false;
    } else if (filterByName == "Eliminated") {
      this.showTicketTextbox = false;
      this.showPickTextbox = false;
      this.showDateTextbox = false;
      this.showEliminatedTextbox = true;
      this.showDefaultedTextbox = false;
    } else if (filterByName == "Defaulted") {
      this.showTicketTextbox = false;
      this.showPickTextbox = false;
      this.showDateTextbox = false;
      this.showEliminatedTextbox = false;
      this.showDefaultedTextbox = true;
    }
  }


}
