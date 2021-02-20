import { NgModule, Component } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { LoginComponent } from "./pages/login/login.component";
import { DashboardComponent } from "./pages/dashboard/dashboard.component";
import { AuthGuard } from "./utility/auth.gaurds";
import { SiteLayoutComponent } from "./_layout/site-layout/site-layout.component";
import { AdministratorListComponent } from "./pages/Administrator/administrator-list/administrator-list.component";
import { AdministratorDetailsComponent } from "./pages/Administrator/administrator-details/administrator-details.component";
import { AddAdministratorComponent } from "./pages/Administrator/add-administrator/add-administrator.component";
import { MemberListComponent } from "./pages/Member/member-list/member-list.component";
import { MemberDetailsComponent } from "./pages/Member/member-details/member-details.component";
import { AddMemberComponent } from "./pages/Member/add-member/add-member.component";
import { AddMailingComponent } from "./pages/Email/add-mailing/add-mailing.component";
import { EditEmailTemplatesComponent } from "./pages/Email/edit-email-templates/edit-email-templates.component";
import { EditMailingComponent } from "./pages/Email/edit-mailing/edit-mailing.component";
import { EmailTemplatesComponent } from "./pages/Email/email-templates/email-templates.component";
import { MailingListComponent } from "./pages/Email/mailing-list/mailing-list.component";
import { AddSportTypeComponent } from "./pages/SportType/add-sport-type/add-sport-type.component";
import { SportTypeListComponent } from "./pages/SportType/sport-type-list/sport-type-list.component";
import { EditSportTypeComponent } from "./pages/SportType/edit-sport-type/edit-sport-type.component";
import { EditMemberComponent } from "./pages/Member/edit-member/edit-member.component";
import { ProfileComponent } from "./pages/profile/profile.component";
import { TeamListComponent } from "./pages/Team/team-list/team-list.component";
import { EditTeamComponent } from "./pages/Team/edit-team/edit-team.component";
import { AddTeamComponent } from "./pages/Team/add-team/add-team.component";
import { PoolListComponent } from "./pages/Pool/pool-list/pool-list.component";
import { EditPoolComponent } from "./pages/Pool/edit-pool/edit-pool.component";
import { AddPoolComponent } from "./pages/Pool/add-pool/add-pool.component";
import { ProfileSettingComponent } from "./pages/profile-setting/profile-setting.component";
import { AddWeekComponent } from "./pages/Week/add-week/add-week.component";
import { EditWeekComponent } from "./pages/Week/edit-week/edit-week.component";
import { WeekListComponent } from "./pages/Week/week-list/week-list.component";
import { EditScheduleComponent } from "./pages/Schedule/edit-schedule/edit-schedule.component";
import { AddScheduleComponent } from "./pages/Schedule/add-schedule/add-schedule.component";
import { ScheduleListComponent } from "./pages/Schedule/schedule-list/schedule-list.component";
import { EntryListComponent } from "./pages/Entry/entry-list/entry-list.component";
import { ViewEntryComponent } from "./pages/Entry/view-entry/view-entry.component";
import { ForgotPasswordComponent } from "./pages/forgot-password/forgot-password.component";
import { DeleteMsgComponent } from "./pages/delete-msg/delete-msg.component";
import { EntriesWithoutPicksComponent } from "./pages/entries-without-picks/entries-without-picks.component";
import { MoveTicketComponent } from "./pages/move-ticket/move-ticket.component";
import { PickReportComponent } from "./pages/pick-report/pick-report.component";
import { WeeklyDeafaultsComponent } from "./pages/weekly-deafaults/weekly-deafaults.component";
import { ResetPasswordComponent } from "./pages/reset-password/reset-password.component";
import { DevControlComponent } from "./pages/devtools/dev-control/dev-control.component";
import { SystemErrorComponent } from "./pages/devtools/system-error/system-error.component";
import { AddEntryComponent } from "./pages/Entry/add-entry/add-entry.component";
import { AssignGroupAdminComponent } from "./pages/Administrator/assign-group-admin/assign-group-admin.component";
import { EntriesByReferralComponent } from "./pages/entries-by-referral/entries-by-referral.component";
import { AlertListComponent } from "./pages/alert/alert-list/alert-list.component";
import { AddAlertComponent } from "./pages/alert/add-alert/add-alert.component";
import { EditAlertComponent } from "./pages/alert/edit-alert/edit-alert.component";
import { DefaultedReportComponent } from './pages/defaulted-report/defaulted-report.component';
import { MainScheduleListComponent } from './pages/main-schedule/main-schedule-list/main-schedule-list.component';
import { AddMainScheduleComponent } from './pages/main-schedule/add-main-schedule/add-main-schedule.component';
import { EditMainScheduleComponent } from './pages/main-schedule/edit-main-schedule/edit-main-schedule.component';
import { GameRulesComponent } from './pages/devtools/game-rules/game-rules.component';

const routes: Routes = [
  { path: "", component: LoginComponent },
  //{ path: 'dashboard', canActivate: [AuthGuard], component: DashboardComponent },
  {
    path: "",
    component: SiteLayoutComponent,
    children: [
      {
        path: "dashboard",
        canActivate: [AuthGuard],
        component: DashboardComponent,
        data: { title: "My Dashboard" }
      },
      {
        path: "administrator-list",
        canActivate: [AuthGuard],
        component: AdministratorListComponent
      },
      {
        path: "administrator-details/:Id",
        canActivate: [AuthGuard],
        component: AdministratorDetailsComponent
      },
      {
        path: "add-administrator",
        canActivate: [AuthGuard],
        component: AddAdministratorComponent
      },
      {
        path: "assign-group-admin/:Id",
        canActivate: [AuthGuard],
        component: AssignGroupAdminComponent
      },
      {
        path: "member-list",
        canActivate: [AuthGuard],
        component: MemberListComponent
      },
      {
        path: "member-details/:Id",
        canActivate: [AuthGuard],
        component: MemberDetailsComponent
      },
      {
        path: "add-member",
        canActivate: [AuthGuard],
        component: AddMemberComponent
      },
      {
        path: "edit-templates/:Id",
        canActivate: [AuthGuard],
        component: EditEmailTemplatesComponent
      },
      {
        path: "add-mailing",
        canActivate: [AuthGuard],
        component: AddMailingComponent
      },
      {
        path: "edit-mailing/:Id",
        canActivate: [AuthGuard],
        component: EditMailingComponent
      },
      {
        path: "email-templates",
        canActivate: [AuthGuard],
        component: EmailTemplatesComponent
      },
      {
        path: "mailing-list",
        canActivate: [AuthGuard],
        component: MailingListComponent
      },
      {
        path: "add-sport-type",
        canActivate: [AuthGuard],
        component: AddSportTypeComponent
      },
      {
        path: "sport-type-list",
        canActivate: [AuthGuard],
        component: SportTypeListComponent
      },
      {
        path: "edit-sport-type",
        canActivate: [AuthGuard],
        component: EditSportTypeComponent
      },
      {
        path: "edit-member/:Id",
        canActivate: [AuthGuard],
        component: EditMemberComponent
      },
      {
        path: "profile",
        canActivate: [AuthGuard],
        component: ProfileComponent
      },
      {
        path: "team-list",
        canActivate: [AuthGuard],
        component: TeamListComponent
      },
      {
        path: "edit-team",
        canActivate: [AuthGuard],
        component: EditTeamComponent
      },
      {
        path: "add-team",
        canActivate: [AuthGuard],
        component: AddTeamComponent
      },
      {
        path: "add-pool",
        canActivate: [AuthGuard],
        component: AddPoolComponent
      },
      {
        path: "edit-pool/:Id",
        canActivate: [AuthGuard],
        component: EditPoolComponent
      },
      {
        path: "pool-list",
        canActivate: [AuthGuard],
        component: PoolListComponent
      },
      {
        path: "profile-setting",
        canActivate: [AuthGuard],
        component: ProfileSettingComponent
      },
      {
        path: "add-week",
        canActivate: [AuthGuard],
        component: AddWeekComponent
      },
      {
        path: "edit-week",
        canActivate: [AuthGuard],
        component: EditWeekComponent
      },
      {
        path: "week-list",
        canActivate: [AuthGuard],
        component: WeekListComponent
      },
      {
        path: "schedule-list",
        canActivate: [AuthGuard],
        component: ScheduleListComponent
      },
      {
        path: "add-schedule",
        canActivate: [AuthGuard],
        component: AddScheduleComponent
      },
      {
        path: "edit-schedule",
        canActivate: [AuthGuard],
        component: EditScheduleComponent
      },
      {
        path: "entry-list",
        canActivate: [AuthGuard],
        component: EntryListComponent
      },
      {
        path: "view-entry/:Id/:Pool",
        canActivate: [AuthGuard],
        component: ViewEntryComponent
      },
      {
        path: "delete-msg",
        canActivate: [AuthGuard],
        component: DeleteMsgComponent
      },
      {
        path: "Set-Weekly-defaults",
        canActivate: [AuthGuard],
        component: WeeklyDeafaultsComponent
      },
      {
        path: "entries-without-picks",
        canActivate: [AuthGuard],
        component: EntriesWithoutPicksComponent
      },
      {
        path: "move-ticket",
        canActivate: [AuthGuard],
        component: MoveTicketComponent
      },
      {
        path: "pick-report",
        canActivate: [AuthGuard],
        component: PickReportComponent
      },
      {
        path: "dev-control",
        canActivate: [AuthGuard],
        component: DevControlComponent
      },
      {
        path: "system-error",
        canActivate: [AuthGuard],
        component: SystemErrorComponent
      },
      {
        path: "add-entry",
        canActivate: [AuthGuard],
        component: AddEntryComponent
      },
      {
        path: "entries-by-referral",
        canActivate: [AuthGuard],
        component: EntriesByReferralComponent
      },
      {
        path: "alert-list",
        canActivate: [AuthGuard],
        component: AlertListComponent
      },
      {
        path: "add-alert",
        canActivate: [AuthGuard],
        component: AddAlertComponent
      },
      {
        path: "edit-alert/:Id",
        canActivate: [AuthGuard],
        component: EditAlertComponent
      },
      {
        path: "default-report",
        canActivate: [AuthGuard],
        component: DefaultedReportComponent
      },
      {
        path: "main-schedule-list",
        canActivate: [AuthGuard],
        component: MainScheduleListComponent
      },
      {
        path: "add-main-schedule",
        canActivate: [AuthGuard],
        component: AddMainScheduleComponent
      },
      {
        path: "edit-main-schedule/:id",
        canActivate: [AuthGuard],
        component: EditMainScheduleComponent
      },
      {
        path: "game-rules",
        canActivate: [AuthGuard],
        component: GameRulesComponent
      }
    ]
  },
  //No layout
  { path: "login", component: LoginComponent },
  { path: "forgot-password", component: ForgotPasswordComponent },
  { path: "reset-password", component: ResetPasswordComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
