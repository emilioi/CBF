import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMainScheduleComponent } from './add-main-schedule.component';

describe('AddMainScheduleComponent', () => {
  let component: AddMainScheduleComponent;
  let fixture: ComponentFixture<AddMainScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMainScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMainScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
