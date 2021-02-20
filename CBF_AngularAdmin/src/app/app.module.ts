import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { CKEditorModule } from "@ckeditor/ckeditor5-angular";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { LoginComponent } from "./pages/login/login.component";
import { DashboardComponent } from "./pages/dashboard/dashboard.component";
import { Config } from "./utility/config";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { HttpModule } from "@angular/http";
import { AuthGuard } from "./utility/auth.gaurds";
import { SiteLayoutComponent } from "./_layout/site-layout/site-layout.component";
import { SiteHeaderComponent } from "./_layout/site-header/site-header.component";
import { SiteSidebarComponent } from "./_layout/site-sidebar/site-sidebar.component";
import { NotificationBarComponent } from "./_layout/notification-bar/notification-bar.component";
import { SiteFooterComponent } from "./_layout/site-footer/site-footer.component";
import { AddAdministratorComponent } from "./pages/Administrator/add-administrator/add-administrator.component";
import { AddMemberComponent } from "./pages/Member/add-member/add-member.component";
import { MemberListComponent } from "./pages/Member/member-list/member-list.component";
import { MemberDetailsComponent } from "./pages/Member/member-details/member-details.component";
import { AdministratorDetailsComponent } from "./pages/Administrator/administrator-details/administrator-details.component";
import { AdministratorListComponent } from "./pages/Administrator/administrator-list/administrator-list.component";
import { EditEmailTemplatesComponent } from "./pages/Email/edit-email-templates/edit-email-templates.component";
import { MailingListComponent } from "./pages/Email/mailing-list/mailing-list.component";
import { AddMailingComponent } from "./pages/Email/add-mailing/add-mailing.component";
import { EditMailingComponent } from "./pages/Email/edit-mailing/edit-mailing.component";
import { EmailTemplatesComponent } from "./pages/Email/email-templates/email-templates.component";
import { AddSportTypeComponent } from "./pages/SportType/add-sport-type/add-sport-type.component";
import { EditSportTypeComponent } from "./pages/SportType/edit-sport-type/edit-sport-type.component";
import { SportTypeListComponent } from "./pages/SportType/sport-type-list/sport-type-list.component";
import { EditMemberComponent } from "./pages/Member/edit-member/edit-member.component";
import { ProfileComponent } from "./pages/profile/profile.component";
import { TeamListComponent } from "./pages/Team/team-list/team-list.component";
import { AddTeamComponent } from "./pages/Team/add-team/add-team.component";
import { EditTeamComponent } from "./pages/Team/edit-team/edit-team.component";
import { PoolListComponent } from "./pages/Pool/pool-list/pool-list.component";
import { AddPoolComponent } from "./pages/Pool/add-pool/add-pool.component";
import { EditPoolComponent } from "./pages/Pool/edit-pool/edit-pool.component";
import { ProfileSettingComponent } from "./pages/profile-setting/profile-setting.component";
import { WeekListComponent } from "./pages/Week/week-list/week-list.component";
import { AddWeekComponent } from "./pages/Week/add-week/add-week.component";
import { EditWeekComponent } from "./pages/Week/edit-week/edit-week.component";
import { ScheduleListComponent } from "./pages/Schedule/schedule-list/schedule-list.component";
import { AddScheduleComponent } from "./pages/Schedule/add-schedule/add-schedule.component";
import { EditScheduleComponent } from "./pages/Schedule/edit-schedule/edit-schedule.component";
import { EntryListComponent } from "./pages/Entry/entry-list/entry-list.component";
import { ViewEntryComponent } from "./pages/Entry/view-entry/view-entry.component";
import { ForgotPasswordComponent } from "./pages/forgot-password/forgot-password.component";
import { DeleteMsgComponent } from "./pages/delete-msg/delete-msg.component";
import { NgxUiLoaderModule } from "ngx-ui-loader";
import { NgxPaginationModule } from "ngx-pagination";
import { ClipboardModule } from "ngx-clipboard";

import { EntriesWithoutPicksComponent } from "./pages/entries-without-picks/entries-without-picks.component";
import { MoveTicketComponent } from "./pages/move-ticket/move-ticket.component";
import { PickReportComponent } from "./pages/pick-report/pick-report.component";
import { AngularEditorModule } from "@kolkov/angular-editor";
import { WeeklyDeafaultsComponent } from "./pages/weekly-deafaults/weekly-deafaults.component";
import { DatePipe } from "@angular/common";
import { NgxSortableModule } from "ngx-sortable";
import { FileUploadModule } from "ng2-file-upload";
import { ResetPasswordComponent } from "./pages/reset-password/reset-password.component";
import { DevControlComponent } from "./pages/devtools/dev-control/dev-control.component";
import { SystemErrorComponent } from "./pages/devtools/system-error/system-error.component";
import { AddEntryComponent } from "./pages/Entry/add-entry/add-entry.component";
import { SelectDropDownModule } from "ngx-select-dropdown";
import { AssignGroupAdminComponent } from "./pages/Administrator/assign-group-admin/assign-group-admin.component";
import { EntriesByReferralComponent } from "./pages/entries-by-referral/entries-by-referral.component";
import { AlertListComponent } from "./pages/alert/alert-list/alert-list.component";
import { AddAlertComponent } from "./pages/alert/add-alert/add-alert.component";
import { EditAlertComponent } from "./pages/alert/edit-alert/edit-alert.component";
import { DefaultedReportComponent } from './pages/defaulted-report/defaulted-report.component';
import { MainScheduleListComponent } from './pages/main-schedule/main-schedule-list/main-schedule-list.component';
import { EditMainScheduleComponent } from './pages/main-schedule/edit-main-schedule/edit-main-schedule.component';
import { AddMainScheduleComponent } from './pages/main-schedule/add-main-schedule/add-main-schedule.component';
import { GameRulesComponent } from './pages/devtools/game-rules/game-rules.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    SiteLayoutComponent,
    SiteHeaderComponent,
    SiteSidebarComponent,
    NotificationBarComponent,
    SiteFooterComponent,
    AddAdministratorComponent,
    AddMemberComponent,
    MemberListComponent,
    MemberDetailsComponent,
    AdministratorDetailsComponent,
    AdministratorListComponent,
    EditEmailTemplatesComponent,
    MailingListComponent,
    AddMailingComponent,
    EditMailingComponent,
    EmailTemplatesComponent,
    AddSportTypeComponent,
    EditSportTypeComponent,
    SportTypeListComponent,
    EditMemberComponent,
    ProfileComponent,
    TeamListComponent,
    AddTeamComponent,
    EditTeamComponent,
    PoolListComponent,
    AddPoolComponent,
    EditPoolComponent,
    ProfileSettingComponent,
    WeekListComponent,
    AddWeekComponent,
    EditWeekComponent,
    ScheduleListComponent,
    AddScheduleComponent,
    EditScheduleComponent,
    EntryListComponent,
    ViewEntryComponent,
    ForgotPasswordComponent,
    DeleteMsgComponent,
    EntriesWithoutPicksComponent,
    MoveTicketComponent,
    PickReportComponent,
    WeeklyDeafaultsComponent,
    ResetPasswordComponent,
    DevControlComponent,
    SystemErrorComponent,
    AddEntryComponent,
    AssignGroupAdminComponent,
    EntriesByReferralComponent,
    AlertListComponent,
    AddAlertComponent,
    EditAlertComponent,
    DefaultedReportComponent,
    MainScheduleListComponent,
    EditMainScheduleComponent,
    AddMainScheduleComponent,
    GameRulesComponent,
  ],
  imports: [
    FileUploadModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    NgxUiLoaderModule,
    NgxPaginationModule,
    HttpClientModule,
    AngularEditorModule,
    NgxSortableModule,
    SelectDropDownModule,
    CKEditorModule,
    ClipboardModule
  ],
  providers: [Config, AuthGuard, DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule {}
