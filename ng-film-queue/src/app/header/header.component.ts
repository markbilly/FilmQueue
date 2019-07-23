import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../core/auth/auth.service';
import { Router, NavigationEnd } from '@angular/router';
import { Location } from '@angular/common';
import * as _ from 'lodash';
import { faTimes } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  isAuthenticated: boolean;
  subscription: Subscription;
  mode: "page" | "modal";
  faTimes = faTimes;

  constructor(private authService: AuthService, private location: Location, router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.mode = this.determineMode(event.url);
      }
    });
  }

  private determineMode(url: string): "page" | "modal" {
    return _.includes(url, "watchlist") ? "modal" : "page";
  }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => this.isAuthenticated = status);
  }

  signout() {
    this.authService.signout();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
