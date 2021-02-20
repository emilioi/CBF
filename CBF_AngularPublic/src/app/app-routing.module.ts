import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { LoginComponent } from "./pages/login/login.component";
import { SiteLayoutComponent } from "./_layout/site-layout/site-layout.component";
import { ForgotPasswordComponent } from "./pages/forgot-password/forgot-password.component";
import { DeleteMsgComponent } from "./pages/delete-msg/delete-msg.component";
import { ProfileComponent } from "./pages/profile/profile.component";
import { ProfileSettingComponent } from "./pages/profile-setting/profile-setting.component";
import { SignupComponent } from "./pages/signup/signup.component";
import { ClubHouseComponent } from "./pages/club-house/club-house.component";
import { TicketComponent } from "./pages/ticket/ticket.component";
import { PickCenterComponent } from "./pages/pick-center/pick-center.component";
import { ResetPasswordComponent } from "./pages/reset-password/reset-password.component";
import { AuthGuard } from "./utility/auth.gaurds";
import { RulesComponent } from "./pages/rules/rules.component";
import { Rules2Component } from './pages/rules2/rules2.component';
import { PickAnalysisComponent } from './pages/pick-analysis/pick-analysis.component';
import { TicketAliveComponent } from './pages/ticket-alive/ticket-alive.component';
import { TicketEliminatedComponent } from './pages/ticket-eliminated/ticket-eliminated.component';
import { DefaultPicksComponent } from './pages/default-picks/default-picks.component';

const routes: Routes = [
  { path: "", component: LoginComponent },
  //{ path: 'dashboard', canActivate: [AuthGuard], component: DashboardComponent },

  // After Login Layout
  {
    path: "",
    //canActivate:[AuthGuard],
    component: SiteLayoutComponent,
    children: [
      {
        path: "profile",
        canActivate: [AuthGuard],
        component: ProfileComponent
      },
      {
        path: "profile-setting",
        canActivate: [AuthGuard],
        component: ProfileSettingComponent
      },
      {
        path: "delete-msg",
        canActivate: [AuthGuard],
        component: DeleteMsgComponent
      },
      {
        path: "club-house",
        canActivate: [AuthGuard],
        component: ClubHouseComponent
      },
      {
        path: "pick-analysis/:PoolId",
        canActivate: [AuthGuard],
        component: PickAnalysisComponent
      },
      {
        path: "ticket-alive/:PoolId",
        canActivate: [AuthGuard],
        component: TicketAliveComponent
      },
      {
        path: "ticket-eliminated/:PoolId",
        canActivate: [AuthGuard],
        component: TicketEliminatedComponent
      },
      { path: "ticket/:PoolId", canActivate: [AuthGuard], component: TicketComponent },
      { path: "default-picks/:PoolId", canActivate: [AuthGuard], component: DefaultPicksComponent },
      {
        path: "pick-center/:ticketId",
        canActivate: [AuthGuard],
        component: PickCenterComponent
      },
      { path: "rule", canActivate: [AuthGuard], component: Rules2Component }
    ]
  },
  //Login layout
  { path: "login", component: LoginComponent },
  { path: "forgot-password", component: ForgotPasswordComponent },
  { path: "signup", component: SignupComponent },
  { path: "reset-password", component: ResetPasswordComponent },
  { path: "rules",  component: RulesComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
