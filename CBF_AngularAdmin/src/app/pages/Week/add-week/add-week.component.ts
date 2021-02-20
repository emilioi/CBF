import { Component, OnInit } from "@angular/core";
import Swal from "sweetalert2";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { WeekService } from "../week.service";
import { ActivatedRoute, Router, NavigationExtras } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { formatDate, DatePipe } from '@angular/common';
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-add-week",
  templateUrl: "./add-week.component.html",
  styleUrls: ["./add-week.component.scss"]
})
export class AddWeekComponent implements OnInit {
  poolFrm: FormGroup;
  weekInfo: any;
  weekNumbers: any;

  constructor(
    private api: WeekService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    public datepipe: DatePipe,
    private router: Router,
    private route: ActivatedRoute,
     private config : Config 
  ) {
    this.poolFrm = this.fb.group({
      pool_ID: new FormControl(),
      pool_Name: new FormControl(),
      week_Number: new FormControl(),
      cut_Off_Date: new FormControl(),
      start: new FormControl()
    });

    this.route.queryParams.subscribe(params => {
      if (this.router.getCurrentNavigation().extras.queryParams) {
        this.weekInfo = this.router.getCurrentNavigation().extras.queryParams.userInfo;
        //console.log("I found this info", this.weekInfo);
        this.poolFrm.patchValue({
          pool_ID: this.weekInfo.poolID,
          pool_Name: this.weekInfo.pool_Name,
          week_Number: this.weekInfo.weekNumber,
          cut_Off_Date: this.weekInfo.cutOff,
         //cut_Off_Date: formatDate(this.weekInfo.cutOff, "dd-MM-yyyy", 'en-US', '+0530'),
          start: this.weekInfo.start
        });
        this.getWeekNumberList(this.poolFrm.value);
      }
    });
    //
  }

  ngOnInit() {}

  saveUpdatePoolWeek() {

    //date fix
    let parts = this.poolFrm.value.cut_Off_Date.split("/");
    var d1 = new Date(Number(parts[2]), Number(parts[1]) - 1, Number(parts[0]));
    this.poolFrm.value.cut_Off_Date =this.datepipe.transform(d1, "MM/dd/yyyy");

    //console.log(d1);
  this.config.startLoader();
    this.api.savePoolWeek(this.poolFrm.value).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
           
          Swal.fire("Success", res.message, "success");
          this.router.navigate(["/week-list"]);
        } else {
           
          Swal.fire("Oops...", res.message, "error");
        }
      },
      err => {
       // console.log(err);
       this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  getWeekNumberList(data) {
   
    this.config.startLoader();
    this.api.getWeekNumber(data).subscribe(
      res => {
       // console.log("this is the week list i am getting", res);
        this.weekNumbers = res.weekNumbers;

       this.config.stopLoader();
      },
      err => {
       // console.log(err);
      this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
}
