import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Rules2Component } from './rules2.component';

describe('Rules2Component', () => {
  let component: Rules2Component;
  let fixture: ComponentFixture<Rules2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Rules2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Rules2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
