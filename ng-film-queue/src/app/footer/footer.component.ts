import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  faPlus = faPlus;

  constructor(private router: Router) { }

  get isVisible() {
    return !['/add-film', '/login'].includes(this.router.url.toLowerCase())
  }

  ngOnInit() {
  }

}
