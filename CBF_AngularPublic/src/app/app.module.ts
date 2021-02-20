import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ProfileSettingComponent } from './pages/profile-setting/profile-setting.component';
import { DeleteMsgComponent } from './pages/delete-msg/delete-msg.component';
import { SiteLayoutComponent } from './_layout/site-layout/site-layout.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SiteHeaderComponent } from './_layout/site-header/site-header.component';
import { SiteSidebarComponent } from './_layout/site-sidebar/site-sidebar.component';
import { SiteFooterComponent } from './_layout/site-footer/site-footer.component';
import { AuthGuard } from './utility/auth.gaurds';
import { Config } from './utility/config';
import { SignupComponent } from './pages/signup/signup.component';
import { ClubHouseComponent } from './pages/club-house/club-house.component';
import { NotificationBarComponent } from './_layout/notification-bar/notification-bar.component';
import { TicketComponent } from './pages/ticket/ticket.component';
import { PickCenterComponent } from './pages/pick-center/pick-center.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { RulesComponent } from './pages/rules/rules.component';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { Rules2Component } from './pages/rules2/rules2.component';
import { PickAnalysisComponent } from './pages/pick-analysis/pick-analysis.component';
import { TicketAliveComponent } from './pages/ticket-alive/ticket-alive.component';
import { TicketEliminatedComponent } from './pages/ticket-eliminated/ticket-eliminated.component';
import { ScrollEventModule } from 'ngx-scroll-event';
import { DefaultPicksComponent } from './pages/default-picks/default-picks.component';
import { ReportNavbarComponent } from './pages/controls/report-navbar/report-navbar.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ForgotPasswordComponent,
    ProfileComponent,
    ProfileSettingComponent,
    DeleteMsgComponent,
    SiteLayoutComponent,
    SiteHeaderComponent,
    SiteSidebarComponent,
    SiteFooterComponent,
    SignupComponent,
    ClubHouseComponent,
    NotificationBarComponent,
    TicketComponent,
    PickCenterComponent,
    ResetPasswordComponent,
    RulesComponent,
    Rules2Component,
    PickAnalysisComponent,
    TicketAliveComponent,
    TicketEliminatedComponent,
    DefaultPicksComponent,
    ReportNavbarComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxUiLoaderModule,ScrollEventModule
  ],
  providers:[Config, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
