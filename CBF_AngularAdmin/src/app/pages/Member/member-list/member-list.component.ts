import { Component, OnInit } from "@angular/core";
import { MemberService } from "../member.service";
import { Router, NavigationExtras, ActivatedRoute } from "@angular/router";
import { NgxUiLoaderService } from "ngx-ui-loader";
import Swal from "sweetalert2";
import { map } from "rxjs/operators";
import { Config } from 'src/app/utility/config';
import { formatDate } from '@angular/common';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { AngularCsv } from 'angular-csv-ext/dist/Angular-csv';

@Component({
  selector: "app-member-list",
  templateUrl: "./member-list.component.html",
  styleUrls: ["./member-list.component.scss"]
})
export class MemberListComponent implements OnInit {
  memberlist: any;
  config: any;
  cache: any;
  verifyMemberForm: FormGroup;
  searchFrom: FormGroup;
  member: any;
  showModal: boolean;
  IsVerify: boolean;
  submitted: boolean;
  memberId: any;
  adminType: string;
  referenceList: any;
  reference : any = 'All';
  filterData : any = {
    isFilter: false,
    isSorting: false,
    isAscending: true,
    shortByName: "First_Name",
    filterByName: "None",
    filterByValue: ""
  }
  collection = [];
  constructor(
    private api: MemberService,
    private router: Router,
    private route: ActivatedRoute,
    private loaderConfig: Config,
    private fb: FormBuilder,
  ) {
    this.config = {
      currentPage: 1,
      itemsPerPage: 20
    };
    this.submitted = false;
    this.searchFrom = this.fb.group({
      name: new FormControl(),
      verifiedCheck: new FormControl,
    });
    this.verifyMemberForm = this.fb.group({
      is_Active: [true],
      member_Id: [0],
    });
    this.route.queryParamMap
      .pipe(map(params => params.get("page")))
      .subscribe(page => {
        ////console.log(page);
        this.config.currentPage = page;
      });

    for (let i = 1; i <= 100; i++) {
      this.collection.push(`item ${i}`);
    }
    

  }

  ngOnInit() {
    this.memberlist = null;
    this.cache = formatDate(new Date(), 'dd-MM-yyyy hh:mm:ss a', 'en-US', '+0530');
    //this.loadUserType();
    //this.FilterMemberList();
    //console.log("Catchr" + this.cache);
    var obj = JSON.parse(localStorage.getItem("userObj"));
    this.adminType = obj.userInfo.admin_Type;
    this.getReferenceList()
  }
  PermissionCheck() {
    ////console.log(this.adminType, 'AdminType---');
    if (this.adminType === "Admin" || this.adminType === 'GroupAdmin') {
      return false;
    } else {
      return true;
    }
  }

  pageChange(newPage: number) {
    this.router.navigate(["/member-list/"], { queryParams: { page: newPage } });
  }

  async loadUserType() {
    this.loaderConfig.startLoader();
    this.api.GetAllUser("All").subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          this.memberlist = res.users;
          //new AngularCsv(this.memberlist, 'My Report')
          ////console.log(new AngularCsv(this.memberlist, 'My Report')) ;
        } else {
          // //console.log("Something went wrong");
          this.memberlist = [];
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }
 
  exportTOCSV() {
    var options = {
      fieldSeparator: ',',
      quoteStrings: '"',
      decimalseparator: '.',
    //  showLabels: true,
    //  showTitle: true,
    //  title: 'Your title',
    //  useBom: true,
      noDownload: false,
      headers: ["ID", "Email", "First Name", "Last Name", "Agent", "Phone", "Created On", "Verified"],
      nullToEmptyString: true,
    };
    var MemberReports = [];
    this.memberlist.forEach(objMember => {
      let membersReport = {
        member_Id: objMember.member_Id,
        email_Address: objMember.email_Address,
        first_Name: objMember.first_Name,
        last_Name: objMember.last_Name,
        reference: objMember.reference,
        phone_Number: objMember.phone_Number,
        dts: objMember.dts,
        is_Active: objMember.is_Active,
      };
      MemberReports.push(membersReport);
    
    });  
    new AngularCsv(MemberReports, 'Member Report',options);

  }
  getMemberList(reference) {
    this.ResetFilterData();
    this.loaderConfig.startLoader();
    this.api.GetAllUser(reference).subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          this.memberlist = res.users;
          //console.log("Member List", res.users);
        } else {
          //console.log("Something went wrong");
          this.memberlist = [];
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );

  }
  ResetFilterData() {
    this.filterData = {
      isFilter: false,
      isSorting: false,
      isAscending: true,
      shortByName: "First_Name",
      filterByName: "None",
      filterByValue: ""
    }
  }

  async getReferenceList() {
    this.api.getReferenceList().subscribe(
      res => {
        if (res.status == 1) {
          this.referenceList = res.referenceList;
          //console.log("Refer LIst: " + this.referenceList);
        }
        else { }
      },
      err => {
        throw new Error(err);
      }
    );
  }

  memberDetails(Id) {
    this.router.navigate(["member-details/" + Id]);
  }

  editThisAdmin(Id) {
    this.router.navigate(['edit-member/' + Id]);
  }

  delete(ID) {
    if (!this.PermissionCheck()) {
      Swal.fire("Oops...", "Not Authorized!", "error");
      return;
    }
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
    this.api.deleteMember(ID).subscribe(res => {
      this.loaderConfig.stopLoader();
      if (res.status == 0) {
        Swal.fire("Unathorized", res.message, "error");
      } else {
        this.loaderConfig.stopLoader();
        Swal.fire("Success", "Deleted Successfully!", "success");
      }
      this.loadUserType();
    });
  }});
  }


  getMemberbyReferel() {
    // if(this.searchFrom.value.name == "" || this.searchFrom.value.name == null)
    // {
    //   return;
    // }
    // if(this.searchFrom.value.verifiedCheck == "" || this.searchFrom.value.verifiedCheck == null || typeof this.searchFrom.value.verifiedCheck == "undefined")
    // {
    //   return;
    // }
    //console.log(this.searchFrom.value)
    if (this.searchFrom.invalid) {
      return;
    } else {
      this.loaderConfig.startLoader();
      ////console.log("Value: " + this.searchFrom.value.name);
      this.api.GetMemberBySearch(this.searchFrom.value.name).subscribe(res => {
        this.memberlist = res.users;
        this.loaderConfig.stopLoader();
      });
    }
    this.showModal = false;
  }

  getVerifiedMember() {
    //console.log(this.searchFrom.value)
    if (this.searchFrom.invalid) {
      return;
    } else {
      this.loaderConfig.startLoader();
      ////console.log("Value: " + this.searchFrom.value.name);
      this.api.getVerifiedMembers(this.searchFrom.value.name, this.searchFrom.value.verifiedCheck).subscribe(res => {
        this.memberlist = res.users;
        this.loaderConfig.stopLoader();
      });
    }
    this.showModal = false;
  }

  updateVerification() {
    this.submitted = true;
    if (this.verifyMemberForm.invalid) {
      return;
    } else {
      this.loaderConfig.startLoader();
      this.api.verify(this.verifyMemberForm.value, this.memberId, this.IsVerify).subscribe(res => {
        Swal.fire("Success", res.message, "success");
        this.loadUserType();
        this.loaderConfig.stopLoader();
      });
    }
    this.showModal = false;
  }
  cancelVerify() {
    this.showModal = false;
    this.loadUserType();
  }
  async isVeriedOnChange(Id, IsVerify) {
    this.memberId = Id;
    this.showModal = true;
    this.IsVerify = !IsVerify;
  }
  async onFilterChange() {
    this.IsVerify = !this.IsVerify;
    this.getVerifiedMember()
  }

  sortMemberListBy(shortByName)
  {
    
    this.filterData.isAscending = !this.filterData.isAscending;
    this.filterData.isSorting = true;
    this.filterData.shortByName = shortByName;
    //console.log(this.filterData);
    this.GetMemberByFilter();
  }  
  
  MemberFilterChange(filterByName)
  {
    
    this.filterData.isFilter = true;
    this.filterData.filterByName = filterByName;
    console.log(this.filterData);
    if(filterByName =='None')
    {
      this.filterData.isFilter=false;
      this.filterData.filterByName ='';
      this.FilterMemberList();
    }
  }
  GetMemberByFilter()
  {
    if(this.filterData.filterByName =='Verified'  && this.filterData.filterByName !='None')
    {
      this.filterData.filterByValue = this.searchFrom.value.verifiedCheck;
    }
    else
    {
      this.filterData.filterByValue = this.searchFrom.value.name;
    }
    this.FilterMemberList();
  }
  getMemberListByReference(reference)
  {
    this.reference = reference;
    this.FilterMemberList();
    console.log(this.reference);
  }

  async FilterMemberList() {
    
    this.loaderConfig.startLoader();
    this.api.MemberFilter(this.filterData, this.reference).subscribe(
      res => {
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          this.memberlist=[];
          this.memberlist = res.users;
       
        } else {
          
          this.memberlist = [];
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }
}
