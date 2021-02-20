import { Component, OnInit, ElementRef } from "@angular/core";
import { AdministratorService } from "../administrator.service";
import Swal from "sweetalert2";
import { Config } from "src/app/utility/config";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { formatDate } from '@angular/common';

@Component({
  selector: "app-administrator-details",
  templateUrl: "./administrator-details.component.html",
  styleUrls: ["./administrator-details.component.scss"]
})
export class AdministratorDetailsComponent implements OnInit {
  userslist: Array<object>;
  Counts: any;
  Id: any;
  admin: any;
  cache: any;
  adminReg: FormGroup;
  userInfo: any;
  show: boolean;
  submitted: boolean;
  isUserNameExist: boolean;
  isEmailExist: boolean;
  isEditableMode: boolean;
  showPwdInputBtn: boolean;
  CancelEditableMode: boolean;
  imageSrc: string = "";
  imageExention: string;
  currentProfilePic: any;
  adminType:any;
  IfadminType:string;
  private _chrome = navigator.userAgent.indexOf("Chrome") > -1;
  constructor(
    private config: Config,
    private api: AdministratorService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private _el: ElementRef,

  ) {
    this.Id = this.ID.snapshot.paramMap.get("Id");
    this.show = false;
    this.submitted = false;
    this.isUserNameExist = false;
    this.isEmailExist = false;
    this.isEditableMode = false;
    this.showPwdInputBtn = false;
    this.currentProfilePic = "";

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

    // this.route.queryParams.subscribe(params => {
    //   if (this.router.getCurrentNavigation().extras.queryParams) {
    //     this.userInfo = this.router.getCurrentNavigation().extras.queryParams.userInfo;
    //     console.log(this.userInfo);
    //     this.adminReg.patchValue({
    //       member_Id: this.userInfo.member_Id,
    //       login_Id: this.userInfo.login_Id,
    //       password: this.userInfo.password,
    //       cnfpassword: this.userInfo.password,
    //       first_Name: this.userInfo.first_Name,
    //       last_Name: this.userInfo.last_Name,
    //       phone_Number: this.userInfo.phone_Number,
    //       admin_Type: this.userInfo.admin_Type,
    //       email_Address: this.userInfo.email_Address,
    //       image_Url: this.userInfo.image_Url
    //     });

    //     this.isEditableMode = true;
    //   }
    // });
  }

  // ngOnInit() {
  //   this.loadUserType("all");
  //   this.dashboardCounts();
  //   this.loadAdminById(this.Id);
  // }

  ngOnInit() {
    this.cache = formatDate(new Date(), 'dd-MM-yyyy hh:mm:ss a', 'en-US', '+0530');
    this.loadUserType("all");
    this.dashboardCounts();
    this.loadAdminById(this.Id);

    if (this._chrome) {
      if (this._el.nativeElement.getAttribute("autocomplete") === "off") {
        setTimeout(() => {
          this._el.nativeElement.setAttribute("autocomplete", "offoff");
        });
      }
    }
    this.CancelEditableMode = true;
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.IfadminType = obj.userInfo.admin_Type;
  }
  PermissionCheck() {
    if (this.IfadminType === "Admin"  || this.IfadminType === "GroupAdmin") {
      return false;
    } else {
      return true;
    }
  }

  async loadAdminById(Id) {
    this.config.startLoader();
    this.api.GetAdminById(Id).subscribe(
      res => {
        // console.log("edit " + JSON.stringify(res));
        // console.log("This is GetAdminById", res);
        this.admin = res.administrators;
        this.currentProfilePic = res.administrators.image_Url;
         this.adminType= this.admin.admin_Type;
        // if(res.administrators !== null){
        // this.member_Id = res.users.member_Id,
        //   this.login_Id = res.users.login_Id,
        //   this.password = res.users.password,
        //   this.confirm_password = res.users.password,
        //   this.first_Name = res.users.first_Name,
        //   this.last_Name = res.users.last_Name,
        //   this.phone_Number = res.users.phone_Number,
        //   this.email_Address = res.users.email_Address,
        //   this.image_Url = res.users.image_Url,
        //   this.zip_Code = res.users.zip_Code,
        //   this.fax_Number = res.users.fax_Number,
        //   this.business_Phone = res.users.business_Phone,
        //   this.gender = res.users.gender,
        //   this.address_Line1 = res.users.address_Line1,
        //   this.address_Line2 = res.users.address_Line2,
        //   this.city = res.users.city,
        //   this.country = res.users.country,
        //   this.state = res.users.state
        //   this.getstates(res.users.country == null? 38 : res.users.country);
        //   this.userInfo = res.users;
        // }
        // this.image_Name = res.user.image_Name

        this.adminReg.patchValue({
          member_Id: this.admin.member_Id,
          login_Id: this.admin.login_Id,
          password: this.admin.password,
          confirm_password: this.admin.password,
          first_Name: this.admin.first_Name,
          last_Name: this.admin.last_Name,
          phone_Number: this.admin.phone_Number,
          email_Address: this.admin.email_Address,
          image_Url: this.admin.image_Url,
          zip_Code: this.admin.zip_Code,
          fax_Number: this.admin.fax_Number,
          business_Phone: this.admin.business_Phone,
          gender: this.admin.gender,
          address_Line1: this.admin.address_Line1,
          address_Line2: this.admin.address_Line2,
          city: this.admin.city,
          country: this.admin.country,
          state: this.admin.state,
          admin_Type: this.admin.admin_Type,
          cnfpassword: this.admin.password,

          last_Login: res.administrators.last_Login,
          failed_Attempt: res.administrators.failed_Attempt,
          last_Failed_Login: res.administrators.last_Failed_Login,
          is_Email_Verified: res.administrators.is_Email_Verified,
          is_Temp_Password: res.administrators.is_Temp_Password,
          is_Locked: res.administrators.is_Locked,
          is_Active: res.administrators.is_Active,
          is_Deleted: res.administrators.is_Deleted,
          last_Edited_By: res.administrators.last_Edited_By,
          dts: new Date()
          // image_Name: this.image_Name
        });
        this.config.stopLoader();
        // this.currentProfilePic = res.admin.image_Url;
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
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
    // console.log(this.imageSrc);
    // console.log(this.imageExention);
    Swal.fire('Profile pciture is temporarily saved, Please submit it from the bottom.');
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
        Swal.fire("Success", res.message, "success");
        this.submitted = false;
        //this.adminReg.reset();
        console.log(res);
        this.logoUpload(res.administrators.member_Id);
        this.config.stopLoader();
        this.router.navigateByUrl("/administrator-list");
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
    this.api.GetAllUser(userType).subscribe(
      res => {
        this.userslist = res.administrators;
        //console.log("Admin List : " + res);
      },
      err => {
        throw new Error(err);
      }
    );
  }

  loadSuperAdmin() {
    this.loadUserType("SuperAdmin");
  }
  loadGroupAdmin() {
    this.loadUserType("GroupAdmin");
  }
  loadAdmin() {
    this.loadUserType("Admin");
  }
  loadAllAdmin() {
    this.loadUserType("all");
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
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  password() {
    this.show = !this.show;
  } 
  AdminTypeChange(type)
  {
    this.adminType = type;
  }
}
