import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TeamService } from '../../Team/team.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../Schedule/schedule.service';
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';


@Component({
  selector: 'app-edit-main-schedule',
  templateUrl: './edit-main-schedule.component.html',
  styleUrls: ['./edit-main-schedule.component.scss']
})
export class EditMainScheduleComponent implements OnInit {
  mainScheduleForm: FormGroup
  weeks: any;
  seasons: any;
  teams: any;
  selectedHomeTeamShort: any;
  selectedVisitingTeamShort: any;
  selectedId: string;
  constructor(
    private fb: FormBuilder,
    private api: ScheduleService,
    private router: Router,
    private route: ActivatedRoute,
    private config: Config,
    private teamApi: TeamService
  ) {


    this.mainScheduleForm = this.fb.group({
      id: 0,
      week: "",
      startTime: "",
      endedTime: "",
      venueAllegiance: "",
      scheduleStatus: "",
      originalStartTime: "",
      delayedOrPostponedReason: "",
      playedStatus: "",
      attendance: "",
      officials: "",
      broadcasters: "",
      weather: "",
      homeTeamId: 0,
      visitingTeamID: 0,
      homeTeamShort: "",
      visitingTeamShort: "",
      gameDate: "2020-08-26T03:47:33.735Z",
      cutOffDate: "2020-08-26T03:47:33.735Z",
      venue_ID: 0,
      seasonCode: ""
    });
    this.selectedId = this.route.snapshot.paramMap.get("id");
  }

  ngOnInit() {
    this.getWeekList();
    this.getSeasonList();
    this.getTeams();
    this.loadSchedule();
  }

  saveSchedule() {
    this.mainScheduleForm.value.homeTeamId = parseInt(this.mainScheduleForm.value.homeTeamId);
    this.mainScheduleForm.value.visitingTeamID = parseInt(this.mainScheduleForm.value.visitingTeamID);
    this.mainScheduleForm.value.homeTeamShort = this.selectedHomeTeamShort;
    this.mainScheduleForm.value.visitingTeamShort = this.selectedVisitingTeamShort;
        this.mainScheduleForm.value.id = this.selectedId;
    console.log("Form Value==> ", this.mainScheduleForm.value)
    this.config.startLoader();
    this.api.CreateUpdateMainSchedule(this.mainScheduleForm.value).subscribe(res => {
      this.config.stopLoader();
      if (res.status == "1") {
        Swal.fire("Success", res.message, "success");
        this.router.navigateByUrl('main-schedule-list');
      }
      else {
        console.log("Error==> ", res.message);
      }
    });
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

  getTeams() {
    this.teamApi.getTeams().subscribe(res => {
      if (res.status == 1) {
        this.teams = res.teamList;
        console.log("Teams List=> ", this.teams)
      }
      else {
        console.log("Error==> ", res.message);
      }
    });
  }

  onChangeHomeTeam(val) {
    var selectedHomeTeam = this.teams.filter(x => x.team_Id === parseInt(val));
    this.selectedHomeTeamShort = selectedHomeTeam[0].abbreviation;
  }

  onChangeVisitingTeam(val) {
    var selectedVisitingTeam = this.teams.filter(x => x.team_Id === parseInt(val));
    this.selectedVisitingTeamShort = selectedVisitingTeam[0].abbreviation;
  }

  loadSchedule() {
    this.api.GetMainScheduleById(this.selectedId).subscribe(res => {
      if (res.status == "1") {
        console.log(res.nflSchedule);
        this.selectedHomeTeamShort = res.nflSchedule.homeTeamShort;
        this.selectedVisitingTeamShort = res.nflSchedule.visitingTeamShort;
        this.mainScheduleForm.patchValue({
          id: res.nflSchedule.id,
          week: res.nflSchedule.week,
          startTime: res.nflSchedule.startTime,
          endedTime: res.nflSchedule.endedTime,
          venueAllegiance: res.nflSchedule.venueAllegiance,
          scheduleStatus: res.nflSchedule.scheduleStatus,
          originalStartTime: res.nflSchedule.originalStartTime,
          delayedOrPostponedReason: res.nflSchedule.delayedOrPostponedReason,
          playedStatus: res.nflSchedule.playedStatus,
          attendance: res.nflSchedule.attendance,
          officials: res.nflSchedule.officials,
          broadcasters: res.nflSchedule.broadcasters,
          weather: res.nflSchedule.weather,
          homeTeamId: res.nflSchedule.homeTeamId,
          visitingTeamID: res.nflSchedule.visitingTeamID,
          homeTeamShort: res.nflSchedule.homeTeamShort,
          visitingTeamShort: res.nflSchedule.visitingTeamShort,
          gameDate: res.nflSchedule.gameDate,
          cutOffDate: res.nflSchedule.cutOffDate,
          venue_ID: res.nflSchedule.venue_ID,
          seasonCode: res.nflSchedule.seasonCode
        })
      }
      else {
        console.log("Error==> ", res.message);
      }
    })
  }

}
