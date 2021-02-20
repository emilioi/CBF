import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketEliminatedComponent } from './ticket-eliminated.component';

describe('TicketEliminatedComponent', () => {
  let component: TicketEliminatedComponent;
  let fixture: ComponentFixture<TicketEliminatedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketEliminatedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketEliminatedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
