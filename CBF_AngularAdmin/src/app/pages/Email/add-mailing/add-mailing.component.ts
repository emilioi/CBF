import { Component, OnInit } from '@angular/core';
import { EmailService } from '../email.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';

@Component({
  selector: 'app-add-mailing',
  templateUrl: './add-mailing.component.html',
  styleUrls: ['./add-mailing.component.scss']
})
export class AddMailingComponent implements OnInit {
  saveTemplate: FormGroup;
  constructor(private api: EmailService,
    private router: Router,
     private config : Config,
    private fb: FormBuilder,
  ) {

    this.saveTemplate = this.fb.group({
      email: new FormControl(),
      //mailingList_ID: new FormControl()
    });
  }

  ngOnInit() {
  }
  SaveEmail() {
    this.config.startLoader();
    if (this.saveTemplate.value.email == "" || this.saveTemplate.value.email == null || this.saveTemplate.value.email == undefined) {
       this.config.stopLoader(); 
       Swal.fire("Oops...", "Please enter an Email Address!", "error");
    
    }
    else {
      this.api.UpdateEmail(this.saveTemplate.value).subscribe(res => {
        this.config.stopLoader(); 
        Swal.fire("Success", res.message, "success");
        this.router.navigateByUrl("/mailing-list");
      });
    }
  }
}
