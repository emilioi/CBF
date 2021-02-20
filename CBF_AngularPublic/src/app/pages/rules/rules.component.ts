import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../login/login.service';
import { Config } from 'src/app/utility/config';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-rules',
  templateUrl: './rules.component.html',
  styleUrls: ['./rules.component.scss']
})
export class RulesComponent implements OnInit {
  showModalReminder: any;
  MemberID: any;
  NFLrule: any;
  NHLrule: any;
  Rules: any;
  selectedRuleContent: any;
  firstRuleId: any;
  constructor(private router: Router, private api: LoginService, private config: Config) { }

  ngOnInit() {
    this.getRules();
    this.showModalReminder = JSON.parse(localStorage.getItem("userObj")).userInfo.rules_Accepted;
    this.MemberID = JSON.parse(localStorage.getItem("userObj")).userInfo.member_Id;
  }
  accepted() {
    this.config.startLoader();
    this.api.RulesAccpted(this.MemberID, true).subscribe(res => {
      console.log("RES" + res);
      if (res.status == 1) {
        this.router.navigateByUrl("/club-house");
        this.config.startLoader();
      }
      else {
        Swal.fire("Failed", "Something went wrong.", "error");
        this.router.navigateByUrl("/login");
        this.config.startLoader();
      }
    });
  }


  getRules() {
    this.api.getRules().subscribe(res => {
      this.Rules = res;
      this.firstRuleId = this.Rules[0].rule_Id;
      this.selectedRule(this.firstRuleId);
    });
  }

  selectedRule(id) {
    this.api.getRulesById(id).subscribe(res => {
      if (res.status == "1") {
        this.selectedRuleContent = res.rule.rule_Content;
      } else {
        console.log("Error=> ", res.message)
      }
    })
  }

}
