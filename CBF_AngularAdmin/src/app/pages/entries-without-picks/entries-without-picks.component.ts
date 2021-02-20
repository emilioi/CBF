import { Component, OnInit } from "@angular/core";
import { ScheduleService } from "../Schedule/schedule.service";
import { Config } from "src/app/utility/config";
import { WeekService } from "../Week/week.service";
import { EntryService } from "../Entry/entry.service";
import { AngularCsv } from "angular-csv-ext/dist/Angular-csv";

@Component({
  selector: "app-entries-without-picks",
  templateUrl: "./entries-without-picks.component.html",
  styleUrls: ["./entries-without-picks.component.scss"]
})
export class EntriesWithoutPicksComponent implements OnInit {
  Weeks: any;
  poolId: any;
  show: boolean;
  Entries: any;
  menus: any;
  scheduleList: any;
  pooName: any;
  weekNumber: any;
  filteredItems: any;
  currentWeek: any;
  showEntryNameTextbox: boolean;
  showLoginIdTextbox: boolean;
  showNameTextbox: boolean;
  showDefaultsTextbox: boolean;
  showEliminateTextbox: boolean;
  filterData: any = {
    isFilter: false,
    isSorting: false,
    isAscending: true,
    shortByName: "First_Name",
    filterByName: "None",
    filterByValue: ""
  };
  constructor(
    private api: ScheduleService,
    private WeekApi: EntryService,
    private config: Config
  ) { }

  ngOnInit() {
    this.weeekMenus();
  }

  weekChange(newPage: number) {
    ({
      queryParams: { page: newPage }
    });
  }

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

  exportTOCSV() {
    var options = {
      fieldSeparator: ",",
      quoteStrings: '"',
      decimalseparator: ".",
      noDownload: false,
      headers: [
        "Member ID",
        "Pool ID",
        "Pool Name",
        "Entry Name",
        "Login",
        "Name",
        "Defaults",
        "Eliminated?"
      ],
      nullToEmptyString: true
    };
    var EntryReports = [];
    this.Entries.forEach(objEntry => {
      let entryReport = {
        memberID: objEntry.memberID,
        entryID: objEntry.entryID,
        poolID: objEntry.poolID,
        entryName: objEntry.entryName,
        login_ID: objEntry.login_ID,
        fullName: objEntry.fullName,
        defaults: objEntry.defaults,
        eliminated: objEntry.eliminated
      };
      EntryReports.push(entryReport);
    });
    new AngularCsv(EntryReports, "Entry Without Pick Report", options);
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
          //this.getNFLDataByWeek(1);
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getNFLDataByWeek(obj) {
    this.config.startLoader();
    this.weekNumber = "Week " + obj;
    this.currentWeek = obj;
    this.api.GetWithoutPickPool(this.poolId, obj).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          //console.log(res);
          this.Entries = JSON.parse(
            JSON.stringify(res)
          ).survEntries_WithoutPicks;
          this.show = true;
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  filterByEntry(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Entries).filter(
      Entries =>
        Entries.entryName.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    ///console.log("Filterd Data1: " + JSON.stringify(this.filteredItems));
    this.Entries = this.filteredItems;
  }

  filterByLogin(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Entries).filter(
      Entries =>
        Entries.login_ID.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    //console.log("Filterd Data2: " + JSON.stringify(this.filteredItems));
    this.Entries = this.filteredItems;
  }

  filterByName(value) {
    //console.log("Fuul Name: " + value);
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Entries).filter(
      Entries =>
        Entries.fullName.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Entries = this.filteredItems;
  }
  filterByDefault(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Entries).filter(
      Entries =>
        Entries.defaults.indexOf(value) > -1
    );
    this.Entries = this.filteredItems;
  }
  filterByEliminate(value) {
    if (!value) {
      this.getNFLDataByWeek(this.currentWeek);
    } // when nothing has typed
    this.filteredItems = Object.assign([], this.Entries).filter(
      Entries =>
        Entries.eliminated.toLowerCase().indexOf(value.toLowerCase()) > -1
    );
    this.Entries = this.filteredItems;
  }
  EntryFilterChange(filterByName) {
    if (filterByName == "None") {
      this.showEntryNameTextbox = false;
      this.showLoginIdTextbox = false;
      this.getNFLDataByWeek(this.currentWeek);
    } else if (filterByName == "Entry Name") {
      this.showEntryNameTextbox = true;
      this.showLoginIdTextbox = false;
      this.showNameTextbox = false;
      this.showDefaultsTextbox = false;
      this.showEliminateTextbox = false;
    } else if (filterByName == "Login Id") {
      this.showEntryNameTextbox = false;
      this.showLoginIdTextbox = true;
      this.showNameTextbox = false;
      this.showDefaultsTextbox = false;
      this.showEliminateTextbox = false;
    } else if (filterByName == "Name") {
      this.showEntryNameTextbox = false;
      this.showLoginIdTextbox = false;
      this.showNameTextbox = true;
      this.showDefaultsTextbox = false;
      this.showEliminateTextbox = false;
    } else if (filterByName == "Defaults") {
      this.showEntryNameTextbox = false;
      this.showLoginIdTextbox = false;
      this.showNameTextbox = false;
      this.showDefaultsTextbox = true;
      this.showEliminateTextbox = false;
    } else if (filterByName == "Eliminate") {
      this.showEntryNameTextbox = false;
      this.showLoginIdTextbox = false;
      this.showNameTextbox = false;
      this.showDefaultsTextbox = false;
      this.showEliminateTextbox = true;
    }
  }

  sortEntryListBy(shortByName) {
    this.filterData.isAscending = !this.filterData.isAscending;
    this.filterData.isSorting = true;
    this.filterData.shortByName = shortByName;
    debugger;
    switch (shortByName) {
      case "Entry_Name": {
        if (this.filterData.isAscending) {
          this.Entries.sort((a, b) => a.entryName.localeCompare(b.entryName));
        } else {
          this.Entries.sort((a, b) => b.entryName.localeCompare(a.entryName));
        }
        break;
      }
    }
  }
}
