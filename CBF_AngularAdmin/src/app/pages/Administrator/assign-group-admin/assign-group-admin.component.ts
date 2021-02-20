import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Config } from 'src/app/utility/config';
import { MemberService } from '../../Member/member.service';

@Component({
  selector: 'app-assign-group-admin',
  templateUrl: './assign-group-admin.component.html',
  styleUrls: ['./assign-group-admin.component.scss']
})
export class AssignGroupAdminComponent implements OnInit {
  @Input() GroupAdminID: number;
  AssignedMemberList: any;
  AllMemberList: any;
  selectedMemberToAdd: any;
  selectedMemberToRemove: any;
  ListOfAssignedMembers: any[];
  AssignedMmers: any;
  referenceList: any;
  config: {
    displayKey: string; //if objects array passed which key to be displayed defaults to description
    search: boolean; //true/false for the search functionlity defaults to false,
    height: string; //height of the list so that if there are more no of items it can show a scroll defaults to auto. With auto height scroll will never appear
    placeholder: string; // text to be displayed when no item is selected defaults to Select,
    customComparator: () => void; // a custom function using which user wants to sort the items. default is undefined and Array.sort() will be used in that case,
    limitTo: number; // a number thats limits the no of options displayed in the UI similar to angular's limitTo pipe
    moreText: string; // text to be displayed whenmore than one items are selected like Option 1 + 5 more
    noResultsFound: string; // text to be displayed when no items are found while searching
    searchPlaceholder: string; // label thats displayed in search input,
    searchOnKey: string; // key on which search should be performed this will be selective search. if undefined this will be extensive search on all keys
  };
  AgentList: any[];
  constructor(private route: ActivatedRoute, private configLoader: Config, private memberAPI: MemberService) {

    this.config = {
      displayKey: "description", //if objects array passed which key to be displayed defaults to description
      search: true, //true/false for the search functionlity defaults to false,
      height: 'auto', //height of the list so that if there are more no of items it can show a scroll defaults to auto. With auto height scroll will never appear
      placeholder: 'Select', // text to be displayed when no item is selected defaults to Select,
      customComparator: () => { }, // a custom function using which user wants to sort the items. default is undefined and Array.sort() will be used in that case,
      limitTo: 0, // a number thats limits the no of options displayed in the UI similar to angular's limitTo pipe
      moreText: 'more', // text to be displayed whenmore than one items are selected like Option 1 + 5 more
      noResultsFound: 'No results found!', // text to be displayed when no items are found while searching
      searchPlaceholder: 'Search', // label thats displayed in search input,
      searchOnKey: 'name' // key on which search should be performed this will be selective search. if undefined this will be extensive search on all keys
    };
  }

  ngOnInit() {

    this.getAllMemberByGroupAdminID();
    this.getReferenceList();
  }
  // selectionChangedMember(email_Address) {
  //   console.log(email_Address);
  // }
  // getAllMember(name) {

  //   this.configLoader.startLoader();
  //   this.memberAPI.GetMemberBySearch(name).subscribe(
  //     res => {
  //       this.configLoader.stopLoader();
  //       this.AllMemberList = res.users;
  //     },
  //     err => {
  //       this.configLoader.stopLoader();
  //       console.log(err);
  //     }
  //   );
  // }
  getMemberbyReferel(reference) {
     console.log(reference);
     this.configLoader.startLoader();
      this.memberAPI.GetAllUser(reference.value).subscribe(res => {
        this.AllMemberList = res.users;
        this.configLoader.stopLoader();
      });
    
  }
  async getReferenceList() {
    this.configLoader.startLoader();
    this.memberAPI.getReferenceList().subscribe(
      res => {
        this.configLoader.stopLoader();
        if (res.status == 1) {
          // this.referenceList = res.referenceList;
          console.log(res);
          this.AgentList = [];

          if (res.status == 1) {
            res.referenceList.forEach(element => {
              var str = element.reference ;
              this.AgentList.push(str);
            });


          } else {

            this.AgentList = [];
          }
        }
        else { }
      },
      err => {
        throw new Error(err);
      }
    );
  }
  getAllMemberByGroupAdminID() {

    this.configLoader.startLoader();
    this.memberAPI.getAllMemberByGroupAdminID(this.GroupAdminID).subscribe(
      res => {
        this.configLoader.stopLoader();
        this.AssignedMemberList = res.users;
      },
      err => {
        this.configLoader.stopLoader();
        console.log(err);
      }
    );
  }
  AssignSelectedMembers() {
    // console.log(this.selectedMemberToAdd);
    let members = this.selectedMemberToAdd;
    this.configLoader.startLoader();
    this.memberAPI.AssignGroupAdmin(this.GroupAdminID, members, false).subscribe(
      res => {
        this.configLoader.stopLoader();
        this.AssignedMemberList = res.users;
        this.getAllMemberByGroupAdminID();
      },
      err => {
        this.configLoader.stopLoader();
        console.log(err);
      }
    );

  }
  RemoveSelectedMembers() {
    // console.log(this.selectedMemberToRemove);
    let members = this.selectedMemberToRemove;
    this.configLoader.startLoader();
    this.memberAPI.AssignGroupAdmin(this.GroupAdminID, members, true).subscribe(
      res => {
        this.configLoader.stopLoader();
        this.AssignedMemberList = res.users;
        this.getAllMemberByGroupAdminID();
      },
      err => {
        this.configLoader.stopLoader();
        console.log(err);
      }
    );

  }
  changeAllMemberSelection(event) {
    this.selectedMemberToAdd = [];
    for (var i in event.target.selectedOptions) {
      if (event.target.selectedOptions[i].label) {
        this.selectedMemberToAdd.push(event.target.selectedOptions[i].value);
      }
    }
    console.log(this.selectedMemberToAdd);
  }
  changeAssignedMemberSelection(event) {

    this.selectedMemberToRemove = [];
    for (var i in event.target.selectedOptions) {
      if (event.target.selectedOptions[i].label) {
        this.selectedMemberToRemove.push(event.target.selectedOptions[i].value);
      }
    }

  }
}
