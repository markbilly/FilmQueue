import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';

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

  login() {
    return this.userManager.signinRedirect();
  }

  async completeAuthentication() {
    this.user = await this.userManager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());      
  }
  
  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

}

export function getClientSettings(): UserManagerSettings {
  return {
      authority: 'https://localhost:50505',
      client_id: 'ng-file-queue',
      redirect_uri: 'http://localhost:4200/auth-callback',   
      response_type:"id_token token",
      scope:"openid profile email api.read"
  };
}
