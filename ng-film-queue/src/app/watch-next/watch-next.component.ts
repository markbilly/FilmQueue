import { Component, OnInit } from '@angular/core';
import { WatchNextService } from '../core/watch-next.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators';
import { faCheck, faChevronRight } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-watch-next',
  templateUrl: './watch-next.component.html',
  styleUrls: ['./watch-next.component.scss']
})
export class WatchNextComponent implements OnInit {

  watchNext = null;
  
  busyInit: boolean;
  busyNext: boolean;

  faCheck = faCheck;
  faChevronRight = faChevronRight;

  constructor(private watchNextService: WatchNextService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.busyInit = true;
    this.spinner.show();
    this.getCurrent();
  }

  next() {
    this.busyNext = true;
    this.watchNextService.setAsWatched(this.watchNext.id)
      .then(() => {
        return this.watchNextService.selectWatchNext();
      })
      .then(result => {
        this.watchNext = result;
      })
      .catch(() => {
        this.getCurrent();
      })
      .finally(() => {
        this.busyNext = false;
      });
  }

  private getCurrent() {
    this.watchNextService.getWatchNext()
      .then(result => {
        this.watchNext = result;
      })
      .catch(error => {
        if (error.status === 404) {
          this.watchNext = null;
        }
      })
      .finally(() => {
        this.spinner.hide();
        this.busyInit = false;
      });
  }

}
