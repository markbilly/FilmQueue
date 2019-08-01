import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private userManager = new UserManager(getClientSettings());
  private user: User | null;

  constructor() {
    this.userManager.getUser().then(user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  get authorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  getUser() {
    return this.userManager.getUser();
  }

  login() {
    return this.userManager.signinRedirect();
  }

  signout() {
    this.userManager.signoutRedirect();
  }

  async completeAuthentication() {
    this.user = await this.userManager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());      
  }
  
  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  static isAuthenticated(user: User): boolean {
    return user != null && !user.expired;
  }

}

export function getClientSettings(): UserManagerSettings {
  return {
      authority: environment.identityBaseUrl,
      client_id: 'ng-film-queue',
      redirect_uri: environment.baseUrl + '/auth-callback',   
      response_type:"id_token token",
      scope:"openid profile email api.all"
  };
}
