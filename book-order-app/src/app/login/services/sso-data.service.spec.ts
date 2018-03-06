import { TestBed, inject } from '@angular/core/testing';

import { SsoDataService } from './sso-data.service';

describe('SsoDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SsoDataService]
    });
  });

  it('should be created', inject([SsoDataService], (service: SsoDataService) => {
    expect(service).toBeTruthy();
  }));
});
