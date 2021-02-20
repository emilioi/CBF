import { Component, OnInit } from "@angular/core";
import { AdministratorService } from "../administrator.service";
import { Router, NavigationExtras } from "@angular/router";
import Swal from "sweetalert2";
import { Config } from "src/app/utility/config";
import { formatDate } from '@angular/common';

@Component({
  selector: "app-administrator-list",
  templateUrl: "./administrator-list.component.html",
  styleUrls: ["./administrator-list.component.scss"]
})
export class AdministratorListComponent implements OnInit {
  userslist: Array<object>;
  Counts: any;
  cache: any;
  adminType: string;
  constructor(
    private api: AdministratorService,
    private router: Router,
    private config: Config
  ) { }

  ngOnInit() {
    this.cache = formatDate(new Date(), 'dd-MM-yyyy hh:mm:ss a', 'en-US', '+0530');
    this.loadUserType("all");
    this.dashboardCounts();
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

  async loadUserType(userType) {
    this.config.startLoader();
    this.api.GetAllUser(userType).subscribe(
      res => {
        this.userslist = res.administrators;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
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
        this.Counts = res.dashboard;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  adminDetail(Id) {
    this.router.navigate(["administrator-details/" + Id]);
  }

  editThisAdmin(Id) {
    // let navigationExtras: NavigationExtras = {
    //   queryParams: {
    //     userInfo: data
    //   }
    // };
    // this.router.navigate(["/add-administrator"], navigationExtras);
    this.router.navigate(["administrator-details/" + Id]);
  }

  delete(ID) {

    Swal.fire({
      title: 'Are you sure want to delete?',
      //text: "You won't be able to revert this!",
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.value) {
        this.config.startLoader();
        this.api.deleteAdmin(ID).subscribe(res => {
          if (res.status == 0) {
            this.config.stopLoader();
            Swal.fire("Unathorized", res.message, "error");
          } else {
            this.config.stopLoader();
            Swal.fire("Success", "Deleted Successfully!", "success");
          }
          this.loadUserType("all");
        });
      }
    });
  }
}
