import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { AuthGuard } from './core/auth/auth.guard';
import { LoginComponent } from './account/login/login.component';
import { WatchNextComponent } from './watch-next/watch-next.component';


const routes: Routes = [
  { path: '', component: WatchNextComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'auth-callback', component: AuthCallbackComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
