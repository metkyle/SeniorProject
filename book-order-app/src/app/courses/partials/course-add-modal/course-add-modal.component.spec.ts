import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseAddModalComponent } from './course-add-modal.component';

describe('CourseAddModalComponent', () => {
  let component: CourseAddModalComponent;
  let fixture: ComponentFixture<CourseAddModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseAddModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
