import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthCallbackComponent } from './pages/auth-callback/auth-callback.component';
import { AuthGuard } from './core/guards/auth.guard';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { AddFilmComponent } from './pages/add-film/add-film.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'auth-callback', component: AuthCallbackComponent },
  // Protected routes
  { canActivate: [AuthGuard], path: 'add-film', component: AddFilmComponent },
  { canActivate: [AuthGuard], path: '', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
