import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PickCenterComponent } from './pick-center.component';

describe('PickCenterComponent', () => {
  let component: PickCenterComponent;
  let fixture: ComponentFixture<PickCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PickCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PickCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
