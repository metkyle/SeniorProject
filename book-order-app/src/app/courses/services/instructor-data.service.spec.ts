import { TestBed, inject } from '@angular/core/testing';

import { InstructorDataService } from './instructor-data.service';

describe('InstructorDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InstructorDataService]
    });
  });

  it('should be created', inject([InstructorDataService], (service: InstructorDataService) => {
    expect(service).toBeTruthy();
  }));
});
