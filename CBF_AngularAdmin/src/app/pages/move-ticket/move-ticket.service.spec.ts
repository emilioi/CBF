import { TestBed } from '@angular/core/testing';

import { MoveTicketService } from './move-ticket.service';

describe('MoveTicketService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MoveTicketService = TestBed.get(MoveTicketService);
    expect(service).toBeTruthy();
  });
});
