import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  userObj: any;
  adminType:string;
  constructor() { }

  ngOnInit() {
    this.userObj = JSON.parse(localStorage.getItem("userObj"));
    //console.log(this.userObj);
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
}
