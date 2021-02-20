import { TestBed } from '@angular/core/testing';

import { WeeklyDeafaultsService } from './weekly-deafaults.service';

describe('WeeklyDeafaultsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WeeklyDeafaultsService = TestBed.get(WeeklyDeafaultsService);
    expect(service).toBeTruthy();
  });
});
