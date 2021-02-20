import { TestBed } from '@angular/core/testing';

import { SportTypeService } from './sport-type.service';

describe('SportTypeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SportTypeService = TestBed.get(SportTypeService);
    expect(service).toBeTruthy();
  });
});
