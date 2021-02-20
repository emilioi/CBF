import { TestBed } from '@angular/core/testing';

import { DevelopmentService } from './development.service';

describe('DevelopmentService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));
  it('should be created', () => {
    const service: DevelopmentService = TestBed.get(DevelopmentService);
    expect(service).toBeTruthy();
  });
});
