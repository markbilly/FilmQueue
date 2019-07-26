import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFilmFormComponent } from './add-film-form.component';

describe('AddFilmFormComponent', () => {
  let component: AddFilmFormComponent;
  let fixture: ComponentFixture<AddFilmFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddFilmFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddFilmFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
