import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';

const routes: Routes = [
  {path:"login",component:LoginComponent},
  {path:"signup",component:SignupComponent},
  {path:"dashboard",component:DashboardComponent, canActivate: [authGuard]},
  {path:"forgotpassword",component:ForgotPasswordComponent},
  {path:"resetpassword",component:ResetPasswordComponent},

  // مسیر پیش‌فرض
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

  // برای مسیرهای اشتباه
  { path: '**', redirectTo: 'login' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
