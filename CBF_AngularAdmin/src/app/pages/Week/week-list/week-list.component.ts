import { Component, OnInit } from "@angular/core";
import Swal from "sweetalert2";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { WeekService } from "../week.service";
import { ActivatedRoute, Router, NavigationExtras } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { Config } from "src/app/utility/config";

@Component({
  selector: "app-week-list",
  templateUrl: "./week-list.component.html",
  styleUrls: ["./week-list.component.scss"]
})
export class WeekListComponent implements OnInit {
  // poolFrm: FormGroup;
  weekMenuList: any;
  poolWeekListRes: any;
  selectdPoolId: any;
  Poolname: any;

  constructor(
    private api: WeekService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private config: Config
  ) {}

  ngOnInit() {
    this.weeekMenus();
  }

  weeekMenus() {
    this.config.startLoader();
    this.api.getWeeksMenu().subscribe(
      res => {
        this.config.stopLoader();
        console.log("GetWeeksMenu", res);
        if (res.status == 1) {
          this.weekMenuList = res.weekMenus;
          //console.log(this.weekMenuList);
        }
      },
      err => {
        console.log(err);
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  selectedWeek(menu) {
    this.config.startLoader();
    this.selectdPoolId = menu.pool_ID;
    this.Poolname = menu.pool_Name;
    this.api.GetPoolWeeksList(menu.pool_ID).subscribe(
      res => {
        this.config.stopLoader();
        console.log("GetPoolWeeksList", res);
        if (res.status == 1) {
          this.poolWeekListRes = res.poolMapped;
          console.log(this.poolWeekListRes);
        }
      },
      err => {
        this.config.stopLoader();
        console.log(err);
        throw new Error(err);
      }
    );
  }

  editThisPool(data) {
    let navigationExtras: NavigationExtras = {
      queryParams: {
        userInfo: data
      }
    };
    this.router.navigate(["/edit-week"], navigationExtras);
  }

  deleteThisPoolWeek(data) {
    Swal.fire({
      title: "Are you sure want to delete?",
      //text: "You won't be able to revert this!",
      type: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes",
      cancelButtonText: "No"
    }).then(result => {
      if (result.value) {
        this.config.startLoader();
        this.api.deletPoolWeek(data).subscribe(
          res => {
            this.config.stopLoader();

            if (res.status == 1) {
              Swal.fire("Success", res.message, "success");
              this.api.GetPoolWeeksList(data.poolID).subscribe(
                res => {
                  if (res.status == 1) {
                    this.poolWeekListRes = res.poolMapped;
                    console.log(this.poolWeekListRes);
                  }
                },
                err => {
                  this.config.stopLoader();
                  console.log(err);
                  throw new Error(err);
                }
              );
              console.log(data.poolID);
            } else {
              Swal.fire("Oops...", "Not allowed", "error");
            }
          },
          err => {
            //console.log(err);
            this.config.stopLoader();
            throw new Error(err);
          }
        );
      }
    });
  }

  addNewWeek() {
    let data = {
      poolID: this.selectdPoolId,
      pool_Name: this.Poolname,
      week_Number: "",
      cut_Off_Date: "",
      start: ""
    };
    console.log("Poolname =" + this.Poolname);
    if (
      this.selectdPoolId == "" ||
      this.selectdPoolId == null ||
      typeof this.selectdPoolId == "undefined"
    ) {
      Swal.fire("Oops...", "Please select any pool!", "error");
    } else {
      let navigationExtras: NavigationExtras = {
        queryParams: {
          userInfo: data
        }
      };
      this.router.navigate(["/add-week"], navigationExtras);
    }
  }
}
