import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ScheduleService } from '../schedule.service';

@Component({
  selector: 'app-add-schedule',
  templateUrl: './add-schedule.component.html',
  styleUrls: ['./add-schedule.component.scss']
})
export class AddScheduleComponent implements OnInit {
  poolFrm: FormGroup;
  weekInfo: any;
  weekNumbers: any;
  Teams: any;

  constructor(
    private api: ScheduleService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.poolFrm = this.fb.group({
      pool_ID: [0],
      pool_Name: [""],
      week_Number: [0],
      scheduleID: [0],
      description: [""],
      visitingTeamID: [0],
      homeTeamID: [0],
      winnerTeamID: [0],
    });

    this.route.queryParams.subscribe(params => {
      if (this.router.getCurrentNavigation().extras.queryParams) {
        this.weekInfo = this.router.getCurrentNavigation().extras.queryParams.userInfo;
        this.poolFrm.patchValue({
          pool_ID: this.weekInfo.poolID,
          pool_Name: this.weekInfo.pool_Name,
          week_Number: this.weekInfo.week_Number,
         
        });
        this.getWeekNumberList(this.poolFrm.value);
         console.log("0Wek"+ JSON.parse(JSON.stringify(this.weekInfo)).week_Number);
      }
    });
  }

  ngOnInit() {
    this.getTeamList()
  }

  saveUpdatePoolWeek() {
 
    this.api.savePoolSchedule(this.poolFrm.value).subscribe(
    
      res => {
        // console.log("GetWeeksMenu", res);
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
          this.router.navigate(["/schedule-list"]);
        } else {
          Swal.fire("Oops...", res.message, "error");
        }
      },
      err => {
        throw new Error(err);
      }
    );
  }

  getWeekNumberList(data) {
    this.api.getWeekNumber(data).subscribe(
      res => {
        this.weekNumbers = Object.values(res.weekNumbers);
      },
      err => {
        throw new Error(err);
      }
    );
  }
  getTeamList() {
    this.api.getTeamList().subscribe(
      res => {
        this.Teams = res.teamList;
      },
      err => {
        throw new Error(err);
      }
    );
  }

}
