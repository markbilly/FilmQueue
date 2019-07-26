import { Component, OnInit } from '@angular/core';
import { WatchNextService } from '../../core/services/watch-next.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import * as _ from "lodash";

@Component({
  selector: 'app-watch-next',
  templateUrl: './watch-next.component.html',
  styleUrls: ['./watch-next.component.scss']
})
export class WatchNextComponent implements OnInit {

  watchNext = null;
  hasUnwatchedFilms: boolean;

  busyInit: boolean;

  faCheck = faCheck;

  constructor(private watchNextService: WatchNextService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.busyInit = true;
    this.spinner.show();

    this.watchNextService.getWatchlist()
      .then((watchlist: any) => {
        this.hasUnwatchedFilms = _.isFinite(watchlist.totalItems) && watchlist.totalItems > 1;
        return this.watchNextService.getWatchNext();
      })    
      .then(watchNext => {
        this.watchNext = watchNext;
      })
      .catch(error => {
        if (error.status === 404) {
          this.watchNextService.selectWatchNext()
            .then(watchNext => {
              this.watchNext = watchNext;
            })
            .catch(() => {
              this.watchNext = null;
            });
        }
      })
      .finally(() => {
        this.spinner.hide();
        this.busyInit = false;
      });
  }

  next() {
    this.watchNextService.setAsWatched(this.watchNext.id)
      .then(() => {
        this.ngOnInit();
      });
  }

  skip() {
    this.watchNextService.skipWatchNext()
      .then(() => {
        this.ngOnInit();
      });
  }

}
