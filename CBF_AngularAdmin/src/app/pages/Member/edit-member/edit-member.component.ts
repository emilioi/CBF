import { Component, OnInit, ElementRef } from '@angular/core';
import { MemberService } from '../member.service';
import Swal from 'sweetalert2';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Config } from 'src/app/utility/config';
import { formatDate } from '@angular/common';
import * as $ from 'jquery';
@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.scss']
})
export class EditMemberComponent implements OnInit {
  Id: any;
  memberReg: FormGroup;
  userInfo: any;
  memberlist: any;
  submitted: boolean;
  show:any;
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
  showModal: boolean;
  IsVerify: boolean;
  adminType:string;
  private _chrome = navigator.userAgent.indexOf("Chrome") > -1;
  constructor(
    private fb: FormBuilder,
    private api: MemberService,
    private ID: ActivatedRoute,
    private config: Config,
    private router: Router,
    private _el: ElementRef
  ) {
    this.submitted = false;
    this.memberReg = this.fb.group({
      member_Id: [0],
      login_Id: [""],
      password: [""],
      first_Name: [""],
      last_Name: [""],
      phone_Number: [""],
      email_Address: [""],
      address_Line1: [""],
      address_Line2: [""],
      confirm_password: [""],
      city: [""],
      state: [""],
      country: [""],
      zip_Code: [""],
      fax_Number: [""],
      business_Phone: [""],
      gender: [""],
      image_Url: [""],
      // image_Name: [""],
      last_Login: ["2019-07-23T15:00:34.486Z"],
      failed_Attempt: [0],
      last_Failed_Login: ["2019-07-23T15:00:34.486Z"],
      is_Email_Verified: [true],
      is_Temp_Password: [true],
      is_Locked: [true],
      is_Active: [true],
      is_Deleted: [true],
      last_Edited_By: [0],
      reference: [""],
      dts: null

    });
    this.Id = this.ID.snapshot.paramMap.get("Id");
    this.submitted = false;
    this.verifyMemberForm = this.fb.group({
      is_Active: [true],
      member_Id: [0],
    });
  }

  ngOnInit() {
    this.cache = formatDate(new Date(), 'dd-MM-yyyy hh:mm:ss a', 'en-US', '+0530');
    this.showPwdInputBtn = false;
    this.isEditableMode = true;
    this.CancelEditableMode = false;
    this.loadUserById(this.Id);
    this.loadAdminById(this.Id);
    //this.getCountry();

    
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;

    //script
    
  }
  PermissionCheck() {
    
    if (this.adminType === "Admin") {
      return false;
    } else {
      return true;
    }
  }
  preview(files) {
    if (files.length === 0)
      return;
 
    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
       
      return;
    }
 
    var reader = new FileReader();
    
    reader.readAsDataURL(files[0]); 
    reader.onload = (_event) => { 
      this.image_Url = reader.result; 
    }
  }
  logoUpload(adminId) {
    let data = {
      base64image: this.imageSrc,
      fileExtention: this.imageExention
    };
    this.api.upLoadBase64(adminId, data).subscribe(
      res => {
        console.log("this is upload res", res);
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
    Swal.fire('Profile pciture is temporarily saved, Please submit it from the bottom.');
    // console.log(this.imageSrc);
    // console.log(this.imageExention);
  }

  createMember() {
    if(!this.PermissionCheck())
    {
      Swal.fire("Oops...", "Not Authorized!", "error");
      return;
    }
    this.submitted = true;
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
      this.memberReg.value.login_Id == "" ||
      this.memberReg.value.login_Id == null ||
      this.memberReg.value.login_Id == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Login Id", "error");
    } else if (
      this.memberReg.value.email_Address == "" ||
      this.memberReg.value.email_Address == null ||
      this.memberReg.value.email_Address == undefined
    ) {
      Swal.fire("Oops...", "Please enter the Email Address", "error");
    } else if (
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
            if (res.status == 1) {
              Swal.fire("Success", res.message, "success");
              //this.memberReg.reset();
              this.submitted = false;
              console.log(res);
              this.logoUpload(res.users.member_Id);
              this.router.navigateByUrl("/member-list");
            } else {
              //console.log("something went worng");
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
          //console.log("hey checking tis email_Address exist", res);
        });
    } else {
      //console.log("INVALID EMAIL ! IGNORE");
    }
  }

  checkloginidExist() {
    if (this.memberReg.value.login_Id.length > 3) {
      this.api
        .checkUserNameExist(this.memberReg.value.login_Id)
        .subscribe(res => {
          //console.log("hey checking tis loign exist", res);
        });
    } else {
      //console.log("INVALID USER TYPE");
    }
  }

  showPwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = true;
    this.isEditableMode = false;
    this.CancelEditableMode = true
    this.config.stopLoader();
    $(document).ready(function(){
      $('.pwd.glyphicon').on('click', function () {
        //  $(this).toggleClass('glyphicon-eye-close').toggleClass('glyphicon-eye-open'); // toggle our classes for the eye icon
       if($('.password').attr('type') == 'password')
       {
        $('.password').attr('type','text');
       } 
       else{
        $('.password').attr('type','password');
       }
      });
    });
  }
  hidePwdInput() {
    this.config.startLoader();
    this.showPwdInputBtn = false;
    this.isEditableMode = true;
    this.CancelEditableMode = false
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
        console.log('edit ' + JSON.stringify(res));
        if (res.users !== null) {
          this.member_Id = res.users.member_Id,
            this.login_Id = res.users.login_Id,
            this.password = res.users.password,
            this.confirm_password = res.users.password,
            this.first_Name = res.users.first_Name,
            this.last_Name = res.users.last_Name,
            this.phone_Number = res.users.phone_Number,
            this.email_Address = res.users.email_Address,
            this.image_Url = res.users.image_Url,
            this.zip_Code = res.users.zip_Code,
            this.fax_Number = res.users.fax_Number,
            this.business_Phone = res.users.business_Phone,
            this.gender = res.users.gender,
            this.address_Line1 = res.users.address_Line1,
            this.address_Line2 = res.users.address_Line2,
            this.city = res.users.city,
            this.country = res.users.country,
            this.state = res.users.state
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
          image_Url: this.image_Url+'?cache='+this.cache,
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
  async loadAdminById(Id) {
    this.api.GetMemberById(Id).subscribe(
      res => {
        this.Id = res.users.member_Id;
        // console.log("This is member by ID", res);
        this.member = res.users;
        this.IsVerify = res.users.is_Active;
        console.log("Member= " + this.member.last_Login);
        this.verifyMemberForm.setValue({
          member_Id: this.Id,
          is_Active: this.IsVerify

        });
      },
      err => {
        throw new Error(err);
      }
    );
  }
  updateVerification() {
    this.submitted = true;
    if (this.verifyMemberForm.invalid) {
      return;
    } else {
      this.config.startLoader();
      this.api.verify(this.verifyMemberForm.value, this.Id, this.IsVerify).subscribe(res => {
        Swal.fire("Success", res.message, "success");
        this.loadUserById(this.Id)
        this.config.startLoader();
      });
    }
    this.showModal = false;
  }
  cancelVerify() {
    this.showModal = false;
    this.loadUserById(this.Id)
  }
  async isVeriedOnChange() {
    this.showModal = true;
    this.IsVerify = !this.IsVerify;
  }

  Spassword() {
    this.show = !this.show;
  } 

  copyMail() {
    let data = {
      email: this.member.email_Address,
      referer: "clubbigfun.com",
      active: 1
    };
    this.api.postMailinglist(data).subscribe(res => {
      Swal.fire("Success", res.message, "success");
      console.log("MailingList "+ res)
    });
  }

}
