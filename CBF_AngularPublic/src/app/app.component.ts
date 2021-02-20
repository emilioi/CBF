import { Component, ViewChild, HostListener, OnInit } from "@angular/core";
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "CBF-AngularPublic";
  //location: Location;
  ngOnInit() {

  //   if (environment.production) {
  //     if (location.protocol === 'http:') {
  //       window.location.href = location.href.replace('http', 'https');
  //     }
  //   }
  // }
}
}
