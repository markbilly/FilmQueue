import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { CoreModule } from './core/core.module';
import { AccountModule } from './account/account.module';
import { HeaderComponent } from './header/header.component';
import { WatchNextComponent } from './watch-next/watch-next.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { WatchlistComponent } from './watchlist/watchlist.component';
import { AddFilmComponent } from './add-film/add-film.component';
import { FooterComponent } from './footer/footer.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    AuthCallbackComponent,
    HeaderComponent,
    WatchNextComponent,
    WatchlistComponent,
    AddFilmComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    FontAwesomeModule,
    CoreModule,
    AccountModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
