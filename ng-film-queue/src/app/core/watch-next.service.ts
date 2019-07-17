import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class WatchNextService {

  apiUrl = 'https://localhost:50506';
  token = null;

  constructor(private http: HttpClient, authService: AuthService) { 
    this.token = authService.authorizationHeaderValue;
  }

  getWatchNext() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': this.token
      })
    };

    return this.http.get(this.apiUrl + '/users/me/watchnext', httpOptions).pipe();
  }

}
