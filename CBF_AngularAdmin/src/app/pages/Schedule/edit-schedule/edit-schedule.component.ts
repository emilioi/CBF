import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ScheduleService } from '../schedule.service';
import { Config } from 'src/app/utility/config';

@Component({
  selector: 'app-edit-schedule',
  templateUrl: './edit-schedule.component.html',
  styleUrls: ['./edit-schedule.component.scss']
})
export class EditScheduleComponent implements OnInit {

  poolFrm: FormGroup;
  weekInfo: any;
  weekNumbers: any;
  Teams: any;
  pool_ID: any;
  scheduleID: any;
  pool_Name: any;
  constructor(
    private api: ScheduleService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private config: Config
  ) {
    this.poolFrm = this.fb.group({
      pool_ID: new FormControl({value:[0],disabled: true}),
      pool_Name: new FormControl({value:"",disabled: true}),
      week_Number: new FormControl({value:[0],disabled: true}),
      scheduleID: new FormControl({value:[0],disabled: true}),
      description: new FormControl({value:"",disabled: true}),
      visitingTeamID: new FormControl({value:[0],disabled: true}),
      homeTeamID: new FormControl({value:[0],disabled: true}),
      winnerTeamID: [0],
    });


  }

  ngOnInit() {
    this.getTeamList();
    this.route.queryParams.subscribe(params => {
      this.pool_ID = params["poolID"];
      this.pool_Name = params["pool_Name"];
      this.scheduleID = params["scheduleID"];
      console.log(this.pool_ID);
    });
 
this.getWeekNumberList();
  }
  loadSchedule() {
    this.config.startLoader();
    this.api.SurvScheduleListByScheduleId(this.scheduleID).subscribe(
      res => {
        console.log(res);
        this.poolFrm.setValue({
          pool_ID: res.survSchedule.poolID,
          pool_Name:this.pool_Name,
          week_Number: res.survSchedule.weekNumber,
          scheduleID: this.scheduleID,
          description: res.survSchedule.description,
          visitingTeamID: res.survSchedule.visitingTeam,
          homeTeamID: res.survSchedule.homeTeam,
          winnerTeamID: res.survSchedule.winner,

        });
      });

  }
  saveUpdatePoolWeek() {
    this.config.startLoader();
    //console.log(this.poolFrm.getRawValue(), 'pool');
    this.api.savePoolSchedule(this.poolFrm.getRawValue()).subscribe(

      res => {
      
        this.config.stopLoader();
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
          this.router.navigate(["/schedule-list"]);
        } else {
          Swal.fire("Oppss...", res.message, "error");
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  getWeekNumberList() {
    let data ={
      pool_ID : this.pool_ID
  };
    this.config.startLoader();
    this.api.getWeekNumber(data).subscribe(
      res => {
        this.config.stopLoader();
        this.weekNumbers = Object.values(res.weekNumbers);
        this.loadSchedule();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getTeamList() {
    this.config.startLoader();
    this.api.getTeamList().subscribe(
      res => {
        this.config.stopLoader();
        this.Teams = res.teamList;
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
}
