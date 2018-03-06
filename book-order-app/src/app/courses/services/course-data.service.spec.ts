import { TestBed, inject } from '@angular/core/testing';

import { CourseDataService } from './course-data.service';

describe('CourseDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CourseDataService]
    });
  });

  it('should be created', inject([CourseDataService], (service: CourseDataService) => {
    expect(service).toBeTruthy();
  }));

  it('should return a collection of courses', inject([CourseDataService], (service: CourseDataService) => {
    expect(service.generateCourseDataForInstructor(1).length).toBeGreaterThan(0);
  }));

  it('should return a collection of initialized courses', inject([CourseDataService], (service: CourseDataService) => {
    expect(service.generateCourseDataForInstructor(1)[1]).toBeDefined();
  }));
});
