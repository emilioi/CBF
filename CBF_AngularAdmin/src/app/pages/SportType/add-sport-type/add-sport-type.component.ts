import { Component, OnInit } from "@angular/core";

import { FormGroup, FormBuilder } from "@angular/forms";
import { SportTypeService } from "../sport-type.service";
import Swal from "sweetalert2";
import { Router, ActivatedRoute } from "@angular/router";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-add-sport-type",
  templateUrl: "./add-sport-type.component.html",
  styleUrls: ["./add-sport-type.component.scss"]
})
export class AddSportTypeComponent implements OnInit {
  newSportType: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private api: SportTypeService,
    private router: Router,
    private loaderConfig : Config
  ) {
    this.newSportType = this.fb.group({
      // sportType: [],
      sportTypeName: []
    });
  }

  ngOnInit() {}

  addNewSportType() {
    this.loaderConfig.startLoader();
    this.api.updateNewSportType(this.newSportType.value).subscribe(res => {
      this.loaderConfig.stopLoader();
     if(this.newSportType.value.sportTypeName== ""|| this.newSportType.value.sportTypeName== null || this.newSportType.value.sportTypeName== undefined)
     {
       Swal.fire("Oops...", "Please enter the Sport Type", "error")
     }
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success");
        this.router.navigate(["sport-type-list/"]);
      } else {
        this.loaderConfig.stopLoader();
      }
    });
  }
}
