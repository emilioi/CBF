import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketAliveComponent } from './ticket-alive.component';

describe('TicketAliveComponent', () => {
  let component: TicketAliveComponent;
  let fixture: ComponentFixture<TicketAliveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketAliveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketAliveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
