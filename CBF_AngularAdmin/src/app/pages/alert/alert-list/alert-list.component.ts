import { Component, OnInit } from "@angular/core";
import { Config } from "src/app/utility/config";
import { Router } from "@angular/router";
import Swal from "sweetalert2";
import { MemberService } from "../../Member/member.service";

@Component({
  selector: "app-alert-list",
  templateUrl: "./alert-list.component.html",
  styleUrls: ["./alert-list.component.scss"]
})
export class AlertListComponent implements OnInit {
  alerts: any;

  ShowMailingList: boolean = false;
  MailingList: any;
  constructor(
    private router: Router,
    private api: MemberService,
    private config: Config
  ) {}

  ngOnInit() {
    this.loadAlertList();
  }

  async loadAlertList() {
    this.config.startLoader();
    this.api.GetAlertList().subscribe(
      res => {
        this.alerts = res.member_Alerts;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  delete(ID) {
    Swal.fire({
      title: "Are you sure want to delete?",
      //text: "You won't be able to revert this!",
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes",
      cancelButtonText: "No"
    }).then(result => {
      if (result.value) {
        this.api.deleteAlert(ID).subscribe(res => {
          Swal.fire("Success", res.message, "success");
          this.loadAlertList();
        });
      }
    });
  }

  gotoEdit(ID) {
    this.router.navigate(["edit-alert/" + ID]);
  }
}
