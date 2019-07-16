import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './auth/auth.service';
import { AuthGuard } from './auth/auth.guard';

@NgModule({
  declarations: [],
  imports: [
  ],
  providers: [
    AuthService,
    AuthGuard
  ]
})
export class CoreModule { }
