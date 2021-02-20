import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddWeekComponent } from './add-week.component';

describe('AddWeekComponent', () => {
  let component: AddWeekComponent;
  let fixture: ComponentFixture<AddWeekComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddWeekComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddWeekComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
