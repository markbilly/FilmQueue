import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import * as _ from 'lodash';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  faPlus = faPlus;
  isVisible: boolean;

  constructor(router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isVisible = this.determineIsVisible(event.url);
      }
    });
  }

  determineIsVisible(url: string) {
    let result: boolean = true;
    url = url.toLowerCase();

    _.forEach(['/add-film', '/login'], path => {
      if (_.includes(url, path)) {
        result = false;
      }
    });

    return result;
  }

  ngOnInit() {
  }

}
