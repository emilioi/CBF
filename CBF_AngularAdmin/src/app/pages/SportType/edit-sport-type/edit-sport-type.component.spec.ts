import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditSportTypeComponent } from './edit-sport-type.component';

describe('EditSportTypeComponent', () => {
  let component: EditSportTypeComponent;
  let fixture: ComponentFixture<EditSportTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditSportTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditSportTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
