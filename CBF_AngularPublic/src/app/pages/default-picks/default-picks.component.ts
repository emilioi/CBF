import { Component, OnInit } from '@angular/core';
import { Config } from 'src/app/utility/config';
import { DefaultPickService } from './default-pick.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ClubHouseService } from '../club-house/club-house.service';

@Component({
  selector: 'app-default-picks',
  templateUrl: './default-picks.component.html',
  styleUrls: ['./default-picks.component.scss']
})
export class DefaultPicksComponent implements OnInit {
  SelectedPoolId: any;
  CurrentWeeklyDefaults: any;
  ClubDetail: any;

  constructor(public config: Config, 
    private route : ActivatedRoute, private apiClub: ClubHouseService,
    private router: Router,
    private api : DefaultPickService) { 

    this.SelectedPoolId = this.route.snapshot.paramMap.get("PoolId");
  }

  ngOnInit() {
    this.LoadPool();
    this.getNFLDataByWeek();
  }

  LoadPool() {
    this.apiClub.GetPoolByID(this.SelectedPoolId).subscribe(
      res => {
        this.ClubDetail = res.maintaince;
        
      }
    );
  }
  getNFLDataByWeek() {
    
    let data = {
      pool_ID: this.SelectedPoolId
      
    };
    this.config.startLoader();
    this.api.GetWeeklyDefaultsSchedule(data).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.CurrentWeeklyDefaults = res.weeklyDefaults;
          console.log(this.CurrentWeeklyDefaults);
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  BackToClubs()
  {
    this.router.navigateByUrl("/club-house");
  }
}
