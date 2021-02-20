import { TestBed } from '@angular/core/testing';

import { PickAnalysisService } from './pick-analysis.service';

describe('PickAnalysisService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PickAnalysisService = TestBed.get(PickAnalysisService);
    expect(service).toBeTruthy();
  });
});
