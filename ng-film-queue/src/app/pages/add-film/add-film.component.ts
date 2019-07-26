import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-add-film',
  templateUrl: './add-film.component.html',
  styleUrls: ['./add-film.component.scss']
})
export class AddFilmComponent implements OnInit {
  
  constructor(public location: Location) { }

  ngOnInit() {
  }

}
