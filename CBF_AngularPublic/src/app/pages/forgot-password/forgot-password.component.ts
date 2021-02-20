import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { ForgotPasswordService } from "./forgot-password.service";
import Swal from "sweetalert2";
import { Router } from '@angular/router';
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-forgot-password",
  templateUrl: "./forgot-password.component.html",
  styleUrls: ["./forgot-password.component.scss"]
})
export class ForgotPasswordComponent implements OnInit {
  forgotPwdFrm: FormGroup;
  showModal: boolean;
  message: any;
  constructor(private fb: FormBuilder,
    private api: ForgotPasswordService,
    private router: Router,
    private config: Config,

  ) {
    this.forgotPwdFrm = this.fb.group({
      email_Address: [""]
    });
  }

  ngOnInit() {
    //this.show = true;
  }

  forgotPwd() {
    
    if (this.forgotPwdFrm.value.email_Address == "" || this.forgotPwdFrm.value.email_Address == null || this.forgotPwdFrm.value.email_Address == undefined) {
      Swal.fire("Oops...", "Please enter Email Address!", "error");
    }
    else {
      if (this.forgotPwdFrm.value.email_Address.length > 3) {
        this.config.startLoader();
        this.api
          .forgotPwd(this.forgotPwdFrm.value.email_Address)
          .subscribe(res => {
            
            if (res.status == 1) {
              this.showModal = true;
              this.message = res.message;
              this.config.stopLoader();
            } else {
              this.config.stopLoader();
              Swal.fire("Oops...", res.message, "error");
            }
          });
      }
    }
  }
  cancelAction() {
    this.showModal = false;
    this.router.navigateByUrl('/login');

  }
}
