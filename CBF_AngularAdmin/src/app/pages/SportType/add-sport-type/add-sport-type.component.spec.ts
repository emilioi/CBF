import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSportTypeComponent } from './add-sport-type.component';

describe('AddSportTypeComponent', () => {
  let component: AddSportTypeComponent;
  let fixture: ComponentFixture<AddSportTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddSportTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddSportTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
