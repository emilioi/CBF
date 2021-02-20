import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PoolService } from '../pool.service';
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';

@Component({
  selector: 'app-pool-list',
  templateUrl: './pool-list.component.html',
  styleUrls: ['./pool-list.component.scss']
})
export class PoolListComponent implements OnInit {
  pools: any;
  adminType: string;
  constructor(private router: Router, private api: PoolService,
    private config : Config) { }

  ngOnInit() {
    this.GetAllPools();
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
  }
  PermissionCheck() {
   
    if (this.adminType === "Admin"  || this.adminType === "GroupAdmin") {
      return false;
    } else {
      return true;
    }
  }
  async GetAllPools() {
    this.config.startLoader();
    this.api.GetAllPools().subscribe(
      res => {
        this.config.stopLoader();
        this.pools = res.pool_Master;
        //console.log("success " + JSON.stringify(this.pools));
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  delete(Id) {
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
    this.config.startLoader();
    this.api.deletePool(Id).subscribe(res => {
      this.config.stopLoader();
      if (res.status == 1) {
         Swal.fire("Success", res.message, "success");
         this.GetAllPools();
      } else {
       
        Swal.fire("Failed", res.message, "error");
      }
    });
     
  }});
  }

  gotoEdit(Id) {
   
    this.router.navigate(['edit-pool/' + Id]);
  }

}
