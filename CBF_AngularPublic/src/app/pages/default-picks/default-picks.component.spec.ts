import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultPicksComponent } from './default-picks.component';

describe('DefaultPicksComponent', () => {
  let component: DefaultPicksComponent;
  let fixture: ComponentFixture<DefaultPicksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultPicksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultPicksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
