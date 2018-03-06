import { Component, OnInit, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

import { Instructor } from './../../models/instructors';
import { Course } from './../../models/course';
import { Book } from './../../models/book';
import { Term } from './../../models/term';
import { BookOrderOption } from './../../models/book-order-option.enum';
import { CourseDataService } from './../../services/course-data.service';

import * as _ from 'lodash';

@Component({
  selector: 'app-course-add-modal',
  templateUrl: './course-add-modal.component.html',
  styleUrls: ['./course-add-modal.component.css']
})
export class CourseAddModalComponent implements OnInit {
  @Input() instructor: Instructor;
  @Input() availableTerms: Array<Term> = [];

  private modalForm: FormGroup;
  constructor(private formbuilder: FormBuilder,
              private courseDataService: CourseDataService,
              private snackBar: MatSnackBar) { 
    
    this.modalForm = formbuilder.group({
      'courseToAdd': formbuilder.group({
          'courseName': '',
          'courseSection': '',
          'courseTerm': '',
          'courseYear': '',
      })
  });
  }

  ngOnInit() {
  }

  submitForm(form: FormGroup): void {
    if(!form.invalid) {
        var course = new Course(this.instructor.instructorCourses.length, form.value.courseToAdd.courseName,
            this.instructor.instructorName, form.value.courseToAdd.courseName, this.instructor.instructorDept,
            form.value.courseToAdd.courseSection, false, BookOrderOption.noSelection, null);

            course.term = form.value.courseToAdd.courseTerm;
            course.year = +(form.value.courseToAdd.courseYear);
            course.bookArray = new Array<Book>();
            // this.instructor.instructorCourses.push(course);
            this.addCourse(course);

    }
  }

  setCourseQuarterAndYear(quarter: string, year: number)
  {
    this.modalForm.value.courseToAdd.courseTerm = quarter;
    this.modalForm.value.courseToAdd.courseYear = year;
  }

  private addCourse(courseToAdd: Course): void {
    try {
        this.courseDataService.saveCourse(courseToAdd, this.instructor.instructorId)
        .subscribe(response =>
          this.instructor.instructorCourses.push(response.json())
        );
    } catch (Exception) {
      //TODO implement error handling
      console.log('course add failed.');
    }
    this.snackBar.open(courseToAdd.getCourseName() + ' added', '', {duration:2000});
    console.log(courseToAdd.courseId);
    // this.clearBookInformation();
  }

  private createCourseFromResponse(courseToAdd: Course, response: string)
  {
    console.log(response);
    console.log(courseToAdd);
  }

  private getInstructorDept(instructor: Instructor): string {
    if(instructor) {
      return instructor.instructorDept;
    }
    return '';
  }
}
