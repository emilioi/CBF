import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-site-header",
  templateUrl: "./site-header.component.html",
  styleUrls: ["./site-header.component.scss"]
})
export class SiteHeaderComponent implements OnInit {
  userObj: any;
  constructor() {}

  ngOnInit() {
    this.userObj = JSON.parse(localStorage.getItem("userObj"));
    console.log(this.userObj);
  }

  // keyDownFunction(event) {
  //   if (event.keyCode == 13) {
  //     alert("you just clicked enter");
  //     // rest of your code
  //   }
  // }
}
