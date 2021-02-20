import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportNavbarComponent } from './report-navbar.component';

describe('ReportNavbarComponent', () => {
  let component: ReportNavbarComponent;
  let fixture: ComponentFixture<ReportNavbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportNavbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
