import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TicketService } from './ticket.service';
import { ClubHouseService } from '../club-house/club-house.service';

import { Config } from 'src/app/utility/config';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.scss']
})
export class TicketComponent implements OnInit {
  Tickets: any;
  showModalAddTicket: boolean;
  NewTickets: number;
  ShowticketCreated: boolean;
  selectedClubId: any;
  ClubDetail: any;
  IfclosePool: any;
  constructor(private router: Router,
    private api: TicketService,
    private apiClub: ClubHouseService,
    private config: Config,
    private route: ActivatedRoute,

  ) {
    this.selectedClubId = this.route.snapshot.paramMap.get("PoolId");
  }

  ngOnInit() {
    this.GetTicketsByMemberId(this.selectedClubId);
    this.LoadPool();
  }
  LoadPool() {
    this.apiClub.GetPoolByID(this.selectedClubId).subscribe(
      res => {
        this.ClubDetail = res.maintaince;
        this.IfclosePool = res.maintaince.is_Started;
        console.log("Closed Pool? " + this.IfclosePool)
      }
    );
  }
  GetTicketsByMemberId(PoolId) {
    this.config.startLoader();
    this.api.GetTicketsByMemberIdAndPoolId(PoolId).subscribe(
      res => {
        this.Tickets = JSON.parse(JSON.stringify(res)).survEntries;
        console.log("Tickets List " + JSON.stringify(this.Tickets));
        this.config.stopLoader();

      },
      err => {
        this.config.stopLoader();
        throw new Error(err);

      }
    );
  }
  AddTickets(count, selectedClub) {
    this.config.startLoader();
    this.apiClub.JoinClubAddTickets(count, selectedClub).subscribe(
      res => {
        console.log("CurrentPool " + this.selectedClubId)
        this.Tickets = JSON.parse(JSON.stringify(res)).survEntries;
        if (res.status == 1) {
          this.showModalAddTicket = false;
         this.ngOnInit();
          this.config.stopLoader();
          Swal.fire("Success", res.message, "success");
        }
        else {
          Swal.fire("Oops..", res.message, "error");
        }
      },
      err => {
        throw new Error(err);
      }
    );
    //this.LoadPool();
  }
  AddMoreTickets() {
    this.showModalAddTicket = true;
  }
  cancel() {
    this.showModalAddTicket = false;
  }
  ClearTickets() {
  }
  OpenTicket(ticket) {
    //  this.config.updateEntry(ticket);
    this.router.navigateByUrl("/pick-center/" + ticket.entryID +"?week="+ ticket.weekNumber);
  }
  BackToClubs()
  {
    this.router.navigateByUrl("/club-house");
  }
  pickAnalysis(PoolId)
  {
  this.router.navigateByUrl("/pick-analysis/" + PoolId);
  }

}
