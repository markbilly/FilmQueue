import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WatchNextComponent } from './watch-next.component';

describe('WatchNextComponent', () => {
  let component: WatchNextComponent;
  let fixture: ComponentFixture<WatchNextComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WatchNextComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WatchNextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
