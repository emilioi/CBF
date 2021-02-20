import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainScheduleListComponent } from './main-schedule-list.component';

describe('MainScheduleListComponent', () => {
  let component: MainScheduleListComponent;
  let fixture: ComponentFixture<MainScheduleListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MainScheduleListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainScheduleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
