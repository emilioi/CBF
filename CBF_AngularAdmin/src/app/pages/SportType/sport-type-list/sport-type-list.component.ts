import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder } from "@angular/forms";
import { SportTypeService } from "../sport-type.service";
import Swal from "sweetalert2";
import { Router, NavigationExtras } from "@angular/router";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-sport-type-list",
  templateUrl: "./sport-type-list.component.html",
  styleUrls: ["./sport-type-list.component.scss"]
})
export class SportTypeListComponent implements OnInit {
  sportType: any;

  constructor(
    private fb: FormBuilder,
    private api: SportTypeService,
    private router: Router,
    private loaderConfig: Config
  ) { }

  ngOnInit() {
    this.getSportTypeList();
  }

  getSportTypeList() {
    this.loaderConfig.startLoader();
    this.api.GetSportsType().subscribe(res => {
      this.loaderConfig.stopLoader();
      //console.log("hey checking tis Sport type", res);
      this.sportType = res;
     // console.log("hey checking tis Sport type Again", this.sportType);
    });
  }

  editeThisSportType(sportType) {
    //console.log("edit this sport type", sportType);
    let navigationExtras: NavigationExtras = {
      queryParams: {
        sportType: sportType.sportType,
        sportTypeName: sportType.sportTypeName
      }
    };
    this.router.navigate(["edit-sport-type/"], navigationExtras);
  }

  delete(sportType) {
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
    this.loaderConfig.startLoader();
    this.api.deleteSportType(sportType).subscribe((res) => {
      if(res.status == 1){
        this.loaderConfig.stopLoader();
        Swal.fire("Success", res.message, "success");
        this.getSportTypeList();
      }
      else
      {
        this.loaderConfig.stopLoader();
        Swal.fire("Oops...", res.message, "error");
      }
    });
  }});
  }
}
