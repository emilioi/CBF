import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntriesByReferralComponent } from './entries-by-referral.component';

describe('EntriesByReferralComponent', () => {
  let component: EntriesByReferralComponent;
  let fixture: ComponentFixture<EntriesByReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntriesByReferralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntriesByReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
