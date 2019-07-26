import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthCallbackComponent } from './pages/auth-callback/auth-callback.component';
import { CoreModule } from './core/core.module';
import { WatchNextComponent } from './components/watch-next/watch-next.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { WatchlistComponent } from './components/watchlist/watchlist.component';
import { AddFilmComponent } from './pages/add-film/add-film.component';
import { FooterComponent } from './components/footer/footer.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './pages/home/home.component';
import { AddFilmFormComponent } from './components/add-film-form/add-film-form.component';
import { LoginComponent } from './pages/login/login.component';
import { PageShellComponent } from './components/page-shell/page-shell.component';
import { ModalShellComponent } from './components/modal-shell/modal-shell.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthCallbackComponent,
    WatchNextComponent,
    WatchlistComponent,
    AddFilmComponent,
    FooterComponent,
    HomeComponent,
    AddFilmFormComponent,
    LoginComponent,
    PageShellComponent,
    ModalShellComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    FontAwesomeModule,
    CoreModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
