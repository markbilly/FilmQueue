import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-page-shell',
  templateUrl: './page-shell.component.html',
  styleUrls: ['./page-shell.component.scss']
})
export class PageShellComponent implements OnInit, OnDestroy {

  isAuthenticated: boolean;
  subscription: Subscription;

  constructor(private authService: AuthService) { }

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
