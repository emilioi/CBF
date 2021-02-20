import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Config } from 'src/app/utility/config';
import { EntryService } from '../entry.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-entry-list',
  templateUrl: './entry-list.component.html',
  styleUrls: ['./entry-list.component.scss']
})
export class EntryListComponent implements OnInit {

  poolFrm: FormGroup;
  menus: any;
  entryList: any;
  currentMenu: any;
  searchFrom: FormGroup;
  constructor(
    private api: EntryService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router,
    private config: Config
  ) {

    this.searchFrom = this.fb.group({
      name: new FormControl(),
    });
  }

  ngOnInit() {
    this.weeekMenus();

  }

  weeekMenus() {
    this.config.startLoader();
    this.api.getEntryMenu().subscribe(
      res => {
        //console.log("GetEntriesMenu", res);
        if (res.status == 1) {
          this.config.stopLoader();
          this.menus = res.entryMenus;
          //console.log(this.weekMenuList);this.selectedWeek(this.menus);
        }
      },
      err => {
        //console.log(err);
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }

  selectedWeek(menu) {
    this.currentMenu = menu;
    //console.log(menu);
    this.config.startLoader();
    this.api.GetEntryWeeksList(menu.pool_ID).subscribe(
      res => {
        this.config.stopLoader();
        if (res.status == 1) {
          this.entryList = res.entryWeekLists;
        }
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  view(Id, Pool) {
    this.router.navigate(['view-entry/' + Id + '/' + Pool]);
  }
  delete(ID) {
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
    //console.log("Service Start");
    this.config.startLoader();
    this.api.deletEntryPool(ID).subscribe(res => {
      this.config.stopLoader();
      if (res.status == 1) {
        Swal.fire("Success", res.message, "success");
        this.selectedWeek(this.currentMenu);
      }
      else {
        Swal.fire("Failed", res.message, "error");
      }
    });
    //this.router.navigateByUrl("/delete-msg");
  }
});
  }

  SearchEntry() {
    if (this.searchFrom.value.name == "" || this.searchFrom.value.name == null) {
      return;
    }
    if (this.searchFrom.invalid) {
      return;
    } else {
      this.config.startLoader();
      this.api.SearchEntry(this.searchFrom.value.name,this.currentMenu.pool_ID).subscribe(res => {
        this.entryList = res.entryWeekLists;
        this.config.stopLoader();
      });
    }
  }
}
