import { Component, OnInit } from '@angular/core';
import { MemberService } from '../member.service';
import { Config } from 'src/app/utility/config';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.scss']
})
export class MemberDetailsComponent implements OnInit {
  verifyMemberForm: FormGroup;
  memberlist: any;
  Id: any;
  member: any;
  showModal: boolean;
  IsVerify: boolean;
  submitted: boolean;

  constructor(private api: MemberService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
  ) {
    this.submitted = false;
    this.Id = this.ID.snapshot.paramMap.get('Id')
    this.verifyMemberForm = this.fb.group({
      is_Active: [true],
      member_Id: [0],
    });
  }

  ngOnInit() {
  //  this.loadUserType();
    this.loadAdminById(this.Id);
  }
  async loadAdminById(Id) {
    this.api.GetMemberById(Id).subscribe(
      res => {
        this.Id = res.users.member_Id;
        // console.log("This is member by ID", res);
        this.member = res.users;
        this.IsVerify = res.users.is_Active;
        console.log("Member= "+ this.member.last_Login);
        this.verifyMemberForm.setValue({
          member_Id: this.Id,
          is_Active: this.IsVerify
          
        });
      },
      err => {
        throw new Error(err);
      }
    );
  }
  // async loadUserType() {
  //   this.api.GetAllUser().subscribe(
  //     res => {
  //       // console.log("This is GetAllUser", res);
  //       if (res.status == 1) {
  //         this.memberlist = res.users;
  //       } else {
  //         // console.log("Something went wrong");
  //         this.memberlist = [];
  //       }
  //     },
  //     err => {
  //       throw new Error(err);
  //     }
  //   );
  // }

  updateVerification() {
    this.submitted = true;
    if (this.verifyMemberForm.invalid) {
      return;
    } else {
      this.api.verify(this.verifyMemberForm.value, this.Id, this.IsVerify).subscribe(res => {
        Swal.fire("Success", res.message, "success");
        this.loadAdminById(this.Id);
      });
    }
    this.showModal = false;
  }
  cancelVerify() {
    this.showModal = false;
    this.loadAdminById(this.Id);
  }
  async isVeriedOnChange() {
    this.showModal = true;
    this.IsVerify = !this.IsVerify;
  }
}
