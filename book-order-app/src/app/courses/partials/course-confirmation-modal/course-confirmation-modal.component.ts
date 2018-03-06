import { Component, OnInit, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material';

import { Instructor } from './../../models/instructors';
import { Course } from './../../models/course';
import { Book } from './../../models/book';
import { Term } from './../../models/term';
import { CourseDataService } from './../../services/course-data.service';
import { BookOrderService } from './../../services/book-order.service';
import { TermDataService } from './../../services/term-data.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-course-confirmation-modal',
  templateUrl: './course-confirmation-modal.component.html',
  styleUrls: ['./course-confirmation-modal.component.css']
})
export class CourseConfirmationModalComponent implements OnInit {

    @Input() coursesAvailable: Array<Course> = [];
    @Input() availableTerms: Array<Term> = [];
    @Input() bookToDelete: Book;
    @Input() courseId: number;

    @Input() courseToDelete: Course;
    @Input() currentInstructor: Instructor;
    @Input() termToDelete: Term;

    @Input() modalState: string;

    constructor(private bookOrderService: BookOrderService,
                private courseDataService: CourseDataService,
                private termDataService: TermDataService,
                private snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  deleteSelectedBook(): void {
    try{
      var bookName = this.bookToDelete.bookTitle;
      this.bookOrderService.deleteBook(this.bookToDelete.bookISBN, this.courseId).
      subscribe(response =>
          _.remove(_.find(this.coursesAvailable, (course) => { return course.courseId === this.courseId; }).bookArray,
              book => { return book.bookISBN === this.bookToDelete.bookISBN; }));
      this.snackBar.open('Removed ' + bookName + ' from ' + this.courseToDelete.getCourseName(), '', {duration : 20000});
    } catch (Exception){
      //TODO implement error handling
    }
  }

  deleteSelectedCourse(): void {
    try{
      var courseName = this.courseToDelete.courseNumber;
      this.courseDataService.deleteCourse(this.courseToDelete.courseId).
      subscribe(response =>
          _.remove(this.currentInstructor.instructorCourses,
              (course) => { return course.courseNumber === this.courseToDelete.courseNumber; }));
      this.snackBar.open('Removed ' + this.courseToDelete.getCourseName(), '', {duration : 2000});
    } catch (Exception){
      //TODO implement error handling
    }
  }

  deleteSelectedTerm(): void {
    try{
      //TODO remove hard coded dept id
      this.termDataService.deleteTerm(0, this.termToDelete).subscribe(response =>
        _.remove(this.availableTerms, (term) =>
        {
          return term.quarter === this.termToDelete.quarter && term.year === this.termToDelete.year
        }));

      this.snackBar.open('Removed ' + this.termToDelete.quarter + ' ' + this.termToDelete.year, '', {duration : 2000});
    } catch (Exception){
      //TODO implement error handling
    }
  }

  getConfirmMessage(): string {
    if(this.modalState == 'deleteBook')
    {
      return 'Are you sure you wish to remove this book?';
    }
    else if(this.modalState == 'submitBooks')
    {
      return 'Are you sure you are ready to submit?';
    }
    else if(this.modalState == 'deleteCourse')
    {
      return 'Are you sure you wish to remove this course?';

    }
    else if(this.modalState == 'deleteTerm')
    {
      return 'Are you sure you wish to remove this term?';
    }
  }

  getContentToDisplay(): string {
    if(this.modalState == 'deleteBook')
    {
      return this.bookToDelete.bookTitle;
    }
    else if(this.modalState == 'submitBooks')
    {
      return null;
    }
    else if(this.modalState == 'deleteCourse')
    {
      return this.courseToDelete.courseNumber;
    }
    else if(this.modalState == 'deleteTerm')
    {
      return this.termToDelete.quarter + ' ' + this.termToDelete.year;
    }
  }

  onConfirmClicked()
  {
    if(this.modalState == 'deleteBook')
    {
      this.deleteSelectedBook();
    }
    else if(this.modalState == 'submitBooks')
    {
      this.courseToDelete.isSubmitted = true;
      this.courseSubmitUpdate(+this.courseId);
    }
    else if(this.modalState == 'deleteCourse')
    {
      this.deleteSelectedCourse();
    }
    else if(this.modalState == 'deleteTerm')
    {
      this.deleteSelectedTerm();
    }
  }

  courseSubmitUpdate(id: number): void
  {
    try {
      this.courseDataService.setCourseSubmitted(this.courseToDelete)
        .subscribe(response => _.find(this.coursesAvailable, (course) => {
          return course.courseId === this.courseToDelete.courseId;
        }));
      for (let course of this.coursesAvailable) {
        if (course.courseId == this.courseId)
          var theCourse = course;

        course.isSubmitted = true;
      }
      this.snackBar.open('Submitted Books For ' + this.courseToDelete.department + ' ' + this.courseToDelete.courseNumber, '', { duration: 2000 });
    } catch (Exception) {
      //TODO implement error handling}
    }

  }

}
