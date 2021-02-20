import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeeklyDeafaultsComponent } from './weekly-deafaults.component';

describe('WeeklyDeafaultsComponent', () => {
  let component: WeeklyDeafaultsComponent;
  let fixture: ComponentFixture<WeeklyDeafaultsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeeklyDeafaultsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeeklyDeafaultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
