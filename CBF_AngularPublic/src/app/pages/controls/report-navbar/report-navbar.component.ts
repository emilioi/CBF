import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-report-navbar',
  templateUrl: './report-navbar.component.html',
  styleUrls: ['./report-navbar.component.scss']
})
export class ReportNavbarComponent implements OnInit {
  @Input() PoolID: number;
  @Input() PageName: any;
   @Input() PoolName: any;
  constructor(public router:Router) { }

  ngOnInit() {
    console.log(this.PageName);
  }

  viewTickets() {
    this.router.navigateByUrl("/ticket/" + this.PoolID);
  }
  
  pickAnalysis()
  {
  this.router.navigateByUrl("/pick-analysis/" + this.PoolID);
  }
   
  viewDefaults() {
    this.router.navigateByUrl("/default-picks/" + this.PoolID);
  }
  
}
