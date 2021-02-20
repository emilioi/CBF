import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultedReportComponent } from './defaulted-report.component';

describe('DefaultedReportComponent', () => {
  let component: DefaultedReportComponent;
  let fixture: ComponentFixture<DefaultedReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultedReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultedReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
