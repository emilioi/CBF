import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PickAnalysisComponent } from './pick-analysis.component';

describe('PickAnalysisComponent', () => {
  let component: PickAnalysisComponent;
  let fixture: ComponentFixture<PickAnalysisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PickAnalysisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PickAnalysisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
