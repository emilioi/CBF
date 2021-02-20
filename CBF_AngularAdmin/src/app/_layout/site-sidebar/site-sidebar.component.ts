import { Component, OnInit } from '@angular/core';
import { Config } from 'src/app/utility/config';

@Component({
  selector: 'app-site-sidebar',
  templateUrl: './site-sidebar.component.html',
  styleUrls: ['./site-sidebar.component.scss']
})
export class SiteSidebarComponent implements OnInit {

  fullName: string;
  adminType: string;
  constructor(private config: Config) { }

  ngOnInit() {
    this.fullName = localStorage.getItem("fullName");
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
  }
  PermissionCheck() {
   // console.log(this.adminType, 'AdminType---');
    if (this.adminType === "Admin") {
      return false;
    } else {
      return true;
    }
  }
  PermissionCheckGroupAdmin() {
    // console.log(this.adminType, 'AdminType---');
     if (this.adminType === "GroupAdmin") {
       return false;
     } else {
       return true;
     }
   }
}
