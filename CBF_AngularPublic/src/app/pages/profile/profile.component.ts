import { Component, OnInit, ElementRef } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { Config } from "src/app/utility/config";
import { ProfileService } from "./profile.service";
import { formatDate } from "@angular/common";
import Swal from "sweetalert2";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.scss"]
})
export class ProfileComponent implements OnInit {
  Id: any;
  memberReg: FormGroup;
  userInfo: any;
  memberlist: any;
  submitted: boolean;
  show: any;
  state: any;
  countries: any;
  isEditableMode: boolean;
  showPwdInputBtn: boolean;
  CancelEditableMode: boolean;
  imageSrc: string = "";
  imageExention: string;
  currentMemberProfile: any;
  CountryId: any;
  member_Id: any;
  login_Id: any;
  password: any;
  confirm_password: any;
  first_Name: any;
  last_Name: any;
  phone_Number: any;
  email_Address: any;
  image_Url: any;
  zip_Code: any;
  fax_Number: any;
  business_Phone: any;
  gender: any;
  address_Line1: any;
  city: any;
  country: any;
  address_Line2: any;
  image_Name: any;
  reference: any;
  cache: any;
  verifyMemberForm: FormGroup;
  member: any;
  emailPreference: any;
  private _chrome = navigator.userAgent.indexOf("Chrome") > -1;
  constructor(
    private fb: FormBuilder,
    private api: ProfileService,
    private ID: ActivatedRoute,
    private config: Config,
    private router: Router,
    private _el: ElementRef
  ) {
    this.submitted = false;
    this.memberReg = this.fb.group({
      member_Id: new FormControl([0]),
      login_Id: new FormControl([""]),
      password: new FormControl([""]),
      first_Name: new FormControl([""]),
      last_Name: new FormControl([""]),
      phone_Number: new FormControl([""]),
      email_Address: new FormControl([""]),
      address_Line1: new FormControl([""]),
      address_Line2: new FormControl([""]),
      confirm_password: new FormControl([""]),
      city: new FormControl([""]),
      state: new FormControl([""]),
      country: new FormControl([""]),
      zip_Code: new FormControl([""]),
      fax_Number: new FormControl([""]),
      business_Phone: new FormControl([""]),
      gender: new FormControl([""]),
      image_Url: new FormControl([""]),
      last_Login: new FormControl(["2019-07-23T15:00:34.486Z"]),
      failed_Attempt: new FormControl([0]),
      last_Failed_Login: new FormControl(["2019-07-23T15:00:34.486Z"]),
      is_Email_Verified: new FormControl([true]),
      is_Temp_Password: new FormControl([true]),
      is_Locked: new FormControl([true]),
      is_Active: new FormControl([true]),
      is_Deleted: new FormControl([true]),
      last_Edited_By: new FormControl([0]),
      reference: new FormControl([""]),
      dts: null
    });
    this.Id = this.ID.snapshot.paramMap.get("Id");
    this.submitted = false;
    this.verifyMemberForm = this.fb.group({
      is_Active: [true],
      member_Id: [0]
    });
  }

  ngOnInit() {
    this.cache = formatDate(
      new Date(),
      "dd-MM-yyyy hh:mm:ss a",
      "en-US",
      "+0530"
    );
    this.showPwdInputBtn = false;
    this.isEditableMode = true;
    this.CancelEditableMode = false;
    this.loadUserById(this.Id);
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.emailPreference =
      JSON.parse(localStorage.getItem("userObj")) !== null
        ? JSON.parse(localStorage.getItem("userObj")).emailPreference
        : "";
    this.getCountry();
  }
  preview(files) {
    if (files.length === 0) return;

    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      return;
    }
    var reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = _event => {
      this.image_Url = reader.result;
    };
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
    Swal.fire(
      "Profile pciture is temporarily saved, Please submit it from the bottom."
    );
  }

  createMember() {
    if (
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
      this.memberReg.value.phone_Number == "" ||
      this.memberReg.value.phone_Number == null ||
      this.memberReg.value.phone_Number == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Phone Number", "error");
    } else if (
      this.isEditableMode == false &&
      (this.memberReg.value.password == "" ||
        this.memberReg.value.password == null ||
        this.memberReg.value.password == undefined)
    ) {
      Swal.fire("oops...", "Please enter the Password", "error");
    } else if (
      this.isEditableMode == false &&
      (this.memberReg.value.confirm_password == "" ||
        this.memberReg.value.confirm_password == null ||
        this.memberReg.value.confirm_password == undefined)
    ) {
      Swal.fire("Oops...", "Please enter the Confirm Password", "error");
    } else if (
      this.isEditableMode == false &&
      this.memberReg.value.password !== this.memberReg.value.confirm_password
    ) {
      Swal.fire(
        "Oops...",
        "Password & Confirm Password do not match. Please try again",
        "error"
      );
    } else {
      if (this.memberReg.invalid) {
        return;
      } else {
        if (
          this.memberReg.value.login_Id == "" ||
          this.memberReg.value.login_Id == null ||
          this.memberReg.value.login_Id == undefined
        ) {
          this.memberReg.value.login_Id = this.memberReg.value.email_Address;
        }

        this.api.createUser(this.memberReg.value).subscribe(res => {
          if (res.status == 1) {
            Swal.fire("Success", res.message, "success");
            this.submitted = false;
            //console.log(res);
            this.logoUpload(res.users.member_Id);
            this.loadUserById(this.Id);
            this.router.navigateByUrl("/profile");
          } else {
            //console.log("something went worng");
          }
        });
      }
    }
  }

  showPwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = true;
    this.isEditableMode = false;
    this.CancelEditableMode = true;
    this.config.stopLoader();
  }

  hidePwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = false;
    this.isEditableMode = true;
    this.CancelEditableMode = false;
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
        //console.log("Country LIst: ", this.countries);
      },
      err => {
        throw new Error(err);
      }
    );
  }
  async loadUserById(Id) {
    this.config.startLoader();
    this.api.GetMemberById(Id).subscribe(
      res => {
        //console.log('edit ' + JSON.stringify(res));
        if (res.users !== null) {
          (this.member_Id = res.users.member_Id),
            (this.login_Id = res.users.login_Id),
            (this.password = res.users.password),
            (this.confirm_password = res.users.password),
            (this.first_Name = res.users.first_Name),
            (this.last_Name = res.users.last_Name),
            (this.phone_Number = res.users.phone_Number),
            (this.email_Address = res.users.email_Address),
            (this.image_Url = res.users.image_Url),
            (this.zip_Code = res.users.zip_Code),
            (this.fax_Number = res.users.fax_Number),
            (this.business_Phone = res.users.business_Phone),
            (this.gender = res.users.gender),
            (this.address_Line1 = res.users.address_Line1),
            (this.address_Line2 = res.users.address_Line2),
            (this.city = res.users.city),
            (this.country = res.users.country),
            (this.state = res.users.state);
          this.getstates(res.users.country == null ? 38 : res.users.country);
          this.userInfo = res.users;
        }

        // this.image_Name = res.user.image_Name

        this.memberReg.setValue({
          member_Id: this.member_Id,
          login_Id: this.login_Id,
          password: this.password,
          confirm_password: this.password,
          first_Name: this.first_Name,
          last_Name: this.last_Name,
          phone_Number: this.phone_Number,
          email_Address: this.email_Address,
          image_Url: this.image_Url + "?cache=" + this.cache,
          zip_Code: this.zip_Code,
          fax_Number: this.fax_Number,
          business_Phone: this.business_Phone,
          gender: this.gender,
          address_Line1: this.address_Line1,
          address_Line2: this.address_Line2,
          city: this.city,
          country: this.country,
          state: this.state,

          last_Login: res.users.last_Login,
          failed_Attempt: res.users.failed_Attempt,
          last_Failed_Login: res.users.last_Failed_Login,
          is_Email_Verified: res.users.is_Email_Verified,
          is_Temp_Password: res.users.is_Temp_Password,
          is_Locked: res.users.is_Locked,
          is_Active: res.users.is_Active,
          is_Deleted: res.users.is_Deleted,
          last_Edited_By: res.users.last_Edited_By,
          reference: res.users.reference,
          dts: new Date()
          // image_Name: this.image_Name
        });
        this.config.stopLoader();
        this.currentMemberProfile = res.users.image_Url;
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  Spassword() {
    this.show = !this.show;
  }
  mailingList(event) {
    this.api.UpdateEmailPreference(event).subscribe(
      res => {
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
        } else {
          Swal.fire("Oops...", res.message, "error");
        }
      },
      err => {
        throw new Error(err);
      }
    );
  }
  singout() {
    localStorage.removeItem("userObj");
    this.router.navigateByUrl("/");
  }
}

