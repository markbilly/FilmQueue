import { Component, OnInit } from '@angular/core';
import { faCheck, faEdit, faPencilAlt } from '@fortawesome/free-solid-svg-icons';
import { NgxSpinnerService } from 'ngx-spinner';
import { WatchNextService } from '../../core/services/watch-next.service';

@Component({
  selector: 'app-watchlist',
  templateUrl: './watchlist.component.html',
  styleUrls: ['./watchlist.component.scss']
})
export class WatchlistComponent implements OnInit {

  busyInit: boolean;
  showWatched = false;

  faCheck = faCheck;

  toWatch = [];
  watched = [];
  watchNextId = null;

  constructor(private watchNextService: WatchNextService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.busyInit = true;
    this.spinner.show();

    this.watchNextService.getWatchlist()
      .then((result: any) => {
        this.toWatch = result.items;
      })
      .finally(() => {
        this.busyInit = false;
        this.spinner.hide();
      });
  }

  setWatchNext(watchNext) {
    this.watchNextId = watchNext.id;
  }

  toggleWatched() {
    if (!this.showWatched && this.watched.length === 0) {
      this.watchNextService.getWatched()
        .then((result: any) => {
          this.watched = result.items;
          this.showWatched = !this.showWatched;
        });
    } else {
      this.showWatched = !this.showWatched;
    }
  }

}
