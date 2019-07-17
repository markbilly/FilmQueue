import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { CoreModule } from './core/core.module';
import { AccountModule } from './account/account.module';
import { HeaderComponent } from './header/header.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthCallbackComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    AccountModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
