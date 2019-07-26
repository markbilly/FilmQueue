import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private spinner: NgxSpinnerService) { }
    
  login() {     
    this.spinner.show();
    this.authService.login();
  }   

  ngOnInit() {
  }

}
