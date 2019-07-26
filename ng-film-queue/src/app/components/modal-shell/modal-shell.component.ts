import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { faCheck } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-modal-shell',
  templateUrl: './modal-shell.component.html',
  styleUrls: ['./modal-shell.component.scss']
})
export class ModalShellComponent implements OnInit {

  faCheck = faCheck;

  @Output() accepted: EventEmitter<any> = new EventEmitter();
  @Output() dismissed: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

}
