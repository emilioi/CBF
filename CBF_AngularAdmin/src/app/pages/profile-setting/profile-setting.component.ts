import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { ProfileSettingService } from "./profile-setting.service";
import Swal from "sweetalert2";
import { Router } from '@angular/router';
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-profile-setting",
  templateUrl: "./profile-setting.component.html",
  styleUrls: ["./profile-setting.component.scss"]
})
export class ProfileSettingComponent implements OnInit {
  resetPwdFrm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private api: ProfileSettingService,
    private router: Router,
    private loaderConfig: Config
      ) {
    let email = JSON.parse(localStorage.getItem("userObj")).userInfo
      .email_Address;
    let memberid = JSON.parse(localStorage.getItem("userObj")).member_Id;

    // console.log(email);
    //console.log(memberid);

    this.resetPwdFrm = this.fb.group({
      email_Address: [email],
      password: [""],
      confirmPassword: [""],
      old_Password: [""],
      member_Id: [memberid]
    });
  }

  ngOnInit() { }

  updatePwd() {
    if (this.resetPwdFrm.value.email_Address == null || this.resetPwdFrm.value.email_Address == "") {
      Swal.fire("Oops...", "Please enter email address.", "error");
      return;
    }
    if (this.resetPwdFrm.value.old_Password == null || this.resetPwdFrm.value.old_Password == "") {
      Swal.fire("Oops...", "Please enter old password.", "error");
      return;
    }
    if (this.resetPwdFrm.value.password == null || this.resetPwdFrm.value.password == "") {
      Swal.fire("Oops...", "Please enter password.", "error");
      return;
    }
    if (this.resetPwdFrm.value.confirmPassword == null || this.resetPwdFrm.value.confirmPassword == "") {
      Swal.fire("Oops...", "Please enter confirm password.", "error");
      return;
    }
    if (this.resetPwdFrm.value.password !== this.resetPwdFrm.value.confirmPassword) {
      Swal.fire("Oops...", "Password and confirm password should be same.", "error");
      return;
    }
    this.loaderConfig.startLoader();
    this.api.resetPwd(this.resetPwdFrm.value).subscribe(res => {
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success");
        this.loaderConfig.stopLoader(); 
        this.router.navigateByUrl("/dashboard");
      }
      else {
        Swal.fire("Failed", res.message, "error");
        this.loaderConfig.stopLoader();
      }

    });
  }
}
