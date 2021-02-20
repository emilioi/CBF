import { Component, OnInit } from "@angular/core";
//import { NgForm } from "@angular/forms";
import { Router } from "@angular/router";
import { FormGroup, FormBuilder } from "@angular/forms";
import { SignupService } from "./signup.service";
import Swal from "sweetalert2";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-signup",
  templateUrl: "./signup.component.html",
  styleUrls: ["./signup.component.scss"]
})
export class SignupComponent implements OnInit {
  signupForm: FormGroup;
  showModal: boolean;
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private config: Config,
    private api: SignupService
  ) {
    this.signupForm = this.fb.group({
      reference: [""],
      email_Address: [""],
      phone_Number: [""],
      first_Name: [""],
      last_Name: [""],
      //password: [""],
      //confirmPassword: [""]
    });
  }

  ngOnInit() { }

  signup() {
    if (this.signupForm.value.reference == "" || this.signupForm.value.reference == null || typeof this.signupForm.value.reference == "undefined") {
      Swal.fire("Oops...", "Please enter referral name!", "error");
    }
    else
      if (this.signupForm.value.email_Address == "" || this.signupForm.value.email_Address == null || typeof this.signupForm.value == "undefined") {
        Swal.fire("Oops...", "Please enter email address!", "error");
      }
      else
        if (this.signupForm.value.phone_Number == "" || this.signupForm.value.phone_Number == null || typeof this.signupForm.value.phone_Number == "undefined") {
          Swal.fire("Oops...", "Please enter phone number!", "error");
        }
        else
          if (this.signupForm.value.first_Name == "" || this.signupForm.value.first_Name == null || typeof this.signupForm.value.first_Name == "undefined") {
            Swal.fire("Oops...", "Please enter first name!", "error");
          }
          else
            if (this.signupForm.value.last_Name == "" || this.signupForm.value.last_Name == null || typeof this.signupForm.value.last_Name == "undefined") {
              Swal.fire("Oops...", "Please enter last name!", "error");
            }
            else {
              console.log("signin click", this.signupForm.value);
              this.config.startLoader();
              this.api.register(this.signupForm.value).subscribe(res => {
                
                console.log(res);
                if (res.status == 1) {
                  this.showModal = true;
                  this.config.stopLoader();
                } else {
                  this.config.stopLoader();
                  console.log("login error");
                  Swal.fire("Oops...", res.message, "error");
                }
              });
            }
  }
  cancelAction() {
    this.showModal = false;
    this.router.navigateByUrl('/login');
  }
}
