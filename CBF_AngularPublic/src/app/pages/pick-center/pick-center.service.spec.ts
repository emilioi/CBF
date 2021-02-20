import { TestBed } from '@angular/core/testing';

import { PickCenterService } from './pick-center.service';

describe('PickCenterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PickCenterService = TestBed.get(PickCenterService);
    expect(service).toBeTruthy();
  });
});
