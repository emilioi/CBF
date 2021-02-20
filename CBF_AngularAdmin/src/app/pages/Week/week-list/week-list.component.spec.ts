import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeekListComponent } from './week-list.component';

describe('WeekListComponent', () => {
  let component: WeekListComponent;
  let fixture: ComponentFixture<WeekListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeekListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeekListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
