import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { Config } from "src/app/utility/config";
import Swal from "sweetalert2";
import { MemberService } from "../../Member/member.service";
import { DatePipe } from "@angular/common";

@Component({
  selector: "app-edit-alert",
  templateUrl: "./edit-alert.component.html",
  styleUrls: ["./edit-alert.component.scss"]
})
export class EditAlertComponent implements OnInit {
  alertForm: FormGroup;
  Id: any;
  color : any;
  constructor(
    private api: MemberService,
    private router: Router,
    private config: Config,
    private fb: FormBuilder,
    private ID: ActivatedRoute,
    private datePipe: DatePipe
  ) {
    this.alertForm = this.fb.group({
      alert_Id: [],
      alert_Name: new FormControl(),
      alert_Description: new FormControl(),
      reminderStart: new FormControl(Date),
      reminderEnd: new FormControl(Date),
      is_AfterLogin: new FormControl(false),
      one_TimeReminder: new FormControl(false),
      isExpired: new FormControl(),
      alertColor: this.color,
      
    });
    this.Id = this.ID.snapshot.paramMap.get("Id");
  }

  ngOnInit() {
    this.loadalerts(this.Id);
    console.log("ID: " + this.Id);
  }
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
    }else {
      this.config.startLoader();
      this.alertForm.value.alertColor = this.color;
      console.log(this.alertForm.value);
      console.log("Color: "+ this.alertForm.value)
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

  async loadalerts(Id) {
    this.config.startLoader();
    this.api.getById(Id).subscribe(
      res => {
        this.color = res.member_Alerts.alertColor,
        this.alertForm.setValue({
          alert_Id: res.member_Alerts.alert_Id,
          alert_Name: res.member_Alerts.alert_Name,
          alert_Description: res.member_Alerts.alert_Description,
          reminderStart: res.member_Alerts.reminderStart,
          reminderEnd: res.member_Alerts.reminderEnd,
          is_AfterLogin: res.member_Alerts.is_AfterLogin,
          one_TimeReminder: res.member_Alerts.one_TimeReminder,
          isExpired: res.member_Alerts.isExpired,
          alertColor: res.member_Alerts.alertColor,
          
        });
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  MyColor(val) {
    this.color = val;
    console.log(this.color);
   }
}
