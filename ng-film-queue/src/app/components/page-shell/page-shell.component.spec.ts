import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageShellComponent } from './page-shell.component';

describe('PageShellComponent', () => {
  let component: PageShellComponent;
  let fixture: ComponentFixture<PageShellComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageShellComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
