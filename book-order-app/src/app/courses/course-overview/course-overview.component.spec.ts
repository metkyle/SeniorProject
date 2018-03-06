import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseOverviewComponent } from './course-overview.component';
import { CourseDataService } from '../services/course-data.service';

describe('CourseOverviewComponent', () => {
  let component: CourseOverviewComponent;
  let fixture: ComponentFixture<CourseOverviewComponent>;


  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseOverviewComponent ],
      providers: [ CourseDataService ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseOverviewComponent);    
    component = fixture.componentInstance;
    fixture.detectChanges();    
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
