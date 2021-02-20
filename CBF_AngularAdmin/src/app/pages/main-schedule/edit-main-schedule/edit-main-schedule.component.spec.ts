import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMainScheduleComponent } from './edit-main-schedule.component';

describe('EditMainScheduleComponent', () => {
  let component: EditMainScheduleComponent;
  let fixture: ComponentFixture<EditMainScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMainScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMainScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
