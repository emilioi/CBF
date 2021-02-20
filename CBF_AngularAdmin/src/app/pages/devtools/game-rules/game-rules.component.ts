import { Component, OnInit } from '@angular/core';
import { SportTypeService } from '../../SportType/sport-type.service';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { DevelopmentService } from '../development.service';
import Swal from 'sweetalert2';
import { Config } from 'src/app/utility/config';
import { trace } from 'console';

@Component({
  selector: 'app-game-rules',
  templateUrl: './game-rules.component.html',
  styleUrls: ['./game-rules.component.scss']
})
export class GameRulesComponent implements OnInit {
  sportType: any;
  IsRuleList: boolean = true;
  IsRuleEdit: boolean = false;
  public Editor = ClassicEditor;
  ruleContent: any = "";
  objRule = {
    rule_Id: 0,
    game_Type: "",
    rule_Title: "",
    rule_Content: ""
  }
  rules: any;

  constructor(
    private sportApi: SportTypeService,
    private devApi: DevelopmentService,
    private loaderConfig: Config
  ) { }

  ngOnInit() {
    this.getSportType();
    this.getRules()
  }

  getRules() {
    this.loaderConfig.startLoader();
    this.devApi.getRules().subscribe(res => {
      this.loaderConfig.stopLoader();
      this.rules = res;
      console.log(this.rules)
    })
  }

  getSportType() {
    this.sportApi.GetSportsType().subscribe(res => {
      this.sportType = res;
      console.log("SportType==> ", this.sportType)
    })
  }

  edit(Id) {
    this.devApi.getRulesById(Id).subscribe(res => {
      if (res.status == "1") {
        this.ruleContent = res.rule;
        this.objRule.rule_Id = res.rule.rule_Id;
        this.objRule.rule_Title = res.rule.rule_Title;
        this.objRule.game_Type = res.rule.game_Type;
        this.objRule.rule_Content = res.rule.rule_Content;
        console.log("this.objRule.rule_Content==> ", this.objRule.rule_Content)
        this.IsRuleEdit = true;
        this.IsRuleList = false;
      } else {
        console.log("Error=> ", res.message)
      }
    })
  }

  updateRules() {
    this.loaderConfig.startLoader();
    this.devApi.updateRules(this.objRule).subscribe(res => {
      this.loaderConfig.stopLoader();
      if (res.status == "1") {
        Swal.fire("Success", res.message, "success");

        this.IsRuleEdit = false;
        this.IsRuleList = true;
        this.getRules();
      }
      else {
        Swal.fire("Oops..", res.message, "error");
      }
    })
  }

  addRule() {
    this.objRule.rule_Id = 0;
    this.objRule.rule_Title = "";
    this.objRule.game_Type = "";
    this.objRule.rule_Content = "";
    this.IsRuleEdit = true;
    this.IsRuleList = false;
  }
  cancel() {
    this.IsRuleEdit = false;
    this.IsRuleList = true;
  }

  delete(id) {

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
        this.loaderConfig.startLoader();
        this.devApi.DeleteRule(id).subscribe(res => {
          this.loaderConfig.stopLoader();
          if (res.status == "1") {
            Swal.fire("Success", res.message, "success");
            this.getRules();
          } else {
            Swal.fire("Failed", res.message, "error");
          }
        });
      }
    });
  }
}
