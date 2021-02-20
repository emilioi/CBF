import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { MoveTicketService } from "./move-ticket.service";
import Swal from "sweetalert2";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { Config } from 'src/app/utility/config';
import { MemberService } from '../Member/member.service';
import { EntryService } from '../Entry/entry.service';

@Component({
  selector: "app-move-ticket",
  templateUrl: "./move-ticket.component.html",
  styleUrls: ["./move-ticket.component.scss"]
})
export class MoveTicketComponent implements OnInit {
  poolList;
  moveTktFrom: FormGroup;
  EntryList: any;
  memberList: any;
  newOwner: any;
  currentOwner: any;
  selectedEntriesToAdd: any;
  currentPoolId: any;
  constructor(
    private fb: FormBuilder,
    private api: MoveTicketService,
    private config: Config,
    private memberAPI: MemberService,
    private Entryapi: EntryService
  ) {
    this.moveTktFrom = this.fb.group({
      SelectedPool: [""],
      SelectTicket: [""],
      SelectMember: [""]
    });
  }
  ngOnInit() {
    this.getPoolDD();
    //this.getAllMember();
  }
  getPoolDD() {
    this.config.startLoader();
    this.api.getPoolDD().subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.poolList = res.pool_Master;
        } else {
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  GetTicketByPoolId() {
    this.config.startLoader();
    this.currentPoolId = this.moveTktFrom.value.SelectedPool;
    this.api.GetTicketandOwnerByPoolId(this.moveTktFrom.value.SelectedPool).subscribe(
      res => {
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        console.log(err);
      }
    );
  }


  getAllMember(name) {
    this.config.startLoader();
    this.memberAPI.GetMemberBySearch(name).subscribe(
      res => {
        this.config.stopLoader();
        this.memberList = res.users;;
        this.cancelTicket();
      },
      err => {
        this.config.stopLoader();
        console.log(err);
      }
    );
  }

  getTicketbyPool(entry) {
    if (this.moveTktFrom.value.SelectedPool == "" || this.moveTktFrom.value.SelectedPool == null || this.moveTktFrom.value.SelectedPool == undefined) {
      Swal.fire("Oops...", "Please select a Pool!", "error");
      return;
    }
    this.config.startLoader();
    this.Entryapi.SearchEntry(entry, this.currentPoolId).subscribe(res => {
      this.EntryList = res.entryWeekLists;
      //console.log("Current Owner: "+ JSON.stringify(this.EntryList.memberID));

      this.cancelTicket();
      this.config.stopLoader();
    });

  }
  onChangeTicket(obj) {
    this.currentOwner = obj;
  }

  onChangeMember(obj) {
    this.newOwner = obj;
  }

  moveTicket() {
    if (this.moveTktFrom.value.SelectedPool == "" || this.moveTktFrom.value.SelectedPool == null || this.moveTktFrom.value.SelectedPool == undefined) {
      Swal.fire("Oops...", "Please select a Pool!", "error");
    }
    else
      if (this.moveTktFrom.value.SelectTicket == "" || this.moveTktFrom.value.SelectTicket == null || this.moveTktFrom.value.SelectTicket == undefined) {
        Swal.fire("Oops...", "Please select a Ticket Name!", "error");
      }
      else
        if (this.moveTktFrom.value.SelectMember == "" || this.moveTktFrom.value.SelectMember == null || this.moveTktFrom.value.SelectMember == undefined) {
          Swal.fire("Oops...", "Please select a Member!", "error");
        }
        else {
          let data = {
            EntryId: this.currentOwner.entryID,
            NewOwnerId: this.newOwner.member_Id
          };
          console.log(this.EntryList.entryID);
          Swal.fire({
            title: 'Are you sure want to move this ticket?',
            //text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
          }).then((result) => {
            if (result.value) {
              this.config.startLoader();
              this.api.TransferTickets(data).subscribe(
                res => {
                  this.config.stopLoader();
                  if (res.status == 1) {
                    Swal.fire("Success", res.message, "success");
                    //this.GetTicketByPoolId()
                    //this.getAllMember();
                    this.onChangeTicket(this.currentOwner);
                    this.onChangeMember(this.newOwner)
                  }
                  else {
                    Swal.fire("Failed", res.message, "error");
                  }
                },
                err => {
                  this.config.stopLoader();
                  console.log(err);
                  throw new Error(err);
                }
              );
            }
          })
        }
  }

  // moveTicket() {
  //   if (this.moveTktFrom.value.SelectedPool == "" || this.moveTktFrom.value.SelectedPool == null || this.moveTktFrom.value.SelectedPool == undefined) {
  //     Swal.fire("Oops...", "Please select a Pool!", "error");
  //   }
  //   else
  //     if (this.moveTktFrom.value.SelectTicket == "" || this.moveTktFrom.value.SelectTicket == null || this.moveTktFrom.value.SelectTicket == undefined) {
  //       Swal.fire("Oops...", "Please select a Ticket Name!", "error");
  //     }
  //     else
  //       if (this.moveTktFrom.value.SelectMember == "" || this.moveTktFrom.value.SelectMember == null || this.moveTktFrom.value.SelectMember == undefined) {
  //         Swal.fire("Oops...", "Please select a Member!", "error");
  //       }
  //       else {
  //         let data = this.selectedEntriesToAdd

  //         console.log(this.EntryList.entryID);
  //         Swal.fire({
  //           title: 'Are you sure want to move this ticket?',
  //           //text: "You won't be able to revert this!",
  //           type: 'warning',
  //           showCancelButton: true,
  //           confirmButtonColor: '#3085d6',
  //           cancelButtonColor: '#d33',
  //           confirmButtonText: 'Yes',
  //           cancelButtonText: 'No',
  //         }).then((result) => {
  //           if (result.value) {
  //             this.config.startLoader();
  //             this.api.TransferMultipleTickets(this.EntryList.memberID, this.newOwner.member_Id, data).subscribe(
  //               res => {
  //                 this.config.stopLoader();
  //                 if (res.status == 1) {
  //                   Swal.fire("Success", res.message, "success");
  //                   //this.GetTicketByPoolId()
  //                   //this.getAllMember();
  //                   this.onChangeTicket(this.currentOwner);
  //                   this.onChangeMember(this.newOwner)
  //                 }
  //                 else {
  //                   Swal.fire("Failed", res.message, "error");
  //                 }
  //               },
  //               err => {
  //                 this.config.stopLoader();
  //                 console.log(err);
  //                 throw new Error(err);
  //               }
  //             );
  //           }
  //         })
  //       }
  // }

  cancelTicket() {
    this.moveTktFrom.reset();
    this.moveTktFrom.setValue({
      SelectTicket: false,
      SelectMember: false,
      SelectedPool: this.currentPoolId
    });
    this.currentOwner = "";
    this.newOwner = "";
  }

  changeAllMemberSelection(event) {
    this.selectedEntriesToAdd = [];
    for (var i in event.target.selectedOptions) {
      if (event.target.selectedOptions[i].label) {
        this.selectedEntriesToAdd.push(event.target.selectedOptions[i].value);
      }
    }
    console.log(this.selectedEntriesToAdd);
  }

}
