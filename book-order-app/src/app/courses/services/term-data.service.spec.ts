import { TestBed, inject } from '@angular/core/testing';

import { TermDataService } from './term-data.service';

describe('TermDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TermDataService]
    });
  });

  it('should be created', inject([TermDataService], (service: TermDataService) => {
    expect(service).toBeTruthy();
  }));
});
