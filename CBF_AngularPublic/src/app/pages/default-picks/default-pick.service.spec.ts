import { TestBed } from '@angular/core/testing';

import { DefaultPickService } from './default-pick.service';

describe('DefaultPickService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DefaultPickService = TestBed.get(DefaultPickService);
    expect(service).toBeTruthy();
  });
});
