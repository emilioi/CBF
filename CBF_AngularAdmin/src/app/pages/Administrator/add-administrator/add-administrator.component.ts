import { Component, OnInit, ElementRef } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AdministratorService } from "../administrator.service";
import Swal from "sweetalert2";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { Config } from "src/app/utility/config";

@Component({
  selector: "app-add-administrator",
  templateUrl: "./add-administrator.component.html",
  styleUrls: ["./add-administrator.component.scss"]
})
export class AddAdministratorComponent implements OnInit {
  adminReg: FormGroup;
  userInfo: any;
  Counts: any;
  show: boolean;
  userslist: Array<object>;
  submitted: boolean;
  isUserNameExist: boolean;
  isEmailExist: boolean;
  isEditableMode: boolean;
  showPwdInputBtn: boolean;
  CancelEditableMode: boolean;
  imageSrc: string = "";
  imageExention: string;
  currentProfilePic: any;
  adminType: any;
  private _chrome = navigator.userAgent.indexOf("Chrome") > -1;
  constructor(
    private fb: FormBuilder,
    private api: AdministratorService,
    private router: Router,
    private route: ActivatedRoute,
    private config: Config,
    private _el: ElementRef
  ) {
    this.submitted = false;
    this.isUserNameExist = false;
    this.isEmailExist = false;
    this.isEditableMode = false;
    this.showPwdInputBtn = false;

    this.adminReg = this.fb.group({
      member_Id: [0],
      login_Id: [""],
      password: [""],
      first_Name: [""],
      last_Name: [""],
      phone_Number: [""],
      admin_Type: [""],
      email_Address: [""],
      cnfpassword: [""],
      image_Url: [""],
      last_Login: ["2019-07-22T13:17:34.676Z"],
      failed_Attempt: 0,
      last_Failed_Login: ["2019-07-22T13:17:34.676Z"],
      is_Email_Verified: [true],
      is_Temp_Password: [true],
      is_Locked: [true],
      is_Active: [true],
      is_Deleted: [true],
      last_Edited_By: [0],
      dts: ["2019-07-22T13:17:34.676Z"],
      adminImage: [""]
    });

    this.route.queryParams.subscribe(params => {
      if (this.router.getCurrentNavigation().extras.queryParams) {
        this.userInfo = this.router.getCurrentNavigation().extras.queryParams.userInfo;
        console.log(this.userInfo);
        this.adminReg.patchValue({
          member_Id: this.userInfo.member_Id,
          login_Id: this.userInfo.login_Id,
          password: this.userInfo.password,
          cnfpassword: this.userInfo.password,
          first_Name: this.userInfo.first_Name,
          last_Name: this.userInfo.last_Name,
          phone_Number: this.userInfo.phone_Number,
          admin_Type: this.userInfo.admin_Type,
          email_Address: this.userInfo.email_Address,
          image_Url: this.userInfo.image_Url
        });

        this.isEditableMode = true;
      }
    });
  }

  ngOnInit() {
    this.loadUserType("all");
    this.dashboardCounts();
    if (this._chrome) {
      if (this._el.nativeElement.getAttribute("autocomplete") === "off") {
        setTimeout(() => {
          this._el.nativeElement.setAttribute("autocomplete", "offoff");
        });
      }
    }
    this.CancelEditableMode = true;

    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
  }
  PermissionCheck() {
   
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
    this.config.startLoader();
    this.api.upLoadBase64(adminId, data).subscribe(
      res => {
        this.config.stopLoader();
        //console.log("this is upload res", res);
      },
      err => {
        this.config.stopLoader();
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
    // console.log(this.imageSrc);
    // console.log(this.imageExention);
  }

  get findInvalidC() {
    // console.log(this.adminReg.controls);
    return this.adminReg.controls;
  }

  createAdmin() {
    //this.submitted = true;
    if (
      this.adminReg.value.first_Name == "" ||
      this.adminReg.value.first_Name == null ||
      this.adminReg.value.first_Name == undefined
    ) {
      Swal.fire("Oops...", "Please enter the First Name", "error");
    } else if (
      this.adminReg.value.last_Name == "" ||
      this.adminReg.value.last_Name == null ||
      this.adminReg.value.last_Name == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Last Name", "error");
    } else if (
      this.adminReg.value.login_Id == "" ||
      this.adminReg.value.login_Id == null ||
      this.adminReg.value.login_Id == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Username", "error");
    } else if (
      this.adminReg.value.email_Address == "" ||
      this.adminReg.value.email_Address == null ||
      this.adminReg.value.email_Address == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Email Address", "error");
    }
    // else if (
    //   this.adminReg.value.phone_Number == "" ||
    //   this.adminReg.value.phone_Number == null ||
    //   this.adminReg.value.phone_Number == undefined
    // ) {
    //   Swal.fire("Oops...", "Please enter the Phone Number", "error");
    // }
    else if (
      this.adminReg.value.admin_Type == "" ||
      this.adminReg.value.admin_Type == null ||
      this.adminReg.value.admin_Type == undefined
    ) {
      Swal.fire("Oops...", "Please enter the password", "error");
    } else if (
      this.isEditableMode == false &&
      (this.adminReg.value.password == "" ||
        this.adminReg.value.password == null ||
        this.adminReg.value.password == undefined)
    ) {
      Swal.fire("Oops...", "Please enter the Password", "error");
    } else if (
      this.isEditableMode == false &&
      (this.adminReg.value.cnfpassword == "" ||
        this.adminReg.value.cnfpassword == null ||
        this.adminReg.value.cnfpassword == undefined)
    ) {
      Swal.fire("Oops...", "Please enter the Confirm Password", "error");
    } else if (
      this.isEditableMode == false &&
      this.adminReg.value.password !== this.adminReg.value.cnfpassword
    ) {
      Swal.fire(
        "Oops...",
        "Password & Confirm Password do not match. Please try again",
        "error"
      );
    } else {
      console.log(this.adminReg.value);
      this.config.startLoader();
      this.api.createUser(this.adminReg.value).subscribe(res => {
        if(res.status == 1)
        {
        Swal.fire("Success", res.message, "success");
        this.submitted = false;
        //this.adminReg.reset();
        console.log(res);
        this.logoUpload(res.administrators.member_Id);
        this.config.stopLoader();
        this.router.navigateByUrl("/administrator-list");
        }
        else
        {
        Swal.fire("Failed", res.message, "error");
        }
      });
    }
  }

  checkEmailExist() {
    this.config.startLoader();
    if (this.adminReg.value.email_Address.length > 3) {
      this.api
        .checkEmailId(this.adminReg.value.email_Address)
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
      // console.log("INVALID EMAIL ! IGNORE");
    }
  }

  checkloginidExist() {
    this.config.startLoader();
    if (this.adminReg.value.login_Id.length > 0 && !this.isEditableMode) {
      this.api.checkloginId(this.adminReg.value.login_Id).subscribe(res => {
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
      this.config.stopLoader();
      //console.log("INVALID USER TYPE");
    }
  }

  showPwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = false;
    this.isEditableMode = false;
    this.CancelEditableMode = false;
    this.config.stopLoader();
  }
  hidePwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = false;
    this.isEditableMode = true;
    this.CancelEditableMode = true;
    this.config.stopLoader();
  }
  async loadUserType(userType) {
    this.config.startLoader();
    this.api.GetAllUser(userType).subscribe(
      res => {
        this.userslist = res.administrators;
        this.config.stopLoader();
      },
      err => {
        throw new Error(err);
        this.config.stopLoader();
      }
    );
  }
  loadSuperAdmin() {
    this.loadUserType("SuperAdmin");
    this.config.stopLoader();
  }

  loadGroupAdmin() {
    this.loadUserType("GroupAdmin");
    this.config.stopLoader();
  }

  loadAdmin() {
    this.loadUserType("Admin");
    this.config.stopLoader();
  }
  loadAllAdmin() {
    this.loadUserType("all");
    this.config.stopLoader();
  }

  async dashboardCounts() {
    this.config.startLoader();
    this.api.DasboardInfo().subscribe(
      res => {
        //console.log("This is total count-->>", res);
        this.Counts = res.dashboard;
        this.config.stopLoader();
      },
      err => {
        throw new Error(err);
        this.config.stopLoader();
      }
    );
  }
  password() {
    this.show = !this.show;
  } 
}
