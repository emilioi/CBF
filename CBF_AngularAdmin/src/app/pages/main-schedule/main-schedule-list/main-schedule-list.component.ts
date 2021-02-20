import { Component, OnInit } from '@angular/core';
import { ScheduleService } from '../../Schedule/schedule.service';
import { Router } from '@angular/router';
import { Config } from 'src/app/utility/config';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-main-schedule-list',
  templateUrl: './main-schedule-list.component.html',
  styleUrls: ['./main-schedule-list.component.scss']
})
export class MainScheduleListComponent implements OnInit {
  weeks: any;
  seasons: any;
  selectedSeason: any = "";
  selectedWeek: any = "";
  schedules: any;
  constructor(
    private api: ScheduleService,
    private router: Router,
    private config: Config,
  ) { }

  ngOnInit() {
    this.getWeekList();
    this.getSeasonList();
    this.filterSchedule();
  }

  getWeekList() {
    this.config.startLoader();
    this.api.MainScheduleWeekList().subscribe(res => {
      this.config.stopLoader();
      if (res.status == "1") {
        this.weeks = res.weekList;
        console.log("WeelkList==> ", this.weeks);
      }
    })
  }

  getSeasonList() {
    this.config.startLoader();
    this.api.MainScheduleSeasonList().subscribe(res => {
      this.config.stopLoader();
      if (res.status == "1") {
        this.seasons = res.seasoncodeList;
        console.log("SeasonList==> ", this.seasons);
      }
    })
  }

  filterSchedule() {
    this.config.startLoader();
    this.api.MainScheduleList(this.selectedSeason, this.selectedWeek).subscribe(res => {
      this.config.stopLoader();
      if (res.status == "1") {
        this.schedules = res.nFLSchedules;
        console.log("Schedule==> ", this.schedules);
      }
    })
  }

  edit(id) {
    Swal.fire({
      title: 'Are you sure want to modify?',
      //text: "You won't be able to revert this!",
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.value) {
        this.router.navigateByUrl('/edit-main-schedule/' + id);
      }
    });
  }

  delete(id) {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.value) {
        this.config.startLoader();
        this.api.DeleteMainSchedule(id).subscribe(res => {
          this.config.stopLoader();
          if (res.status == "1") {
            Swal.fire("Success", res.message, "success");
            this.filterSchedule();
          } else {
            Swal.fire("Failed", res.message, "error");
          }
        });
      }
    });
  }

}
