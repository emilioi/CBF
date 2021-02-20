import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators
} from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EmailService } from "../email.service";
import Swal from "sweetalert2";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-edit-mailing",
  templateUrl: "./edit-mailing.component.html",
  styleUrls: ["./edit-mailing.component.scss"]
})
export class EditMailingComponent implements OnInit {
  updateTemplate: FormGroup;
  Id: any;
  emails: any;
  subdate: any;
  mailingID: any;
  thistemplate_Id: any;
  referer: any;
  submitted: boolean;

  constructor(
    private api: EmailService,
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router
,
 
     private config : Config  ) {
    this.submitted = false;
    this.updateTemplate = this.fb.group({
      email: ["", Validators.required],
      mailingList_ID: new FormControl(),
      createdOn: new FormControl(),
      referer: new FormControl()
    });
    // this.code = this.navParams.get('code');
    this.Id = this.ID.snapshot.paramMap.get("Id");
  }
  ngOnInit() {
    //this.loadAllUser();
    this.loadEmailTemplate(this.Id);
  }

  get findInvalidC() {
    return this.updateTemplate.controls;
  }

  UpdateEmail() {
    
    this.submitted = true;
    if(this.updateTemplate.value.email== "" || this.updateTemplate.value.email== null || this.updateTemplate.value.email== undefined)
    {
      Swal.fire("Oops...", "Please enter the Email", "error")
    }
    
    if (this.updateTemplate.invalid) {
      return;
    } else {
      this.config.startLoader();
      this.api.UpdateEmail(this.updateTemplate.value).subscribe(res => {
        Swal.fire("Success", res.message, "success");
        this.updateTemplate.reset();
        this.submitted = false;
        this.config.stopLoader();
        this.router.navigateByUrl("/mailing-list");
      });
    }
  }

  async loadEmailTemplate(Id) {
    this.config.startLoader();
    this.api.GetEmail(Id).subscribe(
      res => {
        //console.log("This is GetAllMailing", res);
        this.emails = res.mailingLists.email;
        this.subdate = res.mailingLists.createdOn;
        this.mailingID = res.mailingLists.mailingList_ID;
        this.referer = res.mailingLists.referer;
        this.updateTemplate.setValue({
          email: this.emails,
          mailingList_ID: this.mailingID,
          referer: this.referer,
          createdOn: this.subdate
        });
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
}
