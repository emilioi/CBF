import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MemberService } from '../../Member/member.service';
import { Config } from 'src/app/utility/config';
import { PoolService } from '../../Pool/pool.service';
import { EntryService } from '../entry.service';
import Swal from "sweetalert2";
import { Router } from '@angular/router';
@Component({
  selector: 'app-add-entry',
  templateUrl: './add-entry.component.html',
  styleUrls: ['./add-entry.component.scss']
})
export class AddEntryComponent implements OnInit {
  config: any;
  entryForm: FormGroup;
  MemberList: any;
  pools: any;
  PoolId: any;
  NoOfTickets:any;
  MemberEmail: any;
  constructor(private fb: FormBuilder,
    private MemberAPI: MemberService,
    private PoolApi: PoolService,
    private EntryApi: EntryService,
    private route: Router,
    private loaderConfig: Config) {

    this.config = {
      displayKey: "description", //if objects array passed which key to be displayed defaults to description
      search: true, //true/false for the search functionlity defaults to false,
      height: 'auto', //height of the list so that if there are more no of items it can show a scroll defaults to auto. With auto height scroll will never appear
      placeholder: 'Select', // text to be displayed when no item is selected defaults to Select,
      customComparator: () => { }, // a custom function using which user wants to sort the items. default is undefined and Array.sort() will be used in that case,
      limitTo: 10, // a number thats limits the no of options displayed in the UI similar to angular's limitTo pipe
      moreText: 'more', // text to be displayed whenmore than one items are selected like Option 1 + 5 more
      noResultsFound: 'No results found!', // text to be displayed when no items are found while searching
      searchPlaceholder: 'Search', // label thats displayed in search input,
      searchOnKey: 'name' // key on which search should be performed this will be selective search. if undefined this will be extensive search on all keys
    };
    this.entryForm = this.fb.group({
      selectPool: [0],
      selectMember: [0],
      selectNoOfTickets:[0]
    });
  }

  ngOnInit() {
    this.LoadMembers();
    this.GetAllPools();
  }
  LoadMembers() {
    this.loaderConfig.startLoader();
    this.MemberAPI.GetAllUser("All").subscribe(
      res => {
        this.MemberList = [];
        this.loaderConfig.stopLoader();
        if (res.status == 1) {
          res.users.forEach(element => {
            var str = element.email_Address;
            this.MemberList.push(str);
          });

          console.log(this.MemberList);
        } else {

          this.MemberList = [];
        }
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }

  async GetAllPools() {
    this.loaderConfig.startLoader();
    this.PoolApi.GetAllPools().subscribe(
      res => {
        this.loaderConfig.stopLoader();
        this.pools = res.pool_Master;
        console.log(this.pools);
      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);
      }
    );
  }

 
  selectionChangedPool(poolId) {
    
    this.PoolId = poolId;
    console.log(this.PoolId);

  }
  selectionChangedMember(email_Address) {
    this.MemberEmail = email_Address.value;
    console.log(this.MemberEmail);
  }

  AddTickets() {
    this.NoOfTickets = this.entryForm.value.selectNoOfTickets;
    console.log(this.entryForm.value);
    console.log(this.PoolId);
    this.loaderConfig.startLoader();
    this.EntryApi.JoinClubAddTickets( this.PoolId,this.MemberEmail, this.NoOfTickets).subscribe(
      res => {
        this.loaderConfig.stopLoader();
        console.log(res);
        if (res.status == 1) {
          Swal.fire("Success", 'New tickets added successfully.', "success");
          this.route.navigateByUrl("/entry-list");
        }
        else {
          Swal.fire("Oops..", res.message, "error");
        }

      },
      err => {
        this.loaderConfig.stopLoader();
        throw new Error(err);

      }
    );
  }
}
