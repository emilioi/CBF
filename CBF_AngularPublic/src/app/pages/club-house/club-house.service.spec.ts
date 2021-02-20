import { TestBed } from '@angular/core/testing';

import { ClubHouseService } from './club-house.service';

describe('ClubHouseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClubHouseService = TestBed.get(ClubHouseService);
    expect(service).toBeTruthy();
  });
});
