import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';

@Component({
  selector: "app-site-header",
  templateUrl: "./site-header.component.html",
  styleUrls: ["./site-header.component.scss"]
})
export class SiteHeaderComponent implements OnInit {
  userObj: any;
  constructor(public router: Router) { }

  ngOnInit() {
    this.userObj = JSON.parse(localStorage.getItem("userObj"));
    console.log(this.userObj);
  }
  pageRefresh() {
    location.reload();
  }
  // keyDownFunction(event) {
  //   if (event.keyCode == 13) {
  //     alert("you just clicked enter");
  //     // rest of your code
  //   }
  // }
  signOut(){
    localStorage.removeItem("userObj");
   
    localStorage.removeItem("LoginMessage");
    this.router.navigateByUrl("/");
  }
}
