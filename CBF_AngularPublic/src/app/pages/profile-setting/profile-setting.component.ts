import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { ProfileSettingService } from "./profile-setting.service";
import Swal from "sweetalert2";

@Component({
  selector: "app-profile-setting",
  templateUrl: "./profile-setting.component.html",
  styleUrls: ["./profile-setting.component.scss"]
})
export class ProfileSettingComponent implements OnInit {
  resetPwdFrm: FormGroup;

  constructor(private fb: FormBuilder, private api: ProfileSettingService) {
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

  ngOnInit() {}

  updatePwd() {
    console.log("pwd update", this.resetPwdFrm.value);

    this.api.resetPwd(this.resetPwdFrm.value).subscribe(res => {
      console.log("reset pwd res", res);
      Swal.fire("Success", res.message, "success");
    });
  }
}
