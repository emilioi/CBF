import { Component, OnInit } from "@angular/core";
import { Config } from "src/app/utility/config";
import { ActivatedRoute, Router } from "@angular/router";
import { PickCenterService } from "./pick-center.service";
import Swal from "sweetalert2";
import { ClubHouseService } from "../club-house/club-house.service";

@Component({
  selector: "app-pick-center",
  inputs: ["model"],
  templateUrl: "./pick-center.component.html",
  styleUrls: ["./pick-center.component.scss"]
})
export class PickCenterComponent implements OnInit {
  CurrentTicketId: any;
  Entry: any;
  Weeks: any;
  Schedules: any;
  showModal: any;
  logurl: any;
  teamname: any;
  entryId: any;
  currentSchedule: any;
  currentPickedSchedule: any;
  winnerTeam: any;
  CurrentPickList: any;
  CurrentPick: any;
  weekNumber: any = 1;
  LogoWeeks: any;
  ShowCurrentPick: boolean = false;
  MakeNewPick: boolean = false;
  WeekClose: boolean = false;
  NotLoading: boolean = true;
  CurrentWeek: any;
  PooolID: any;
  selectedweek: any;
  ClubDetail: any;
  selectedClubId: any;
  constructor(
    private config: Config,
    private route: ActivatedRoute,
    private api: PickCenterService,
    private router: Router,
    private apiClub: ClubHouseService
  ) {
    this.CurrentTicketId = this.route.snapshot.paramMap.get("ticketId");
  }

  ngOnInit() {
    this.weekNumber = this.route.snapshot.queryParamMap.get("week");
    //console.log(this.weekNumber);
    this.getEntry();
    this.getCurrentPick();
    this.getPickReportWithLogos();
    //console.log('WeekOpen--', this.WeekClose);
  }
  getEntry() {
    this.config.startLoader();
    this.api.GetEntryById(this.CurrentTicketId).subscribe(
      res => {
        this.Entry = JSON.parse(JSON.stringify(res)).survEntries;
        this.selectedClubId = this.Entry.poolID;
        //console.log(this.Entry );
        // //console.log(this.Entry.poolID);
        this.entryId = JSON.parse(JSON.stringify(res)).survEntries.entryName;
        this.getWeeks(this.Entry.poolID);
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getCurrentPick() {
    this.config.startLoader();
    this.api.GetEntryPickListById(this.CurrentTicketId).subscribe(
      res => {
        this.CurrentPickList = JSON.parse(JSON.stringify(res)).survEntryPicks;
        this.FilterCurrentPick();
        //console.log('1.CurrentPick--', this.CurrentPickList);
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  getPickReportWithLogos() {
    this.config.startLoader();
    this.api.GetPickReportWithLogo(this.CurrentTicketId).subscribe(
      res => {
        this.LogoWeeks = JSON.parse(JSON.stringify(res)).pickReport;
        //console.log(this.LogoWeeks);
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getWeeks(PoolId) {
    this.config.startLoader();
    this.api.getWeeks(PoolId).subscribe(
      res => {
        this.Weeks = res.poolMapped;
       ///console.log('getWeeks', this.Weeks[0]);
       //console.log('getWeeks-------', this.weekNumber);
        this.getSchedules(
          PoolId,
          this.weekNumber,
          this.Weeks[0]
        );

        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  getSchedules(PoolId, weekNumber, currentWeek) {
    this.router.navigateByUrl(
      "/pick-center/" + this.CurrentTicketId + "?week=" + weekNumber
    );
    this.NotLoading = false;
    this.config.startLoader();
    this.weekNumber = weekNumber;
    this.currentPickedSchedule = null;
    this.CurrentPick = null;
    this.CurrentWeek = currentWeek;
    //console.log(currentWeek, 'test current week');
    this.WeekClose = this.CurrentWeek.start;

    this.api.getSchedules(PoolId, weekNumber).subscribe(
      res => {
        this.Schedules = JSON.parse(JSON.stringify(res)).schedulesGroup;
        this.FilterCurrentPick();
        //work here
        // //console.log("2..CurrentPick-",this.CurrentPick);
        ////console.log("Schedules-", this.Schedules);
        if (this.CurrentPick != null) {
          this.GetCurrentPickScheduleFromList(
            this.Schedules,
            this.CurrentPick.scheduleID
          );
          // //console.log("currentPickedSchedule-",this.currentPickedSchedule);
        }
        this.ResetPickcenterLayoutForWeek();
        this.config.stopLoader();
        this.NotLoading = true;
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  MakePick(schedule, winner, IsHome) {
    console.log("Current Schedule ID-", schedule.scheduleID);
    let data = {
      entryID: this.Entry.entryID,
      scheduleID: schedule.scheduleID,
      yourWinner: winner,
      weekNumber: schedule.weekNumber
    };

    //this.currentSchedule = schedule;
    if (IsHome == true) {
      this.logurl = schedule.homeLogoImageSrc;
      this.teamname = schedule.homeTeamName;
      this.winnerTeam = winner;
      this.selectedweek = schedule.weekNumber;
      this.config.startLoader();
      this.api.MakePickTwiceValidate(data).subscribe(res => {
        if (res.status == "0") {
          Swal.fire({
            html:
              '<b>The <span style="color: red;"><span style="text-transform: uppercase;">' +
              this.teamname +
              '</span> cannot be used</span> for <span style="color:red;"><span style="text-transform: uppercase;">TICKET </span>' +
              this.Entry.entryName +
              "</span></b>" +
              "<br><br>" +
              '<img style="box-shadow: 0px 4px 6px #7a7a7a; padding: 6px; max-width: 162px;max-height: 112px;" src="../../../assets/logos/' +
              this.logurl +
              '"/>' +
              '<br><br><br><p style="color:red;"><b>Please select another team.</b></p> ',
            showCloseButton: true,
            focusConfirm: false,
            confirmButtonAriaLabel: "OK"
          });
          this.config.stopLoader();
        } else {
          Swal.fire({
            html:
              '<b>For WEEK <span style="text-transform: uppercase;">' +
              this.selectedweek +
              " :</span> <br><br>" +
              'You have selected the <span style="text-transform: uppercase;">' +
              this.teamname +
              '</span> for TICKET <span style="color: red;">' +
              this.Entry.entryName +
              "</span></b>" +
              '<br><br><img style="box-shadow: 0px 4px 6px #7a7a7a; padding: 6px; max-width: 162px;max-height: 112px;" src="../../../assets/logos/' +
              this.logurl +
              '"/>',
            showCancelButton: false,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "CONFIRM"
          }).then(result => {
            if (result.value) {
              this.api.makePick(data).subscribe(
                res => {
                  if (res.status == "1") {
                    this.getCurrentPick();
                    this.showModal = false;
                    this.Schedules = null;
                    this.getSchedules(
                      this.selectedClubId,
                      this.weekNumber,
                      this.CurrentWeek
                    );
                    this.getPickReportWithLogos();
                    Swal.fire("Success", res.message, "success");
                  } else {
                    Swal.fire("Failed", res.message, "error");
                  }
                  this.config.stopLoader();
                },
                err => {
                  this.config.stopLoader();
                  throw new Error(err);
                }
              );
            } else {
              this.config.stopLoader();
            }
          });
        }
      });
    } else {
      this.logurl = schedule.visitingLogoImageSrc;
      this.teamname = schedule.visitingTeamName;
      this.winnerTeam = winner;
      this.selectedweek = schedule.weekNumber;
      this.config.startLoader();
      this.api.MakePickTwiceValidate(data).subscribe(
        res => {
          if (res.status == "0") {
            Swal.fire({
              html:
                '<b>The <span style="color: red;"><span style="text-transform: uppercase;">' +
                this.teamname +
                '</span> cannot be used</span> for <span style="color:red;"><span style="text-transform: uppercase;">TICKET </span>' +
                this.Entry.entryName +
                "</span></b>" +
                "<br><br>" +
                '<img style="box-shadow: 0px 4px 6px #7a7a7a; padding: 6px; max-width: 162px;max-height: 112px;" src="../../../assets/logos/' +
                this.logurl +
                '"/>' +
                '<br><br><br><p style="color:red;"><b>Please select another team.</b></p> ',
              showCloseButton: true,
              focusConfirm: false,
              confirmButtonAriaLabel: "OK"
            });
            this.config.stopLoader();
          } else {
            Swal.fire({
              html:
                '<b>For WEEK <span style="text-transform: uppercase;">' +
                this.selectedweek +
                " :</span> <br><br>" +
                'You have selected the <span style="text-transform: uppercase;">' +
                this.teamname +
                '</span> for TICKET <span style="color: red;">' +
                this.Entry.entryName +
                "</span></b>" +
                '<br><br><img style="box-shadow: 0px 4px 6px #7a7a7a; padding: 6px; max-width: 162px;max-height: 112px;" src="../../../assets/logos/' +
                this.logurl +
                '"/>',
              showCancelButton: false,
              confirmButtonColor: "#3085d6",
              confirmButtonText: "CONFIRM"
            }).then(result => {
              if (result.value) {
                this.api.makePick(data).subscribe(
                  res => {
                    if (res.status == "1") {
                      this.getCurrentPick();
                      this.showModal = false;
                      this.Schedules = null;
                      this.getSchedules(
                        this.selectedClubId,
                        this.weekNumber,
                        this.CurrentWeek
                      );
                      this.getPickReportWithLogos();
                      Swal.fire("Success", res.message, "success");
                    } else {
                      Swal.fire("Failed", res.message, "error");
                    }
                    this.config.stopLoader();
                  },
                  err => {
                    this.config.stopLoader();
                    throw new Error(err);
                  }
                );
              } else {
                this.config.stopLoader();
              }
            });
          }
        },
        err => {
          this.config.stopLoader();
          throw new Error(err);
        }
      );
    }
    //this.showModal = true;
  }

  // ConfirmPick() {
  //   let data = {
  //     entryID: this.Entry.entryID,
  //     scheduleID: this.currentSchedule.scheduleID,
  //     yourWinner: this.winnerTeam,
  //     weekNumber: this.weekNumber,
  //     entryName: this.Entry.entryName
  //   };
  //   ////console.log("Current Schedule ID-", this.currentSchedule.scheduleID);
  //   ////console.log("Current WeekNumber-", this.weekNumber);
  //   this.config.startLoader();
  //   this.api.makePick(data).subscribe(
  //     res => {
  //       if (res.status == "1") {
  //         //console.log(JSON.stringify(res));
  //         Swal.fire("Success", res.message, "success");
  //         this.showModal = false;
  //         this.router.navigateByUrl("/ticket/" + this.Entry.poolID);
  //         this.getEntry();
  //         this.getCurrentPick();
  //         this.getPickReportWithLogos();
  //       }
  //       else {
  //         Swal.fire("Error", res.message, "error");
  //       }
  //       this.config.stopLoader();
  //     },
  //     err => {
  //       this.config.stopLoader();
  //       throw new Error(err);
  //     }
  //   );
  // }
  // cancelAction() {
  //   this.showModal = false;
  // }

  setMyStyles(schedule, winner, IsHome) {
    if (this.CurrentPick == null) {
      return "default-team";
    }

    if (
      this.CurrentPick != null &&
      this.CurrentPick.scheduleID == schedule.scheduleID &&
      this.CurrentPick.winner == winner
    ) {
      return "picked-team";
    } else {
      return "not-picked-team";
    }
  }
  //Get Current pciked shcedules
  GetCurrentPickScheduleFromList(scheduleList, scheduleId) {
    this.GetWeekCloseStatusFromList(scheduleList);
    if (
      typeof scheduleList != undefined &&
      scheduleList != null &&
      scheduleList.length > 0
    ) {
      scheduleList.forEach(groupDate => {
        if (
          typeof groupDate != undefined &&
          groupDate != null &&
          groupDate.scheduleGroupTime.length > 0
        ) {
          groupDate.scheduleGroupTime.forEach(groupTime => {
            if (
              typeof groupTime != undefined &&
              groupTime != null &&
              groupTime.scheduleWeekLists.length > 0
            ) {
              groupTime.scheduleWeekLists.forEach(schedule => {
                if (typeof groupTime != undefined) {
                  // //console.log('scheduleID--',schedule.scheduleID,scheduleId)
                  if (schedule.scheduleID == scheduleId) {
                    //console.log('GetCurrentPickScheduleFromList', schedule);
                    this.currentPickedSchedule = schedule;
                  }
                }
              });
            }
          });
        }
      });
    }
  }

  //Find week status
  GetWeekCloseStatusFromList(scheduleList) {
    this.WeekClose = this.CurrentWeek.start;

    // if (typeof scheduleList != undefined && scheduleList != null && scheduleList.length > 0) {

    //   scheduleList.forEach(groupDate => {
    //     if (typeof groupDate != undefined && groupDate != null && groupDate.scheduleGroupTime.length > 0) {

    //       groupDate.scheduleGroupTime.forEach(groupTime => {

    //         if (typeof groupTime != undefined && groupTime != null && groupTime.scheduleWeekLists.length > 0) {

    //           groupTime.scheduleWeekLists.forEach(schedule => {
    //             if (typeof groupTime != undefined) {
    //              // debugger;
    //                //console.log('schedule.cutOff--',new Date(schedule.cutOff) );
    //                //console.log('new Date()--',new Date());
    //               if (new Date(schedule.cutOff) >= new Date()) {
    //                  = true;
    //               }
    //               else {
    //                 this.WeekOpen = false;
    //               }
    //               return null;
    //             }
    //           });
    //         }
    //       });
    //     }
    //   });
    // }
  }
  //Callback after Confirm Pick
  ResetPickcenterLayoutForWeek() {
    if (this.currentPickedSchedule != null && !this.MakeNewPick) {
      this.ShowCurrentPick = true;
    } else {
      this.ShowCurrentPick = false;
    }
  }
  //ChangePick()
  ChangePick() {
    this.MakeNewPick = true;
    this.ResetPickcenterLayoutForWeek();
  }
  FilterCurrentPick() {
    if (
      typeof this.CurrentPickList != undefined &&
      this.CurrentPickList != null &&
      this.CurrentPickList.length > 0
    ) {
      this.CurrentPickList.forEach(pick => {
        if (pick.weekNumber == this.weekNumber) {
          this.CurrentPick = pick;
        }
      });
    }
  }
  logoWeek(weekNo) {
    if (
      typeof this.LogoWeeks != undefined &&
      this.LogoWeeks != null &&
      this.LogoWeeks.length > 0
    ) {
      this.LogoWeeks.forEach(element => {
        //  //console.log('element--',element);
        if (element.weekNumber == weekNo) {
          ////console.log('week--',weekNo);
          return '"' + element.logoImageSrc + '"';
        }
      });
      return "NoPick";
    }
  }
  backToTIckets(Id) {
    this.router.navigateByUrl("/ticket/" + Id);
  }
  LoadPool() {
    this.apiClub.GetPoolByID(this.selectedClubId).subscribe(res => {
      this.ClubDetail = res.maintaince;
      this.PooolID = this.ClubDetail.pool_ID;
      this.getCurrentPick();
      this.getPickReportWithLogos();
    });
  }
}
