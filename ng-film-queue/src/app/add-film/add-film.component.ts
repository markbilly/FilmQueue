import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormBuilder, Validators } from '@angular/forms';
import { WatchNextService } from '../core/watch-next.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-film',
  templateUrl: './add-film.component.html',
  styleUrls: ['./add-film.component.scss']
})
export class AddFilmComponent implements OnInit {

  filmForm = this.formBuilder.group({
    title: ['', Validators.required],
    runtimeInMinutes: ['', Validators.required]
  });

  constructor(
    private location: Location,
    private formBuilder: FormBuilder,
    private watchNextService: WatchNextService) { }

  ngOnInit() {
  }

  submit() {
    if (this.filmForm.valid) {
      this.watchNextService.addFilm(this.filmForm.value)
        .then(() => {
          this.location.back();
        });
    }
  }

}
