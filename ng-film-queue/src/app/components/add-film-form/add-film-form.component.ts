import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { WatchNextService } from '../../core/services/watch-next.service';

@Component({
  selector: 'app-add-film-form',
  templateUrl: './add-film-form.component.html',
  styleUrls: ['./add-film-form.component.scss']
})
export class AddFilmFormComponent implements OnInit {

  filmForm = this.formBuilder.group({
    title: ['', Validators.required],
    runtimeInMinutes: ['', Validators.required]
  });

  @Output() submitted: EventEmitter<any> = new EventEmitter();

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
          this.submitted.emit(null);
        });
    }
  }

}
