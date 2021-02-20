import { Component, OnInit } from "@angular/core";
//import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { FormGroup, FormBuilder } from "@angular/forms";
import { LoginService } from "./login.service";
import Swal from "sweetalert2";
import { Config } from 'src/app/utility/config';
import { AuthGuard } from 'src/app/utility/auth.gaurds';

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"]
})
export class LoginComponent implements OnInit {
  message: string;
  name: string;
  password: string;
  LoginMessage:string;
  loginForm: FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private api: LoginService,
    private config : Config,
    private auth : AuthGuard
  ) {
    this.loginForm = this.fb.group({
      email_Address: [""],
      password: [""]
    });
  }

  ngOnInit() { 
    localStorage.removeItem("userObj");
  this.LoginMessage =   localStorage.getItem("LoginMessage");
  localStorage.setItem("LoginMessage","");
  }

  signin() {
    //this.router.navigateByUrl("/dashboard");
    if (this.loginForm.value.email_Address == "" || this.loginForm.value.email_Address == null || this.loginForm.value.email_Address == undefined) {
      Swal.fire("Oops...", "Please enter User Id", "error");
    }
    else
      if (this.loginForm.value.password == "" || this.loginForm.value.password == null || this.loginForm.value.password == undefined) {
        Swal.fire("Oops...", "Please enter Password", "error");
      }
      else {
        this.config.startLoader();
        this.api.login(this.loginForm.value).subscribe(res => {
          this.config.stopLoader();
          if (res.status == 1) {
            localStorage.setItem("userObj", JSON.stringify(res));
          this.config.updateGlobalKey(res.key);
          console.log('loginkey-'+ this.config.loginKey);
          this.auth.updateAuth(true);
         
            this.router.navigateByUrl("/dashboard");
          } else {
            //console.log("login error");
            Swal.fire("Oops...", res.message, "error");
          }
        });
      }
  }
}
