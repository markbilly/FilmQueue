import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { WatchNextService } from '../core/watch-next.service';

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

  @Output() filmAdded: EventEmitter<any> = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    private watchNextService: WatchNextService) { }

  ngOnInit() {
  }

  submit() {
    if (this.filmForm.valid) {
      this.watchNextService.addFilm(this.filmForm.value)
        .then(() => {
          this.filmForm.reset();
          this.filmAdded.emit(null);
        });
    }
  }

}
