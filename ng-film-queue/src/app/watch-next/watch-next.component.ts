import { Component, OnInit } from '@angular/core';
import { WatchNextService } from '../core/watch-next.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-watch-next',
  templateUrl: './watch-next.component.html',
  styleUrls: ['./watch-next.component.scss']
})
export class WatchNextComponent implements OnInit {

  watchNext = null;

  constructor(private watchNextService: WatchNextService) { }

  ngOnInit() {
    this.watchNextService.getWatchNext().subscribe(result => {
      this.watchNext = result;
    });
  }

}
