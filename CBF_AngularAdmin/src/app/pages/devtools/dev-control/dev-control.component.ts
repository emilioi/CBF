import { Component, OnInit } from '@angular/core';
import { DevelopmentService } from '../development.service';
import { Config } from 'src/app/utility/config';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-dev-control',
  templateUrl: './dev-control.component.html',
  styleUrls: ['./dev-control.component.scss']
})
export class DevControlComponent implements OnInit {
  IsPublicRegistrationOpen: any;
  Check_lookup_Value: any;
  Text_lookup_Value: any;
  allsettings: any;
  SettingForm: FormGroup;
  MaintenanceSettingForm: FormGroup;
  public Editor = ClassicEditor;
  constructor(private api: DevelopmentService,
    private loaderConfig: Config, private fb: FormBuilder,
  ) {

    this.SettingForm = this.fb.group({
      isPublicRegistrationOpen: false
    });

    this.MaintenanceSettingForm = this.fb.group(
      {
        Check_lookup_Value: new FormControl(),
        Text_lookup_Value: new FormControl()
      }
    )
  }

  ngOnInit() {
    this.LoadSetting();
    this.LoadMaintenanceSetting();
  }

  SettingOnChange(status) {
    this.loaderConfig.startLoader();
    this.api.ChangeRegistrationSetting(status).subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          this.IsPublicRegistrationOpen = res.settings[0].lookup_Value == "true" ? true : false;
        } else {
          this.IsPublicRegistrationOpen = "false";
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }
  LoadSetting() {
    this.loaderConfig.startLoader();
    this.api.GetRegistrationSetting().subscribe(
      res => {
        this.loaderConfig.stopLoader();

        if (res.status == 1) {
          this.IsPublicRegistrationOpen = res.settings[0].lookup_Value == "true" ? true : false;
          this.SettingForm.setValue({
            isPublicRegistrationOpen: this.IsPublicRegistrationOpen
          });
        } else {
          this.IsPublicRegistrationOpen = "false";
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }

  ChangeMaintenanceSetting() {
    this.loaderConfig.startLoader();
    let data = {
      maintenanceOn: this.MaintenanceSettingForm.value.Check_lookup_Value,
      maintenanceText: this.MaintenanceSettingForm.value.Text_lookup_Value
    };
    this.api.ChangeMaintenanceSetting(data).subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
        } else {
          Swal.fire("Failed", res.message, "error");
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }

  LoadMaintenanceSetting() {
    this.loaderConfig.startLoader();
    this.api.GetMaintenanceSetting().subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          this.allsettings = res.settings;
          var Check_lookup = false;
          var Text_lookup = '';
          this.allsettings.forEach(lokupSetting => {
            if (lokupSetting.lookup_Name == 'MaintenanceText') {
              console.log("Lookup2 " + lokupSetting.lookup_Value);
              Text_lookup = lokupSetting.lookup_Value;
            }
            if (lokupSetting.lookup_Name == 'MaintenanceOn') {
              console.log("Lookup " + lokupSetting.lookup_Value);
              Check_lookup = lokupSetting.lookup_Value == "true" ? true : false
            }
          });
          this.MaintenanceSettingForm.setValue({
            Check_lookup_Value: Check_lookup,
            Text_lookup_Value: Text_lookup,
          });

        } else {
          this.Check_lookup_Value = "false";
          this.Text_lookup_Value = ""
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }


  ///**********FOR NFL************///
  pickDefault() {
    this.api.CronPickDefaulted().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success")
        this.loaderConfig.stopLoader();
      }
    });
  }
  pickEliminate() {
    this.api.CronElimnation().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        this.cronWinner();
        Swal.fire("Success", res.message, "success");
        this.loaderConfig.stopLoader();
      }
      else {
        Swal.fire("Failed", res.message, "error");
        this.loaderConfig.stopLoader();
      }
    });
  }
  cronWinner() {
    this.api.CronWinners().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        //Swal.fire("Success", res.message, "success");
        this.loaderConfig.stopLoader();
      }
      else {
        //Swal.fire("Failed", res.message, "error");
        this.loaderConfig.stopLoader();
      }
    });
  }
  pickScore() {
    this.api.updateScore().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        Swal.fire("Success", "Score updated successfully", "success");
        this.loaderConfig.stopLoader();
      }
      else {
        Swal.fire("Failed", res.message, "error");
        this.loaderConfig.stopLoader();
      }
    });
  }
  sendFailedEmail() {
    this.api.Cron().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success");
        this.loaderConfig.stopLoader();
      }
      else {
        Swal.fire("Failed", res.message, "error");
      }
    })
  }


  ///**********FOR NHL************///
  pickDefaultNHL() {
    this.api.CronPickDefaultedNHL().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success")
        this.loaderConfig.stopLoader();
      }
    });
  }

  pickEliminateNHL() {
    this.api.CronElimnationNHL().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        this.cronWinner();
        Swal.fire("Success", res.message, "success");
        this.loaderConfig.stopLoader();
      }
      else {
        Swal.fire("Failed", res.message, "error");
        this.loaderConfig.stopLoader();
      }
    });
  }

  pickScoreNHL() {
    this.api.updateScoreNHL().subscribe(res => {
      this.loaderConfig.startLoader();
      if (res.status == 1) {
        Swal.fire("Success", "Score updated successfully", "success");
        this.loaderConfig.stopLoader();
      }
      else {
        Swal.fire("Failed", res.message, "error");
        this.loaderConfig.stopLoader();
      }
    });
  }


}
