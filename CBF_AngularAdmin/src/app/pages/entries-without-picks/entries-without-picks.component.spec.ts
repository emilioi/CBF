import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntriesWithoutPicksComponent } from './entries-without-picks.component';

describe('EntriesWithoutPicksComponent', () => {
  let component: EntriesWithoutPicksComponent;
  let fixture: ComponentFixture<EntriesWithoutPicksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntriesWithoutPicksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntriesWithoutPicksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
