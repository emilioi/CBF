import { Component, OnInit } from '@angular/core';
import { EntryService } from '../entry.service';
import { ActivatedRoute } from '@angular/router';
import { Config } from 'src/app/utility/config';
import { FormGroup, FormBuilder } from '@angular/forms';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-view-entry',
  templateUrl: './view-entry.component.html',
  styleUrls: ['./view-entry.component.scss']
})
export class ViewEntryComponent implements OnInit {
  Id: any;
  entry: any;
  LogoWeeks: any;
  entryForm: FormGroup;
  ShowCurrentPick: any;
  PoolName: any;
  entryID: any;
  eliminated: any;
  entryName:any;

  constructor(private api: EntryService,
    private ID: ActivatedRoute,
    private config: Config,
    private fb: FormBuilder,
  ) {
    this.Id = this.ID.snapshot.paramMap.get('Id')
    this.PoolName = this.ID.snapshot.paramMap.get('Pool')
    this.entryForm = this.fb.group({
      entryID: [""],
      eliminated: [""],
      entryName: [""],
    })
  }
  ngOnInit() {
    this.bindEnriesByID(this.Id);
    this.getPickReportWithLogos(this.Id);
    this.ShowCurrentPick = true;
  }

  async bindEnriesByID(Id) {
    this.config.startLoader();
    this.api.GetPoolEntriesById(Id).subscribe(
      res => {
        this.config.stopLoader();
        this.entry = res.survEntries;
        this.entryID = res.survEntries.entryID;
        this.eliminated = res.survEntries.eliminated;
        this.entryName = res.survEntries.entryName;
        console.log("This is Entry List", this.entry);

        this.entryForm.setValue({
          entryID: this.entryID,
          eliminated: this.eliminated,
          entryName: this.entryName,
        })
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );

  }

  getPickReportWithLogos(Id) {
    this.config.startLoader();
    this.api.GetPickReportWithLogo(Id).subscribe(
      res => {
        this.LogoWeeks = JSON.parse(JSON.stringify(res)).pickReport;
        console.log(this.LogoWeeks);
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  logoWeek(weekNo) {
    if (typeof this.LogoWeeks != undefined && this.LogoWeeks != null && this.LogoWeeks.length > 0) {
      this.LogoWeeks.forEach(element => {
        //  console.log('element--',element);
        if (element.weekNumber == weekNo) {
          //console.log('week--',weekNo);
          return '"' + element.logoImageSrc + '"';
        }
      });
      return 'NoPick'
    }
  }

  updateEntry() {
    if (this.entryForm.invalid) {
      return;
    } else {
      this.config.startLoader();
      this.api.updateEntry(this.entryForm.value).subscribe(res => {
        Swal.fire("Success", res.message, "success");
        this.config.stopLoader();
      });
    }
  }
}
