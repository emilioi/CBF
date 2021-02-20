import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevControlComponent } from './dev-control.component';

describe('DevControlComponent', () => {
  let component: DevControlComponent;
  let fixture: ComponentFixture<DevControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevControlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
