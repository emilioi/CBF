import { Component, OnInit } from "@angular/core";

import { FormGroup, FormBuilder } from "@angular/forms";
import { SportTypeService } from "../sport-type.service";
import Swal from "sweetalert2";
import { Router, ActivatedRoute } from "@angular/router";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-edit-sport-type",
  templateUrl: "./edit-sport-type.component.html",
  styleUrls: ["./edit-sport-type.component.scss"]
})
export class EditSportTypeComponent implements OnInit {
  updateSportType: FormGroup;
  editParms: any;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private api: SportTypeService,
    private router: Router,
    private loaderConfig : Config
  ) {
    this.route.queryParams.subscribe(params => {
      //console.log("this is new page", params);
      this.editParms = params;
    });

    this.updateSportType = this.fb.group({
      sportType: [this.editParms.sportType],
      sportTypeName: [this.editParms.sportTypeName]
    });
  }

  ngOnInit() {}

  updateNewSport() {
    this.loaderConfig.startLoader();
    this.api.updateNewSportType(this.updateSportType.value).subscribe(res => {
      this.loaderConfig.stopLoader();
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success");
        this.router.navigate(["sport-type-list/"]);
      } else {
        this.loaderConfig.stopLoader();
      }
    });
  }
}
