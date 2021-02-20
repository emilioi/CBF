import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { TeamService } from "../team.service";
import Swal from "sweetalert2";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";

@Component({
  selector: "app-add-team",
  templateUrl: "./add-team.component.html",
  styleUrls: ["./add-team.component.scss"]
})
export class AddTeamComponent implements OnInit {
  newTeam: FormGroup;
  teamInfo: any;
  sportTypeList: any;
  submitted: boolean;
  imageSrc: string = "";
  imageExention: string;
  currentTeamLogo: any;
  constructor(
    private fb: FormBuilder,
    private api: TeamService,
    private router: Router,
    private route: ActivatedRoute,
    private ngxService: NgxUiLoaderService
  ) {
    this.submitted = false;

    this.newTeam = this.fb.group({
      teamID: [0],
      sportType: [0],
      teamName: [""],
      teamNameShort: [""],
      teamLogo: [(File = null)]
    });

    this.route.queryParams.subscribe(params => {
      if (this.router.getCurrentNavigation().extras.queryParams) {
        this.teamInfo = this.router.getCurrentNavigation().extras.queryParams.userInfo;

        this.newTeam.patchValue({
          teamID: this.teamInfo.team_Id,
          sportType: this.teamInfo.sportType,
          teamName: this.teamInfo.team_Name,
          teamNameShort: this.teamInfo.abbreviation
          // teamLogo: this.teamInfo.logoImageSrc
        });
        this.currentTeamLogo = this.teamInfo.logoImageSrc;
      }
    });
  }

  ngOnInit() {
    this.getSportTypeList();
  }

  get findInvalidC() {
    return this.newTeam.controls;
  }

  CreateUpdateNewTeam() {
    if (
      this.newTeam.value.sportType == "" ||
      this.newTeam.value.sportType == null ||
      typeof this.newTeam.value.sportType == "undefined"
    ) {
      Swal.fire("Oops...", "Please select sport type", "error");
    } else if (
      this.newTeam.value.teamName == "" ||
      this.newTeam.value.teamName == null ||
      typeof this.newTeam.value.teamName == "undefined"
    ) {
      Swal.fire("Oops...", "Please enter team name", "error");
    } else if (
      this.newTeam.value.teamNameShort == "" ||
      this.newTeam.value.teamNameShort == null ||
      typeof this.newTeam.value.teamNameShort == "undefined"
    ) {
      Swal.fire("Oops...", "Please enter team short name", "error");
    }
    this.submitted = true;
    if (this.newTeam.invalid) {
      return;
    } else {
      //this.ngxService.start();
      this.api.CreateUpdateTeams(this.newTeam.value).subscribe(
        res => {
          if (res.status == 1) {
            // this.submitted = false;
            this.logoUpload(res.teamList.team_Id);
            Swal.fire("Success", res.message, "success");
            this.router.navigateByUrl("/team-list");
          } else {
          }
        },
        err => {
          //this.ngxService.stop();
          throw new Error(err);
        }
      );
    }
  }

  logoUpload(teamId) {
    let data = {
      base64image: this.imageSrc,
      fileExtention: this.imageExention
    };
    this.api.upLoadBase64(teamId, data).subscribe(
      res => {
        //this.ngxService.stop();
        if (res.status == 1) {
          //Swal.fire("Success", res.message, "success");
          //this.router.navigate(["team-list/"]);
        } else {
          // console.log("something went wrong");
        }
      },
      err => {
        //this.ngxService.stop();
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
    console.log(this.imageSrc);
    console.log(this.imageExention);
  }

  getSportTypeList() {
    this.api.GetSportsType().subscribe(
      res => {
        // console.log(res);
        this.sportTypeList = res;
      },
      err => {
        throw new Error(err);
      }
    );
  }
}
