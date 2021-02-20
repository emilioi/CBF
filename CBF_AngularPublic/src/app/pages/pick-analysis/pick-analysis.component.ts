import { Component, OnInit, Input } from '@angular/core';
import { Config } from 'src/app/utility/config';
import { ActivatedRoute, Router } from '@angular/router';
import { PickAnalysisService } from './pick-analysis.service';
import { ClubHouseService } from '../club-house/club-house.service';

@Component({
  selector: 'app-pick-analysis',
  templateUrl: './pick-analysis.component.html',
  styleUrls: ['./pick-analysis.component.scss']
})
export class PickAnalysisComponent implements OnInit {
  CurrentPoolID: any;
  PickAnalysisReport: any;
  ClubDetail: any;
  PickAnalysisReportFlag: any;
  PickAnalysisCount:any;
  IsNFL : boolean = true;
  IsNHL : boolean = false;
  constructor(private config: Config,
    private route: ActivatedRoute,
    private api: PickAnalysisService,
    private apiClub: ClubHouseService,
    private router: Router) {
    this.CurrentPoolID = this.route.snapshot.paramMap.get("PoolId");
  }

  ngOnInit() {
    this.getPickAnalysisWithTeam();
   // this.getPickAnalysisEntriesCount(false);
    this.LoadPool();
  }

  LoadPool() {
    this.apiClub.GetPoolByID(this.CurrentPoolID).subscribe(
      res => {
        this.ClubDetail = res.maintaince;
        this.IsNFL = this.ClubDetail.sport_Type == 1? true : false;
        this.IsNHL = this.ClubDetail.sport_Type == 2? true : false;
        //this.IfclosePool = res.maintaince.is_Started;
        //console.log("Closed Pool? " + this.IfclosePool)
      }
    );
  }
  // getPickAnalysisEntriesCount(IsAll) {
  //   this.config.startLoader();
  //   this.api.PickAnalysisAliveByMember(this.CurrentPoolID,IsAll).subscribe(
  //     res => {
  //       this.config.stopLoader();
  //       this.PickAnalysisCount = res.pickReportCount;
  //       //console.log("PIcks Count: "+ JSON.stringify(this.PickAnalysisCount));
        
  //     },
  //     err => {
  //       this.config.stopLoader();
  //       throw new Error(err);
  //     }
  //   );
  // }

  getPickAnalysisWithTeam() {
    this.config.startLoader();
    this.api.PickAnalysisWithTeam(this.CurrentPoolID).subscribe(
      res => {
        this.PickAnalysisReport = res.pickAnalysis;
        this.PickAnalysisReportFlag = res.pickAnalysisFlag;
        this.PickAnalysisCount = res.pickReportCount;
        //console.log(res);
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  viewTickets(PoolId) {
    this.router.navigateByUrl("/ticket/" + PoolId);
  }
  BackToClubs() {
    this.router.navigateByUrl("/club-house");
  }
  totalEliminated(PoolId) {
    this.router.navigateByUrl("/ticket-eliminated/" + PoolId);
  }
  totalAlive(PoolId) {
    this.router.navigateByUrl("/ticket-alive/" + PoolId);
  } 
  viewDefaults(PoolId) {
    this.router.navigateByUrl("/default-picks/" + PoolId);
  }
  getClassFlag(team, week) {
    var keepGoing = true;
    var myClass = 'color-no';
    if (team.abbreviation !== 'Defaulted' && team.abbreviation !== 'Eliminated') {
      this.PickAnalysisReportFlag.forEach(element => {

        if (keepGoing) {
          if (week === 1 && team.abbreviation === element.abbreviation && element.week1 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 2 && team.abbreviation === element.abbreviation && element.week2 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 3 && team.abbreviation === element.abbreviation && element.week3 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 4 && team.abbreviation === element.abbreviation && element.week4 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 5 && team.abbreviation === element.abbreviation && element.week5 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 6 && team.abbreviation === element.abbreviation && element.week6 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 7 && team.abbreviation === element.abbreviation && element.week7 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 8 && team.abbreviation === element.abbreviation && element.week8 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 9 && team.abbreviation === element.abbreviation && element.week9 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 10 && team.abbreviation === element.abbreviation && element.week10 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 11 && team.abbreviation === element.abbreviation && element.week11 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 12 && team.abbreviation === element.abbreviation && element.week12 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 13 && team.abbreviation === element.abbreviation && element.week13 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 14 && team.abbreviation === element.abbreviation && element.week14 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 15 && team.abbreviation === element.abbreviation && element.week15 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 16 && team.abbreviation === element.abbreviation && element.week16 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          if (week === 17 && team.abbreviation === element.abbreviation && element.week17 !== 0) {
            keepGoing = false;
            myClass = 'color-red';
          }
          
        }
      });
    }
    return myClass;
 
  }
}
