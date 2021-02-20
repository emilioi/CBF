import { Component, OnInit } from "@angular/core";
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ActivatedRoute, Router } from "@angular/router";
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators
} from "@angular/forms";
import { EmailService } from "../email.service";
import Swal from "sweetalert2";
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-edit-email-templates",
  templateUrl: "./edit-email-templates.component.html",
  styleUrls: ["./edit-email-templates.component.scss"]
})
export class EditEmailTemplatesComponent implements OnInit {
  updateTemplate: FormGroup;
  Id: any;
  tamplates: Array<object>;
  subject: any;
  content: any;
  Purpose: any;
  template_Id: any;
  fromEmail: any;
  submitted: boolean;
  public Editor = ClassicEditor;
  constructor(
    private ID: ActivatedRoute,
    private fb: FormBuilder,
    private service: EmailService,
    private router: Router,
    private api: EmailService,
     private config : Config
  ) {


// editorConfig = {
//   editable: true,
//   spellcheck: true,
//   height: '25rem',
//   minHeight: '5rem',
//   placeholder: 'Enter text here...',
//   translate: 'no',
//   uploadUrl: 'v1/images', // if needed
//   customClasses: [ // optional
//     {
//       name: "quote",
//       class: "quote",
//     },
//     {
//       name: 'redText',
//       class: 'redText'
//     },
//     {
//       name: "titleText",
//       class: "titleText",
//       tag: "h1",
//     },
//   ]
// };
    this.submitted = false;
    this.updateTemplate = this.fb.group({
      body: [""],
      purpose: [""],
      subject: [""],
      emailID: [""],
      fromAddress: [""]
    });

    this.Id = this.ID.snapshot.paramMap.get("Id");
  }
  ngOnInit() {
    //this.loadAllUser();
    this.loadEmailTemplate(this.Id);
  }

  UpdateEmailTemplate() {
    this.submitted = true;
    if (this.updateTemplate.invalid) {
      return;
    } else {
      this.config.startLoader();
      this.service.UpdateTemplate(this.updateTemplate.value).subscribe(res => {
        Swal.fire("Success", res.message, "success");
        this.updateTemplate.reset();
        this.submitted = false;
        this.config.stopLoader();
        this.router.navigateByUrl("/email-templates");
      });
    }
  }
  async loadEmailTemplate(Id) {
    this.config.startLoader();
    this.api.GetEmailTemplateByID(Id).subscribe(
      res => {
        this.tamplates = res.email_Templates;

        this.Purpose = res.email_Templates.purpose;
        this.subject = res.email_Templates.subject;
        this.content = res.email_Templates.body;
        this.template_Id = res.email_Templates.emailID;
        this.fromEmail = res.email_Templates.fromAddress;
        this.updateTemplate.setValue({
          purpose: this.Purpose,
          subject: this.subject,
          body: this.content,
          emailID: this.template_Id,
          fromAddress: this.fromEmail
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
