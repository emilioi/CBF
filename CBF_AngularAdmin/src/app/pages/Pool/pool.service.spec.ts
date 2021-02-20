import { TestBed } from '@angular/core/testing';

import { PoolService } from './pool.service';

describe('PoolService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PoolService = TestBed.get(PoolService);
    expect(service).toBeTruthy();
  });
});
