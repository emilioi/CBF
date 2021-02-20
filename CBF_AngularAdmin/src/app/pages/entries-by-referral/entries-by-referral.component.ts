import { Component, OnInit } from '@angular/core';
import { MemberService } from '../Member/member.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Config } from 'src/app/utility/config';
import { EntryService } from '../Entry/entry.service';

@Component({
  selector: 'app-entries-by-referral',
  templateUrl: './entries-by-referral.component.html',
  styleUrls: ['./entries-by-referral.component.scss']
})
export class EntriesByReferralComponent implements OnInit {
  memberlist: Array<object>;
  config: any;
  entryList: any;
  adminType: string;
  referenceList: any;

  collection = [];
  constructor(
    private api: MemberService,
    private Entryapi: EntryService,
    private router: Router,
    private route: ActivatedRoute,
    private loaderConfig: Config,
    // private fb: FormBuilder,
  ) {

  }

  ngOnInit() {
    this.getReferenceList()
  }
  async getReferenceList() {
    this.api.getReferenceList().subscribe(
      res => {
        if (res.status == 1) {
          this.referenceList = res.referenceList;
        }
        else { }
      },
      err => {
        throw new Error(err);
      }
    );
  }
  PermissionCheck() {
    if (this.adminType === "Admin") {
      return false;
    } else {
      return true;
    }
  }
  geEntryList(referral){
    this.loaderConfig.startLoader();
    this.Entryapi.GetEntriesWeeksListByReferral(referral).subscribe(
      res => {
        if (res.status == 1) {
          this.entryList = res.members;
          this.loaderConfig.stopLoader();
        }
        else { }
      },
      err => {
        throw new Error(err);
      }
    );
  }
}
