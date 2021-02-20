import { Component, OnInit } from '@angular/core';
import { ClubHouseService } from 'src/app/pages/club-house/club-house.service';
import { Config } from 'src/app/utility/config';

@Component({
  selector: 'app-site-sidebar',
  templateUrl: './site-sidebar.component.html',
  styleUrls: ['./site-sidebar.component.scss']
})
export class SiteSidebarComponent implements OnInit {
  clubs: any;
  fullName: string;
  constructor( private api: ClubHouseService,
    private config: Config,) { }

  ngOnInit() {
    this.fullName =  localStorage.getItem("fullName");
    this. GetClubs();
  }
  GetClubs() {
    this.config.startLoader();
    this.api.GetClubDetails().subscribe(
      res => {
        this.clubs = JSON.parse(JSON.stringify(res)).clubs;
        this.config.stopLoader();

      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
}
