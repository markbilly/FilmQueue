import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class WatchNextService {

  apiUrl = 'https://localhost:50506';
  token = null;

  constructor(private http: HttpClient, authService: AuthService) { 
    this.token = authService.authorizationHeaderValue;
  }

  get httpOptions() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.token
      })
    };
  };

  getWatchNext() {
    return this.http.get(this.apiUrl + '/users/me/watchnext', this.httpOptions).toPromise();
  }

  selectWatchNext() {
    return this.http.put(this.apiUrl + "/users/me/watchnext", { selectRandomFilm: true }, this.httpOptions).toPromise();
  }

  skipWatchNext() {
    return this.http.delete(this.apiUrl + '/users/me/watchnext', this.httpOptions).toPromise();
  }

  // TODO: Move to a film service
  setAsWatched(id: number) {
    return this.http.put(this.apiUrl + "/users/me/films/" + id + "/watched", { watched: true }, this.httpOptions).toPromise();
  }

  getWatchlist() {
    return this.http.get(this.apiUrl + "/users/me/watchlist/?page=1&pageSize=100", this.httpOptions).toPromise();
  }

  getWatched() {
    return this.http.get(this.apiUrl + "/users/me/watchedfilms/?page=1&pageSize=100", this.httpOptions).toPromise();
  }

  addFilm(film: Object) {
    return this.http.post(this.apiUrl + "/users/me/films", film, this.httpOptions).toPromise();
  }

}
