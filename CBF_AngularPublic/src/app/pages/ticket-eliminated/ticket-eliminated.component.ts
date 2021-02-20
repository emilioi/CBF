import { Component, OnInit } from "@angular/core";
import { Config } from "src/app/utility/config";
import { ActivatedRoute, Router } from "@angular/router";
import { PickAnalysisService } from "../pick-analysis/pick-analysis.service";
import { ClubHouseService } from "../club-house/club-house.service";

@Component({
  selector: "app-ticket-eliminated",
  templateUrl: "./ticket-eliminated.component.html",
  styleUrls: ["./ticket-eliminated.component.scss"]
})
export class TicketEliminatedComponent implements OnInit {
  CurrentPoolID: any;
  PickAnalysisReport: any;
  ClubDetail: any;
  PickAnalysisCount: any;
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
    this.getPickAnalysisEliminatedByMember();
    //this.getPickAnalysisCount(false);
    this.LoadPool();
  }

  LoadPool() {
    this.apiClub.GetPoolByID(this.CurrentPoolID).subscribe(res => {
      this.ClubDetail = res.maintaince;
      this.IsNFL = this.ClubDetail.sport_Type == 1? true : false;
      this.IsNHL = this.ClubDetail.sport_Type == 2? true : false;
    });
  }
  // getPickAnalysisCount(IsAll) {
  //   this.config.startLoader();
  //   this.api.PickAnalysisAliveByMember(this.CurrentPoolID, IsAll).subscribe(
  //     res => {
  //       this.config.stopLoader();
  //       this.PickAnalysisCount = res.pickReportCount;
  //     },
  //     err => {
  //       this.config.stopLoader();
  //       throw new Error(err);
  //     }
  //   );
  // }
  getPickAnalysisEliminatedByMember() {
    this.config.startLoader();
    this.api.PickAnalysisEliminatedByMember(this.CurrentPoolID, "").subscribe(
      res => {
        this.PickAnalysisReport = res.pickAnalysisEliminated;
        this.PickAnalysisCount = res.pickReportCount;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  search(titleInput) {
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
}
