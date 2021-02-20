import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { Router } from "@angular/router";
import { Config } from "src/app/utility/config";
import Swal from "sweetalert2";
import { MemberService } from "../../Member/member.service";

@Component({
  selector: "app-add-alert",
  templateUrl: "./add-alert.component.html",
  styleUrls: ["./add-alert.component.scss"]
})
export class AddAlertComponent implements OnInit {
  alertForm: FormGroup;
  color : any;
  constructor(
    private api: MemberService,
    private router: Router,
    private config: Config,
    private fb: FormBuilder
  ) {
    this.alertForm = this.fb.group({
      alert_Name: new FormControl(),
      alert_Description: new FormControl(),
      reminderStart: new FormControl(Date),
      reminderEnd: new FormControl(Date),
      is_AfterLogin: new FormControl(false),
      one_TimeReminder: new FormControl(false),
      isExpired: new FormControl(),
      alertColor: new FormControl(this.color)
    });
  }

  ngOnInit() { }
  SaveAlert() {
    if (
      this.alertForm.value.alert_Name == "" ||
      this.alertForm.value.alert_Name == null ||
      this.alertForm.value.alert_Name == undefined
    ) {
      this.config.stopLoader();
      Swal.fire("Oops...", "Please enter alert name.", "error");
      return;
    }
    if (
      this.alertForm.value.alert_Description == "" ||
      this.alertForm.value.alert_Description == null ||
      this.alertForm.value.alert_Description == undefined
    ) {
      this.config.stopLoader();
      Swal.fire("Oops...", "Please enter description.", "error");
      return;
    }
    else {
      this.config.startLoader();
      this.alertForm.value.alertColor = this.color;
      console.log(this.alertForm.value);
      this.api.saveAlert(this.alertForm.value).subscribe(res => {
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
          this.router.navigateByUrl("/alert-list");
        } else {
          Swal.fire("Failed", res.message, "error");
        }
        this.config.stopLoader();
      });
    }
  }
  MyColor(val) {
   this.color = val;
  }
}
