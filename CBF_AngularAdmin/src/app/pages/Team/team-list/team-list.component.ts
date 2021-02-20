import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { TeamService } from "../team.service";
import Swal from "sweetalert2";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { Config } from 'src/app/utility/config';

// Swal.fire("Success", res.message, "success");

@Component({
  selector: "app-team-list",
  templateUrl: "./team-list.component.html",
  styleUrls: ["./team-list.component.scss"]
})
export class TeamListComponent implements OnInit {
  teamName: any;

  constructor(
    private fb: FormBuilder,
    private api: TeamService,
    private router: Router,
    private route: ActivatedRoute,
     private loaderConfig : Config 
  ) {
    this.teamName = [];
  }

  ngOnInit() {
    this.getteamsName();
  }

  getteamsName() {
    this.loaderConfig.startLoader();
    this.api.getTeams().subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          this.teamName = res.teamList;
        } else {
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }

  editThisTeam(data) {
    let navigationExtras: NavigationExtras = {
      queryParams: {
        userInfo: data
      }
    };
    this.router.navigate(["/add-team"], navigationExtras);
  }

  delete(teamIdObj) {
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
    this.loaderConfig.startLoader();
    this.api.deleteTeamList(teamIdObj.team_Id).subscribe(data => {
      Swal.fire("Success", data.message, "success");
      this.getteamsName();
      this.loaderConfig.stopLoader();
    });
  }});
  }

}
