import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EmailService } from '../email.service';
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';
import { ClipboardService } from 'ngx-clipboard'

@Component({
  selector: 'app-mailing-list',
  templateUrl: './mailing-list.component.html',
  styleUrls: ['./mailing-list.component.scss']
})
export class MailingListComponent implements OnInit {
  emails: any;
  Id: any;
  ShowMailingList: boolean = false;
  MailingList: any;
  constructor(private router: Router,
    private api: EmailService,
    private config: Config,
    private _clipboardService: ClipboardService
  ) { }

  ngOnInit() {
    this.loadMailingList(this.Id);
  }

  async loadMailingList(Id) {
    this.config.startLoader();
    this.api.GetEmailList(Id).subscribe(
      res => {
        //console.log("This is GetAllTemplate", res);
        this.emails = res;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  delete(ID) {
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
    //console.log("Service Start");
    this.api.deleteEmail(ID).subscribe((data) => {
      Swal.fire("Success", "Deleted Successfully!", "success");
      this.loadMailingList(this.Id);
    });
    //this.router.navigateByUrl("/delete-msg");
  }
});
  }
  gotoEdit(ID) {
    this.router.navigate(['edit-mailing/' + ID]);
  }
  GetMailingHref() {
    this.config.startLoader();


    this.api.DownloadEmailList().subscribe(
      res => {
        this.MailingList = res;
        this.ShowMailingList = true;
        this.config.stopLoader();
      });
  }
  copy(text: string) {
    this._clipboardService.copyFromContent(text)
  }
}
