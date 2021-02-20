import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SportTypeListComponent } from './sport-type-list.component';

describe('SportTypeListComponent', () => {
  let component: SportTypeListComponent;
  let fixture: ComponentFixture<SportTypeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SportTypeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SportTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
