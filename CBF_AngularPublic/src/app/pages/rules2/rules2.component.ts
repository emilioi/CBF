import { Component, OnInit } from '@angular/core';
import { LoginService } from '../login/login.service';

@Component({
  selector: 'app-rules2',
  templateUrl: './rules2.component.html',
  styleUrls: ['./rules2.component.scss']
})
export class Rules2Component implements OnInit {
  Rules: any;
  selectedRuleContent: any;
  firstRuleId: any;

  constructor(
    private loginApi: LoginService
  ) { }

  ngOnInit() {
    this.getRules()
  }
  getRules() {
    this.loginApi.getRules().subscribe(res => {
      this.Rules = res;
      this.firstRuleId = this.Rules[0].rule_Id;
      this.selectedRule(this.firstRuleId);
    });

  }
  selectedRule(id) {
    this.loginApi.getRulesById(id).subscribe(res => {
      if (res.status == "1") {
        this.selectedRuleContent = res.rule.rule_Content;
        console.log(this.selectedRuleContent)
      } else {
        console.log("Error=> ", res.message)
      }
    })
  }
}
