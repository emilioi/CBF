import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { PickAnalysisService } from "../pick-analysis/pick-analysis.service";
import { Config } from "src/app/utility/config";
import { ClubHouseService } from "../club-house/club-house.service";
import { ScrollEvent } from "ngx-scroll-event";
@Component({
  selector: "app-ticket-alive",
  templateUrl: "./ticket-alive.component.html",
  styleUrls: ["./ticket-alive.component.scss"]
})
export class TicketAliveComponent implements OnInit {
  CurrentPoolID: any;
  PickAnalysisReport: any = [];
  PickAnalysisReportCurrent: any;
  ClubDetail: any;
  IsAll: any;
  PickAnalysisCount: any;
  PageNo: any = 1;
  ScrollDepth: any;
  IsNFL : boolean = true;
  IsNHL : boolean = false;
  constructor(
    private config: Config,
    private route: ActivatedRoute,
    private api: PickAnalysisService,
    private apiClub: ClubHouseService,
    private router: Router
  ) {
    this.CurrentPoolID = this.route.snapshot.paramMap.get("PoolId");
  }

  ngOnInit() {
    this.getPickAnalysisAliveByMember(false, "");
    this.LoadPool();
    // window.addEventListener('scroll', this.scroll, true); //third parameter
  }

  // ngOnDestroy() {
  //     window.removeEventListener('scroll', this.scroll, true);
  // }

  // scroll = (event: any): void => {
  //   // Here scroll is a variable holding the anonymous function
  //   // this allows scroll to be assigned to the event during onInit
  //   // and removed onDestroy
  //   // To see what changed:
  //   this.ScrollDepth ++;
  //   const number = this.PickAnalysisReport.length;
  //   console.log(event);
  //   console.log('I am scrolling ' + number);
  //   if(this.PickAnalysisCount.totalEntriesCount > number)
  //   {
  //     console.log('Next API Call ' + number);
  //   }

  // };

  LoadPool() {
    this.apiClub.GetPoolByID(this.CurrentPoolID).subscribe(res => {
      this.ClubDetail = res.maintaince;
      this.IsNFL = this.ClubDetail.sport_Type == 1? true : false;
        this.IsNHL = this.ClubDetail.sport_Type == 2? true : false;
    });
  }

  getPickAnalysisAliveByMember(IsAll, titleInput) {
    this.config.startLoader();
    this.api
      .PickAnalysisAliveByMember(
        this.CurrentPoolID,
        IsAll,
        this.PageNo,
        titleInput
      )
      .subscribe(
        res => {
          this.config.stopLoader();
          this.PickAnalysisReportCurrent = res.pickAnalysisAlive;
          this.PickAnalysisCount = res.pickReportCount;
          //console.log(this.PickAnalysisReportCurrent, "new data");
          this.PickAnalysisReport.push(...this.PickAnalysisReportCurrent);
          //console.log(this.PickAnalysisReport, "main");
        },
        err => {
          this.config.stopLoader();
          throw new Error(err);
        }
      );
  }

  search(titleInput) {
    this.PickAnalysisReport = [];
    this.config.startLoader();
    this.api
      .PickAnalysisAliveByMember(
        this.CurrentPoolID,
        false,
        this.PageNo,
        titleInput
      )
      .subscribe(
        res => {
          this.config.stopLoader();
          this.PickAnalysisReportCurrent = res.pickAnalysisAlive;
          this.PickAnalysisCount = res.pickReportCount;
          console.log(this.PickAnalysisReportCurrent, "new data");
          this.PickAnalysisReport.push(...this.PickAnalysisReportCurrent);
          //console.log(this.PickAnalysisReport, "main");
        },
        err => {
          this.config.stopLoader();
          throw new Error(err);
        }
      );
  }

  searchOnChange(titleInput) {
    this.config.startLoader();
    this.api
      .PickAnalysisEliminatedByMember(this.CurrentPoolID, titleInput)
      .subscribe(
        res => {
          this.PickAnalysisReport = res.pickAnalysisEliminated;
          this.PickAnalysisCount = res.pickReportCount;
          this.config.stopLoader();
          //this.getPickAnalysisEliminatedByMember();
        },
        err => {
          this.config.stopLoader();
          throw new Error(err);
        }
      );
  }

  pickAnalysis(PoolId) {
    this.router.navigateByUrl("/pick-analysis/" + PoolId);
  }

  viewTickets(IsAll) {
    this.PickAnalysisReport = [];
    this.IsAll = IsAll;
    this.getPickAnalysisAliveByMember(IsAll, "");
  }
  handleScroll(event: ScrollEvent) {
    //console.log("scroll occurred", event.originalEvent);
    if (event.isReachingBottom) {
      //console.log(`the user is reaching the bottom`);
      if (this.IsAll) {
        this.PageNo += 1;
        //console.log(this.PageNo, "Page no");
        this.getPickAnalysisAliveByMember(this.IsAll, "");
      }
    }
    if (event.isReachingTop) {
      //console.log(`the user is reaching the top`);
    }
    if (event.isWindowEvent) {
      // console.log(`This event is fired on Window not on an element.`);
    }
  }
}
