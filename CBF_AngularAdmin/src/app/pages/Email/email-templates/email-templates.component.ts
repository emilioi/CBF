import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { EmailService } from "../email.service";
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';

@Component({
  selector: "app-email-templates",
  templateUrl: "./email-templates.component.html",
  styleUrls: ["./email-templates.component.scss"]
})
export class EmailTemplatesComponent implements OnInit {
  tamplates: Array<object>;
  constructor(private api: EmailService, private router: Router,
     private config : Config) { }
  ngOnInit() {
    //this.loadAllUser();
    this.loadEmailTemplate();
  }
  async loadEmailTemplate() {
    this.config.startLoader();
    this.api.GetAllEmailTemplate().subscribe(
      res => {
        this.tamplates = res;
        this.config.stopLoader();
      },
      err => {
        this.config.stopLoader();
        throw new Error(err);
      }
    );
  }
  delete(ID) { this.config.startLoader();
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
    this.api.deleteTemplate(ID).subscribe(res => { 
       Swal.fire("Success", res.message, "error");
     }); this.config.stopLoader();
    }
    });
  }

  gotoEdit(ID) {
    this.router.navigate(["edit-templates/" + ID]);
  }
}
