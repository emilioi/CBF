import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MoveTicketComponent } from './move-ticket.component';

describe('MoveTicketComponent', () => {
  let component: MoveTicketComponent;
  let fixture: ComponentFixture<MoveTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MoveTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
