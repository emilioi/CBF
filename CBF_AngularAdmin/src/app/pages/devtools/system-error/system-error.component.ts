import { Component, OnInit } from '@angular/core';
import { Config } from 'src/app/utility/config';
import { DevelopmentService } from '../development.service';

@Component({
  selector: 'app-system-error',
  templateUrl: './system-error.component.html',
  styleUrls: ['./system-error.component.scss']
})
export class SystemErrorComponent implements OnInit {
  ErrorList:any;
  constructor(  private loaderConfig: Config,
    private api: DevelopmentService,) { }

  ngOnInit() {
    this.loadUserType();
  }
  async loadUserType() {
    this.loaderConfig.startLoader();
    this.api.GetErrorExceptionsList().subscribe(
      res => {
        this.loaderConfig.stopLoader();
        
        if (res.status == 1) {
          this.ErrorList = res.errorList;
        } else {
          this.ErrorList = [];
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }
}
