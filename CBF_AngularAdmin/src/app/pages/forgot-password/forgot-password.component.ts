import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { ForgotPasswordService } from "./forgot-password.service";
import Swal from "sweetalert2";

@Component({
  selector: "app-forgot-password",
  templateUrl: "./forgot-password.component.html",
  styleUrls: ["./forgot-password.component.scss"]
})
export class ForgotPasswordComponent implements OnInit {
  forgotPwdFrm: FormGroup;

  constructor(private fb: FormBuilder, private api: ForgotPasswordService) {
    this.forgotPwdFrm = this.fb.group({
      email_Address: [""]
    });
  }

  ngOnInit() {}

  forgotPwd() {
    //console.log("forgot pwd", this.forgotPwdFrm.value);
if(this.forgotPwdFrm.value.email_Address=="" || this.forgotPwdFrm.value.email_Address== null || this.forgotPwdFrm.value.email_Address== undefined)
{
  Swal.fire("Oops...", "Please enter Email Address", "error")
}
    if (this.forgotPwdFrm.value.email_Address.length > 3) {
      this.api
        .forgotPwd(this.forgotPwdFrm.value.email_Address)
        .subscribe(res => {
          if (res.status == 1) {
            Swal.fire("Success", res.message, "success");
          } else {
            //console.log("something went worng");
            Swal.fire("Oops...", res.message, "error");
          }
        });
    }
  }
}
