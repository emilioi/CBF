import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-delete-msg',
  templateUrl: './delete-msg.component.html',
  styleUrls: ['./delete-msg.component.scss']
})
export class DeleteMsgComponent implements OnInit {


  constructor(private _location: Location) 
  {}
  backClicked() {
    this._location.back();
  }
  ngOnInit() {
  }
}
