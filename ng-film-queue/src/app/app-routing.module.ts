import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { AuthGuard } from './core/auth/auth.guard';
import { LoginComponent } from './account/login/login.component';


const routes: Routes = [
  { path: '', canActivate: [AuthGuard], canActivateChild: [AuthGuard], children: [
    
  ]},
  { path: 'login', component: LoginComponent },
  { path: 'auth-callback', component: AuthCallbackComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
