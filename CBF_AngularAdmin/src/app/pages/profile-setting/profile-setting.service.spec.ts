import { TestBed } from '@angular/core/testing';

import { ProfileSettingService } from './profile-setting.service';

describe('ProfileSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProfileSettingService = TestBed.get(ProfileSettingService);
    expect(service).toBeTruthy();
  });
});
