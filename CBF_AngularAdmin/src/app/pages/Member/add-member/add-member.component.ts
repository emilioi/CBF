import { Component, OnInit, ElementRef } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { MemberService } from "../member.service";
import Swal from "sweetalert2";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { RootService } from "src/app/root.service";
import { Config } from "src/app/utility/config";
import * as $ from 'jquery';
@Component({
  selector: "app-add-member",
  templateUrl: "./add-member.component.html",
  styleUrls: ["./add-member.component.scss"]
})
export class AddMemberComponent implements OnInit {
  memberReg: FormGroup;
  userInfo: any;
  memberlist: any;
  submitted: boolean;
  isUserNameExist: boolean;
  isEmailExist: boolean;
  state: any;
  show: boolean;
  countries: any;
  isEditableMode: boolean;
  showPwdInputBtn: boolean;
  CancelEditableMode: boolean;
  imageSrc: string = "";
  imageExention: string;
  currentMemberProfile: any;
  CountryId: any;
  reference:any;
  adminType: string;
  private _chrome = navigator.userAgent.indexOf("Chrome") > -1;
  constructor(
    private fb: FormBuilder,
    private api: MemberService,
    private route: ActivatedRoute,
    private config: Config,
    private router: Router,
    private _el: ElementRef
  ) {
    this.submitted = false;
    this.isEditableMode = false;
    this.showPwdInputBtn = false;

    this.memberReg = this.fb.group({
      member_Id: [0],
      login_Id: [""],
      password: [""],
      first_Name: [""],
      last_Name: [""],
      phone_Number: [""],
      user_Type: [""],
      email_Address: [""],
      address_Line1: [""],
      address_Line2: [""],
      confirm_password: [""],
      city: [""],
      state: [""],
      country: [38],
      zip_Code: [""],
      fax_Number: [""],
      business_Phone: [""],
      gender: [""],
      image_Url: [""],
      image_Name: [""],
      last_Login: ["2019-07-23T15:00:34.486Z"],
      failed_Attempt: [0],
      last_Failed_Login: ["2019-07-23T15:00:34.486Z"],
      is_Email_Verified: [true],
      is_Temp_Password: [true],
      is_Locked: [true],
      is_Active: [true],
      is_Deleted: [true],
      last_Edited_By: [0],
      dts: ["2019-07-23T15:00:34.486Z"],
      reference: [""],
    });

    this.route.queryParams.subscribe(params => {
      if (this.router.getCurrentNavigation().extras.queryParams) {
        this.userInfo = this.router.getCurrentNavigation().extras.queryParams.userInfo;
        //console.log("I found this info from last page", this.userInfo);
        this.memberReg.patchValue({
          member_Id: this.userInfo.member_Id,
          login_Id: this.userInfo.login_Id,
          password: this.userInfo.password,
          confirm_password: this.userInfo.password,
          first_Name: this.userInfo.first_Name,
          last_Name: this.userInfo.last_Name,
          phone_Number: this.userInfo.phone_Number,
          user_Type: this.userInfo.admin_Type,
          email_Address: this.userInfo.email_Address,
          image_Url: this.userInfo.image_Url,
          zip_Code: this.userInfo.zip_Code,
          fax_Number: this.userInfo.fax_Number,
          business_Phone: this.userInfo.business_Phone,
          gender: this.userInfo.gender,
          address_Line1: this.userInfo.address_Line1,
          city: this.userInfo.city,
          country: this.userInfo.country,
          state: this.userInfo.state,
          reference: this.userInfo.reference
        });
        this.isEditableMode = true;
        this.currentMemberProfile = this.userInfo.image_Url;
      }
    });
  }

  ngOnInit() {
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
    //this.getCountry();
    this.getstates(this.userInfo.country);
    this.CancelEditableMode = true
    $(document).ready(function () {
      $('.pwd.glyphicon').on('click', function () {
        //  $(this).toggleClass('glyphicon-eye-close').toggleClass('glyphicon-eye-open'); // toggle our classes for the eye icon
        if ($('.password').attr('type') == 'password') {
          $('.password').attr('type', 'text');
        }
        else {
          $('.password').attr('type', 'password');
        }
      });
    });

  }
  PermissionCheck() {
   debugger
    if (this.adminType === "Admin"  || this.adminType === "GroupAdmin") {
      return false;
    } else {
      return true;
    }
  }

  logoUpload(adminId) {
    let data = {
      base64image: this.imageSrc,
      fileExtention: this.imageExention
    };
    this.api.upLoadBase64(adminId, data).subscribe(
      res => {
        //console.log("this is upload res", res);
      },
      err => {
        //this.ngxService.stop();
        throw new Error(err);
      }
    );
  }

  handleInputChange(e) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      alert("invalid format");
      return;
    }
    reader.onload = this._handleReaderLoaded.bind(this);
    reader.readAsDataURL(file);
    this.imageExention = file.name.substring(
      file.name.length - 3,
      file.name.length
    );
  }
  _handleReaderLoaded(e) {
    let reader = e.target;
    this.imageSrc = reader.result;
    Swal.fire('Profile pciture is temporarily saved, Please submit it from the bottom.');
    //console.log(this.imageSrc);
    //console.log(this.imageExention);
  }

  get findInvalidC() {
    return this.memberReg.controls;
  }

  createMember() {
    this.submitted = true;
    if (
      this.memberReg.value.reference == "" ||
      this.memberReg.value.reference == null ||
      this.memberReg.value.reference == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Referral Name", "error");
    } else if (
      this.memberReg.value.first_Name == "" ||
      this.memberReg.value.first_Name == null ||
      this.memberReg.value.first_Name == undefined
    ) {
      Swal.fire("Oops...", "Please enter the First Name", "error");
    } else if (
      this.memberReg.value.last_Name == "" ||
      this.memberReg.value.last_Name == null ||
      this.memberReg.value.last_Name == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Last Name", "error");
    } else if (
      this.memberReg.value.email_Address == "" ||
      this.memberReg.value.email_Address == null ||
      this.memberReg.value.email_Address == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Email Address", "error");
    }
    //  else if (
    //   this.memberReg.value.login_Id == "" ||
    //   this.memberReg.value.login_Id == null ||
    //   this.memberReg.value.login_Id == undefined
    // ) {
    //   Swal.fire("Oops...", "Please enter the Login Id", "error");
    // } 
    else if (
      this.memberReg.value.phone_Number == "" ||
      this.memberReg.value.phone_Number == null ||
      this.memberReg.value.phone_Number == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Phone Number", "error");
    } else
      if (this.isEditableMode == false && (
        this.memberReg.value.password == "" ||
        this.memberReg.value.password == null ||
        this.memberReg.value.password == undefined)
      ) {
        Swal.fire("oops...", "Please enter the Password", "error");
      } else if (this.isEditableMode == false && (
        this.memberReg.value.confirm_password == "" ||
        this.memberReg.value.confirm_password == null ||
        this.memberReg.value.confirm_password == undefined)
      ) {
        Swal.fire("Oops...", "Please enter the Confirm Password", "error");
      } else if (this.isEditableMode == false && (
        this.memberReg.value.password !== this.memberReg.value.confirm_password)
      ) {
        Swal.fire(
          "Oops...",
          "Password & Confirm Password do not match. Please try again",
          "error"
        );
      }
      else {

        if (this.memberReg.invalid) {
          return;
        } else {
          
          if (this.memberReg.value.login_Id == "" || this.memberReg.value.login_Id == null || this.memberReg.value.login_Id == undefined
          ) {
            this.memberReg.value.login_Id = this.memberReg.value.email_Address;
          } 
          this.api.createUser(this.memberReg.value).subscribe(res => {
            //console.log("This is Start " + res);

            if (res.status == 1) {
              //console.log("This is If ", res);
              Swal.fire("Success", res.message, "success");
              //this.memberReg.reset();
              this.submitted = false;
              this.logoUpload(res.users.member_Id);
              this.router.navigateByUrl("/member-list");
            } else {
              //console.log("This is Else " + res);

              Swal.fire("Failed", res.message, "error");
            }
          });
        }
      }
  }

  checkEmailExist() {
   
  
    if (this.memberReg.value.email_Address.length > 3) {
      this.api
        .checkEmailIdExist(this.memberReg.value.email_Address)
        .subscribe(res => {
          if (res.status == 0) {
            Swal.fire("Oops", res.message, "error");
            this.isEmailExist = true;
            this.config.stopLoader();
          } else {
            this.isEmailExist = false;
            this.config.stopLoader();
          }
        });
    } else {
      
    }
  }

  checkloginidExist() {
    //console.log("TEst Id ");
    if (this.memberReg.value.login_Id.length > 3) {
      this.api
        .checkUserNameExist(this.memberReg.value.login_Id)
        .subscribe(res => {
          if (res.status == 0) {
            Swal.fire("Oops", res.message, "error");
            this.isUserNameExist = true;
            this.config.stopLoader();
          } else {
            this.isUserNameExist = false;
            this.config.stopLoader();
          }
        });
    } else {
      //console.log("INVALID USER TYPE");
    }
  }

  showPwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = false;
    this.isEditableMode = false;
    this.CancelEditableMode = false
    this.config.stopLoader();
  }
  hidePwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = false;
    this.isEditableMode = true;
    this.CancelEditableMode = true;
    this.config.stopLoader();
  }

  onCountryChange(event) {
    this.CountryId = event;
    this.getstates(this.CountryId);
  }

  async getstates(CountryId) {

    this.api.GetStates(CountryId).subscribe(
      res => {
        this.state = JSON.parse(JSON.stringify(res)).states;
        //console.log("States :"  + this.states);
      },
      err => {
        throw new Error(err);
      }
    );
  }


  async getCountry() {
    this.api.GetCountry().subscribe(
      res => {
        this.countries = JSON.parse(JSON.stringify(res)).countries;
        //console.log("Country :" + this.states);
      },
      err => {
        throw new Error(err);
      }
    );
  }

  // async loadUserType() {
  //   this.api.GetAllUser().subscribe(
  //     res => {
  //       // console.log("This is GetAllUser", res);
  //       if (res.status == 1) {
  //         this.memberlist = res.users;
  //       } else {
  //         // console.log("Something went wrong");
  //         this.memberlist = [];
  //       }
  //     },
  //     err => {
  //       throw new Error(err);
  //     }
  //   );
  // }
  password() {
    this.show = !this.show;
  }
}
