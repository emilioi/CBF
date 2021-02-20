import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWeekComponent } from './edit-week.component';

describe('EditWeekComponent', () => {
  let component: EditWeekComponent;
  let fixture: ComponentFixture<EditWeekComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditWeekComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditWeekComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
