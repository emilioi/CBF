import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { PoolService } from "../pool.service";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import Swal from "sweetalert2";
import { Config } from 'src/app/utility/config';
import * as $ from 'jquery';
import  'jquery-ui';

@Component({
  selector: "app-add-pool",
  templateUrl: "./add-pool.component.html",
  styleUrls: ["./add-pool.component.scss"]
})
export class AddPoolComponent implements OnInit {
  updatePool: FormGroup;
  pool_ID: any;
  pool_Name: any;
  sport_Type: any;
  rules_URL: any;
  price: any;
  cut_Off: any;
  description: any;
  ruler_Season: any;
  sportTypeList: any;
  ClosedPool: any;
  passCode:any;
  private: boolean;
  imageSrc: any;
  imageExention: any;
  adminType: string;
  currentMemberProfile:any;
  constructor(
    private api: PoolService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
     private config : Config
  ) {
    this.updatePool = this.fb.group({
      // pool_ID: new FormControl(),
      pool_Name: new FormControl(),
      sport_Type: [1],
      rules_URL: new FormControl(),
      price: new FormControl([0]),
      cut_Off: new FormControl(),
      description: new FormControl(),
      is_Started: 'false', 
      ruler_Season: new FormControl(true),
      private: new FormControl(),
      passCode: new FormControl(),
      thrusdayGames: new FormControl(),
      saturdayGames: new FormControl(),
      sundayGames: new FormControl(),
    });
  }

  ngOnInit() {
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
    this.getSportTypeList();
    this.getClosedPoolList();
    // $(document).ready(function(){
    //   $(".calendar").datepicker({ dateFormat: 'dd/mm/yy' });
    // });
    this.updatePool.setValue(
      {
        sport_Type: 1,
        is_Started: 'false',
      }
    );
    this.ruler_Season = true;

  }
  PermissionCheck() {
   
    if (this.adminType === "Admin"  || this.adminType === "GroupAdmin") {
      return false;
    } else {
      return true;
    }
  }

  savePool() {
    this.updatePool.value.price =0;
    if(this.updatePool.value.pool_Name == "" || this.updatePool.value.pool_Name == null)
    {
      Swal.fire("Failed", "Please enter pool name.", "error");
      return;
    }
    if(this.updatePool.value.sport_Type == "" || this.updatePool.value.sport_Type <=0)
    {
      Swal.fire("Failed", "Please select sport type.", "error");
      return;
    }
    if( this.private ==true && (this.updatePool.value.passCode == "" || this.updatePool.value.passCode == null))
    {
      Swal.fire("Failed", "Please enter passcode.", "error");
      return;
    }
    // if(this.updatePool.value.price == "" || this.updatePool.value.price == null)
    // {
    //   Swal.fire("Failed", "Please enter valid price.", "error");
    //   return;
    // }
    console.log("Date: "+this.updatePool.value.cut_Off);
    if(this.updatePool.value.cut_Off == "" || this.updatePool.value.cut_Off == null)
    {
      Swal.fire("Failed", "Please enter valid Cut off date.", "error");
      return;
    }
    if(this.updatePool.value.is_Started == "" || this.updatePool.value.is_Started == null)
    {
      Swal.fire("Failed", "Please select pool close status.", "error");
      return;
    }
  if(this.updatePool.value.ruler_Season == null)
  {
    this.updatePool.value.ruler_Season = false;
  }
  if(this.updatePool.value.thrusdayGames == null)
  {
    this.updatePool.value.thrusdayGames = false;
  }
   if(this.updatePool.value.sundayGames == null)
  {
    this.updatePool.value.sundayGames = false;
  }
  if(this.updatePool.value.saturdayGames == null)
  {
    this.updatePool.value.saturdayGames = false;
  }
  if(this.updatePool.value.private == null)
  {
    this.updatePool.value.private = false;
  }
    this.config.startLoader();
    this.api.UpdatePool(this.updatePool.value, this.updatePool.value.ruler_Season, this.updatePool.value.thrusdayGames, this.updatePool.value.saturdayGames, this.updatePool.value.sundayGames).subscribe(res => {
      this.config.stopLoader();
      if (res.status == 1) {
        this.logoUpload(res.pool_Master.pool_ID);
        Swal.fire("Success", res.message, "success");
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
  privatePool(){
    this.private = !this.private
  }
  getClosedPoolList() {
    this.config.startLoader();
    this.api.GetClosePoolType().subscribe(
      res => {
        this.config.stopLoader();
        this.ClosedPool = res;
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
