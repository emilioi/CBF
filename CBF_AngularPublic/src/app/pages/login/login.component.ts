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
  LoginMessage: string;
  loginForm: FormGroup;
  EntryList: any;
  acceptedRules: any;
  IsPublicRegistrationOpen: any;
  IsMaintenanceOn: any = 'false';
  MaintenanceText: any;
  allsettings: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private api: LoginService,
    private config: Config,
    private auth: AuthGuard
  ) {
    this.loginForm = this.fb.group({
      email_Address: [""],
      password: [""]
    });
  }

  ngOnInit() {
    this.LoadSetting();
    this.LoadMaintenanceSetting();
    localStorage.removeItem("userObj");
    this.LoginMessage = localStorage.getItem("LoginMessage");
    localStorage.setItem("LoginMessage", "");
    localStorage.removeItem("localAlerts");
  }

  signin() {
    if (this.IsMaintenanceOn == 'false') {
      if (this.loginForm.value.email_Address == "" || this.loginForm.value.email_Address == null || this.loginForm.value.email_Address == undefined) {
        Swal.fire("Oops...", "Please enter Email Address!", "error");
      }
      else if (this.loginForm.value.password == "" || this.loginForm.value.password == null || this.loginForm.value.password == undefined) {
        Swal.fire("Oops...", "Please enter Password!", "error");
      }
      else {
        this.config.startLoader();
        this.api.login(this.loginForm.value).subscribe(res => {
          if (res.status == 1) {
            localStorage.setItem("userObj", JSON.stringify(res));
            this.config.updateGlobalKey(res.key);
            console.log('loginkey-' + this.config.loginKey);
            this.auth.updateAuth(true);
            localStorage.setItem("ShowWithoutPickModal", "true");

            //////=====Swal fire for accepting rules
            this.acceptedRules = JSON.parse(localStorage.getItem("userObj")).userInfo.rules_Accepted;
            console.log("Rules Acceptance: " + JSON.parse(localStorage.getItem("userObj")).userInfo.rules_Accepted);
            if (this.acceptedRules !== true) {
              Swal.fire({
                title: 'CLUB BIG FUN Rules',
                // html:
                //   "You won't be able login without accepting our Rules! <br/>" +
                //   " <a target='_blank' href='/rules'>Click here to Read it.</a> ",
                type: 'info',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'View Rules',
                cancelButtonText: 'Cancel'
              }).then((result) => {
                if (result.value) {
                  this.router.navigateByUrl("/rules");
                  this.config.stopLoader();
                }
                else {
                  this.config.stopLoader();
                }
              });
            }
            else {
              this.router.navigateByUrl("/club-house");
            }
          }
          else {
            this.config.stopLoader();
            console.log("login error");
            Swal.fire("Oops...", res.message, "error");
          }
        });
      }
    }
    else
    {
      Swal.fire("Failed", "Maintenance mode is on.", "error");
    }
  }
  LoadSetting() {
    this.api.GetRegistrationSetting().subscribe(
      res => {
        if (res.status == 1) {
          this.IsPublicRegistrationOpen = res.settings[0].lookup_Value == "true" ? true : false;
        } else {
          this.IsPublicRegistrationOpen = "false";
        }
      },
      err => {
        throw new Error(err);
      }
    );
  }

  LoadMaintenanceSetting() {
    this.api.GetMaintenanceSetting().subscribe(
      res => {
        if (res.status == 1) {
          console.log(res);
          this.allsettings = res.settings;
          var Check_lookup = false;
          var Text_lookup = '';
          this.allsettings.forEach(lokupSetting => {
            if (lokupSetting.lookup_Name == 'MaintenanceText') {
              console.log("Lookup2 " + lokupSetting.lookup_Value);
              Text_lookup = lokupSetting.lookup_Value;
            }
            if (lokupSetting.lookup_Name == 'MaintenanceOn') {
              console.log("Lookup " + lokupSetting.lookup_Value);
              Check_lookup = lokupSetting.lookup_Value;
            }
          });
          this.IsMaintenanceOn = Check_lookup;
          this.MaintenanceText = Text_lookup
        } else {
          this.IsMaintenanceOn = "false";
          this.MaintenanceText = ""
        }
      },
      err => {
        throw new Error(err);
      }
    );
  }
}
