import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClubHouseComponent } from './club-house.component';

describe('ClubHouseComponent', () => {
  let component: ClubHouseComponent;
  let fixture: ComponentFixture<ClubHouseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClubHouseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClubHouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
