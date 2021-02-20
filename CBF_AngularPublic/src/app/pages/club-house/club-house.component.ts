import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ClubHouseService } from "./club-house.service";
import { Config } from "src/app/utility/config";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import Swal from "sweetalert2";
import { formatDate } from "@angular/common";
import { LoginService } from "../login/login.service";

@Component({
  selector: "app-club-house",
  templateUrl: "./club-house.component.html",
  styleUrls: ["./club-house.component.scss"]
})
export class ClubHouseComponent implements OnInit {
  AddPasscode: FormGroup;
  club: any;
  showModal: boolean = false;
  clubs: any;
  clubsAll: any;
  logos: any;
  selectedClub: any;
  selectedClubId: any;
  passcode: any;
  showModalAddTicket: boolean = false;
  showModalPrivate: boolean;
  NewTickets: number;
  Tickets: any;
  private: boolean;
  currentDate: any;
  cutoffDate: any;
  showModalReminder: boolean;
  ShowticketCreated: boolean = false;
  PickList: any;
  DisplayJoinClubModalForFuture: boolean = true;
  CustomMessages: any;
  showModalCustom: boolean;
  totalAlerts: any;
  count: any = 0;
  hiddenAlerts: any = [];
  AlertList: any;
  constructor(
    private router: Router,
    private apiLogin: LoginService,
    private api: ClubHouseService,
    private config: Config,
    private fb: FormBuilder
  ) {
    this.AddPasscode = this.fb.group({
      passCode: new FormControl()
    });
  }

  ngOnInit() {
    const ShowWithoutPickModal = localStorage.getItem("ShowWithoutPickModal");
    if (ShowWithoutPickModal === "true") {
      this.getEntriesWithoutPick();
      localStorage.removeItem("ShowWithoutPickModal");
    }
    // this.PickList = localStorage.getItem("EntryLIst");
    // //console.log("EntryLIst on CLub " + JSON.stringify(this.PickList));
    // if (this.PickList !== null) {
    //   this.showModalReminder = true;
    // }
    this.GetClubs();
    this.GetClubsAll();
    this.DisplayJoinClubModalForFuture =
      localStorage.getItem("DisplayJoinClubModalForFuture") == "false"
        ? false
        : true;
    this.GetMemberAlert();
  }

  GetClubs() {
    this.config.startLoader();
    this.api.GetClubDetails().subscribe(
      res => {
        this.clubs = JSON.parse(JSON.stringify(res)).clubs;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  GetClubsAll() {
    this.config.startLoader();
    this.api.GetClubDetailsAll().subscribe(
      res => {
        this.clubsAll = JSON.parse(JSON.stringify(res)).clubs;
        this.config.stopLoader();
        ////console.log("Pool Details " + JSON.stringify(this.clubsAll));
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  joinClub(clubId, clubName, isprivate, passcode) {
    this.DisplayJoinClubModalForFuture =
      localStorage.getItem("DisplayJoinClubModalForFuture") == "false"
        ? false
        : true;
    this.selectedClub = clubName;
    this.selectedClubId = clubId;
    this.private = isprivate;
    this.passcode = passcode;
    ////console.log("Private= " + this.passcode)
    //Hide checkbox is not cliked

    if (isprivate == true) {
      this.showModalPrivate = true;
    } else if (this.DisplayJoinClubModalForFuture) {
      this.showModal = true;
    } else {
      //Hide checkbox is cliked
      this.showModal = false;
      this.showModalAddTicket = true;
    }
  }

  SubmitPrivatePool() {
    if (
      this.AddPasscode.value.passCode == "" ||
      this.AddPasscode.value.passCode == null ||
      typeof this.AddPasscode.value.passCode == "undefined"
    ) {
      Swal.fire("Failed", "Please enter passcode.", "error");
      return;
    }
    if (this.AddPasscode.value.passCode !== this.passcode) {
      Swal.fire("Failed", "Passcode is not valid.", "error");
      return;
    }
    this.showModalPrivate = false;
    if (this.DisplayJoinClubModalForFuture) {
      this.showModal = true;
    } else {
      this.showModalAddTicket = true;
    }
    this.AddPasscode.reset();
  }

  CancelPrivatePool() {
    this.showModalPrivate = false;
  }

  cancelAction() {
    this.showModal = false;
    this.showModalAddTicket = true;
    //this.router.navigateByUrl("/ticket");
  }
  GoBackFromAddTicket() {
    this.showModalAddTicket = false;
  }

  AddTickets(count, selectedClub) {
    this.config.startLoader();
    this.api.JoinClubAddTickets(count, selectedClub).subscribe(
      res => {
        console.log("CurrentPool " + this.selectedClubId);
        this.Tickets = JSON.parse(JSON.stringify(res)).survEntries;
        if (res.status == 1) {
          localStorage.setItem("NewTickets", this.Tickets);
          this.ShowticketCreated = true;
          this.NewTickets = count;
          this.router.navigateByUrl("/ticket/" + this.selectedClubId);
        } else {
          Swal.fire("Oops..", res.message, "error");
        }
      },
      err => {
        this.config.stopLoader();
        this.ShowticketCreated = false;
        throw new Error(err);
      }
    );
  }
  AddMoreTickets() {
    this.ShowticketCreated = false;
    this.NewTickets = 0;
  }
  GoToTickets() {
    this.router.navigateByUrl("/ticket");
  }

  setMyStyles(week, logo) {
    let styles = {
      width: (logo.pickCount / week.entriesCount) * 100 + "%"
    };
    return styles;
  }
  hideJoinModalForFuture(input: HTMLInputElement) {
    //console.log(input);
    if (input.checked === true) {
      localStorage.setItem("DisplayJoinClubModalForFuture", "false");
    } else {
      localStorage.removeItem("DisplayJoinClubModalForFuture");
    }
  }
  CheckCuttOff(obj) {
    return !obj.is_Started;
  }

  enterPool(pool_ID) {
    this.router.navigateByUrl("/ticket/" + pool_ID);
  }

  CancelReminder() {
    this.showModalReminder = false;
  }
  SubmitReminder() {
    this.showModalReminder = false;
    this.router.navigateByUrl("/ticket");
  }
  getEntriesWithoutPick() {
    this.apiLogin.getEntryWithoutPick().subscribe(
      res => {
        this.PickList = res.listString;
        if (
          res.listString !== null ||
          res.listString !== "" ||
          typeof res.listString !== "undefined"
        ) {
          if (res.listString.length > 0) {
            this.showModalReminder = true;
          }
        }
        //console.log("Entries " + this.PickList);
      },
      err => {
        throw new Error(err);
      }
    );
  }

  GetMemberAlert() {
    if (JSON.parse(localStorage.getItem("localAlerts")) !== null) {
      this.totalAlerts = JSON.parse(localStorage.getItem("localAlerts"));
    } else {
      this.api.GetMemberAlert().subscribe(res => {
        this.AlertList = res.member_Alerts;
        localStorage.setItem("localAlerts", JSON.stringify(this.AlertList));
        this.showModalCustom = true;
        this.totalAlerts = JSON.parse(localStorage.getItem("localAlerts"));
      });
    }
  }

  closeMessage(obj) {
    //debugger
    let Alerts = JSON.parse(localStorage.getItem("localAlerts"));
    let RemovedAlerts = this.totalAlerts.filter(
      item => item.alert_Id == obj.alert_Id
    );
    localStorage.removeItem(RemovedAlerts);
    this.totalAlerts = this.totalAlerts.filter(
      item => item.alert_Id !== RemovedAlerts[0].alert_Id
    );
    localStorage.setItem("localAlerts", JSON.stringify(this.totalAlerts));
  }

  // GetMemberAlert() {
  //   this.api.GetMemberAlert().subscribe(res => {
  //     this.totalAlerts = res.member_Alerts;//
  //     this.GetFilteredAlerts();
  //     this.showModalCustom = true;
  //   });
  // }

  // closeMessage(obj) {
  //   localStorage.setItem("hiddenAlerts"+obj.alert_Id, JSON.stringify(obj));
  //   this.GetFilteredAlerts();
  // }
  // GetFilteredAlerts() {
  //     this.totalAlerts.forEach(hidAlert => {
  //       let alerts = localStorage.getItem("hiddenAlerts"+hidAlert.alert_Id);
  //       if(alerts !== null)
  //       {
  //             this.totalAlerts = this.totalAlerts.filter(item => item.alert_Id != hidAlert.alert_Id);
  //       }
  //     });
  // }

  // GetMemberAlert() {
  //   this.api.GetMemberAlert().subscribe(res => {
  //     this.totalAlerts = res.member_Alerts;
  //     if (this.totalAlerts != null && this.totalAlerts.length > 0) {
  //       this.presentAlert(res.member_Alerts[0]);
  //     }
  //   });
  // }

  // presentAlert(msg) {
  //   this.CustomMessages = msg;
  //   console.log("Message" + JSON.stringify(msg));
  //   if (msg !== null) {
  //     this.showModalCustom = true;
  //   } else {
  //     this.showModalCustom = false;
  //   }
  // }

  // closeMessage() {
  //   this.count++;
  //   this.showModalCustom = false;
  //   if (this.totalAlerts.length > this.count) {
  //     this.presentAlert(this.totalAlerts[this.count]);
  //   }
  // }
 
}
