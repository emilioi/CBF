import { Component, OnInit } from "@angular/core";
import Swal from "sweetalert2";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators
} from "@angular/forms";
import { PoolService } from "../pool.service";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import { DatePipe } from '@angular/common';
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-edit-pool",
  templateUrl: "./edit-pool.component.html",
  styleUrls: ["./edit-pool.component.scss"]
})
export class EditPoolComponent implements OnInit {
  updatePool: FormGroup;
  Id: any;
  pool_ID: any;
  pool_Name: any;
  sport_Type: any;
  rules_URL: any;
  price: any;
  cut_Off: any;
  description: any;
  is_Started: any;
  sportTypeList: any;
  ClosedPool: any;
  submitted: boolean;
  ruler_Season: boolean;
  private: boolean;
  thrusdayGames: boolean;
  saturdayGames: boolean;
  passCode:any;
  imageSrc: any;
  imageExention: any;
  currentMemberProfile:any;
  constructor(
    private api: PoolService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private config: Config,
    private datePipe: DatePipe
  ) {
    this.submitted = false;

    this.updatePool = this.fb.group({
      pool_ID: [""],
      pool_Name: ["", Validators.required],
      sport_Type: [""],
      rules_URL: [""],
      price: ["", Validators.required],
      cut_Off: ["", Validators.required],
      description: [""],
      is_Started: [""],
      ruler_Season: [""],
      private: [""],
      thrusdayGames: [""],
      saturdayGames: [""],
      passCode: [""]
    });

    this.Id = this.ID.snapshot.paramMap.get("Id");
  }

  ngOnInit() {
    this.loadPool(this.Id);
    this.getClosedPoolList();
    this.getSportTypeList();
  }

  get findInvalidC() {
    return this.updatePool.controls;
  }

  UpdatePool() {

    this.submitted = true;
    console.log("Test");
    if (this.updatePool.value.sport_Type == "" || this.updatePool.value.sport_Type <= 0 || typeof this.updatePool.value.sport_Type == "undefined") {
      Swal.fire("Failed", "Please select sport type.", "error");
      return;
    }
    if( this.private ==true && (this.updatePool.value.passCode == "" || this.updatePool.value.passCode == null))
    {
      Swal.fire("Failed", "Please enter passcode.", "error");
      return;
    }
    if (this.updatePool.value.price === "" || this.updatePool.value.price == null)// || this.updatePool.value.price == "" || typeof this.updatePool.value.price == "undefined"
    {
      Swal.fire("Failed", "Please enter valid price.", "error");
      return;
    }
    if (this.updatePool.value.cut_Off == "" || this.updatePool.value.cut_Off == null || typeof this.updatePool.value.cut_Off == "undefined") {
      Swal.fire("Failed", "Please enter valid Cut off date.", "error");
      return;
    }
    if (this.updatePool.value.is_Started === "" || this.updatePool.value.is_Started == null || typeof this.updatePool.value.is_Started == "undefined") {
      Swal.fire("Failed", "Please select pool close status.", "error");
      return;
    }
    // if (this.updatePool.invalid) {
    //   console.log(this.updatePool.value);
    //   return;
    // }
    if (this.updatePool.value.ruler_Season == null) {
      this.updatePool.value.ruler_Season = false;
    }
    if (this.updatePool.value.thrusdayGames == null) {
      this.updatePool.value.thrusdayGames = false;
    }
    if (this.updatePool.value.saturdayGames == null) {
      this.updatePool.value.saturdayGames = false;
    }
    if (this.updatePool.value.private == null) {
      this.updatePool.value.private = false;
    }

    this.config.startLoader();
    this.api
      .UpdatePool(this.updatePool.value, false, false, false)
      .subscribe(res => {
        this.config.stopLoader();
        if (res.status == 1) {
          Swal.fire("Success", res.message, "success");
          this.logoUpload(res.pool_Master.pool_ID);
          this.submitted = false;
          this.updatePool.reset();
          this.router.navigateByUrl("/pool-list");
        } else {
          Swal.fire("Failed", res.message, "error");
        }
      });
  }


  getSportTypeList() {
    this.config.startLoader();
    this.api.GetSportsType().subscribe(
      res => {
        this.config.stopLoader();
        this.sportTypeList = res;
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  async loadPool(Id) {
    this.config.startLoader();
    this.api.GetPoolByID(Id).subscribe(
      res => {
        //console.log('edit ' + JSON.stringify(res));
        this.pool_ID = res.maintaince.pool_ID;
        this.pool_Name = res.maintaince.pool_Name;
        this.price = res.maintaince.price;
        this.is_Started = res.maintaince.is_Started;
        this.rules_URL = res.maintaince.rules_URL;
        this.description = res.maintaince.description;
        this.sport_Type = res.maintaince.sport_Type;
        this.cut_Off = res.maintaince.cut_Off;
        this.ruler_Season = res.maintaince.ruler_Season;
        this.private = res.maintaince.private;
        this.thrusdayGames = res.maintaince.thrusdayGames;
        this.saturdayGames = res.maintaince.saturdayGames;
        this.passCode = res.maintaince.passCode;
        this.currentMemberProfile = res.maintaince.image_Url;
        // this.datePipe.transform(res.maintaince.cut_Off, 'dd/MM/yyyy');
        this.cut_Off = this.datePipe.transform(res.maintaince.cut_Off, 'dd/MM/yyyy');
        //console.log('started--' + this.is_Started);
        this.updatePool.setValue({
          pool_ID: this.pool_ID,
          pool_Name: this.pool_Name,
          price: this.price,
          is_Started: this.is_Started,
          rules_URL: this.rules_URL,
          description: this.description,
          sport_Type: this.sport_Type,
          cut_Off: this.cut_Off,
          ruler_Season: this.ruler_Season,
          private: this.private,
          thrusdayGames: this.thrusdayGames,
          saturdayGames: this.saturdayGames,
          passCode: this.passCode
        });
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  getClosedPoolList() {
    this.config.startLoader();
    this.api.GetClosePoolType().subscribe(
      res => {

        this.ClosedPool = res;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  handleInputChange(e) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      alert("invalid format");
      return;
    }
    reader.onload = this._handleReaderLoaded.bind(this);
    reader.readAsDataURL(file);
    this.imageExention = file.name.substring(
      file.name.length - 3,
      file.name.length
    );
  }
  _handleReaderLoaded(e) {
    let reader = e.target;
    this.imageSrc = reader.result;
    Swal.fire('Picture is temporarily saved, Please submit it from the bottom.');
    
  }
  logoUpload(poolId) {
    let data = {
      base64image: this.imageSrc,
      fileExtention: this.imageExention
    };
    this.api.upLoadBase64(poolId, data).subscribe(
      res => {
         
      },
      err => {
       
        throw new Error(err);
      }
    );
  }
}
