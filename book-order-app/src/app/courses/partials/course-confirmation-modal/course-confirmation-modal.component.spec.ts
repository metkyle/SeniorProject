import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseConfirmationModalComponent } from './course-confirmation-modal.component';

describe('CourseConfirmationModalComponent', () => {
  let component: CourseConfirmationModalComponent;
  let fixture: ComponentFixture<CourseConfirmationModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseConfirmationModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseConfirmationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
