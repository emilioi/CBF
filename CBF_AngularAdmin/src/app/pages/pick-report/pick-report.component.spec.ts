import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PickReportComponent } from './pick-report.component';

describe('PickReportComponent', () => {
  let component: PickReportComponent;
  let fixture: ComponentFixture<PickReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PickReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PickReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
