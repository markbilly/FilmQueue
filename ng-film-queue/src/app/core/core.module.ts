import { NgModule } from '@angular/core';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './guards/auth.guard';

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
