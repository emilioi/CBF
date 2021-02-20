import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ForgotPasswordService } from '../forgot-password/forgot-password.service';
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPwdFrm: FormGroup;
  showModal: boolean;
  message: any;
  memberId: any;
  code: any;
  constructor(private router: Router,
    private fb: FormBuilder,
    private api: ForgotPasswordService,
    private route: ActivatedRoute,
    private config: Config,
  ) { 
    this.memberId = this.route.snapshot.queryParamMap.get("member_Id");
    this.code = this.route.snapshot.queryParamMap.get("code");


    this.resetPwdFrm = this.fb.group({
      password: [""],
      confirmPassword: [""],
      member_Id: this.memberId,
      old_Password: this.code,
      email_Address: [""]
    });

  }

  ngOnInit() {
    // console.log("MemberID=> " + this.memberId);
    // console.log("Code=> " + this.code);
  }
  resetPwd() {
    if (this.resetPwdFrm.value.password == "" || this.resetPwdFrm.value.password == null || this.resetPwdFrm.value.password == undefined) {
      Swal.fire("Oops...", "Please enter Password!", "error");
    }
    // else
    // if (this.resetPwdFrm.value.confirmPassword == "" || this.resetPwdFrm.value.confirmPassword == null || this.resetPwdFrm.value.confirmPassword == undefined) {
    //   Swal.fire("Oops...", "Please enter Confirm Password!", "error");
    // }
    else
      if (this.resetPwdFrm.value.password !== this.resetPwdFrm.value.confirmPassword) {
        Swal.fire("Oops...", "Confirm Password not matched!", "error");
      }
      else {
        //if (this.resetPwdFrm.value.email_Address.length > 3) {
          this.config.startLoader();
        this.api.resetPassword(this.resetPwdFrm.value).subscribe(res => {
          if (res.status == 1) {
            //this.showModal = true;
            this.message = res.message;
            console.log("res==> "+res);
            Swal.fire("Success", res.message, "success");
            this.config.stopLoader();
            this.router.navigateByUrl("/login");
          } else {
            this.config.stopLoader();
            Swal.fire("Oops...", res.message, "error");
          }
        });
        //}
      }
  }
  // cancelAction() {
  //   this.showModal = false;
  //   this.router.navigateByUrl('/login');
  // }
}
