import { Component, OnInit, EventEmitter } from '@angular/core';
import { Output } from '@angular/core/src/metadata/directives';
import { MatSnackBar } from '@angular/material';

import { Term } from '../courses/models/term';
import { Instructor } from '../courses/models/instructors';
import { Course } from '../courses/models/course';
import { Book } from '../courses/models/book';
import { BookOrderOption } from '../courses/models/book-order-option.enum';
import { InstructorDataService } from './../courses/services/instructor-data.service';
import { TermDataService } from './../courses/services/term-data.service';
import { CourseDataService } from 'app/courses/services/course-data.service';
import { BookOrderService } from './../courses/services/book-order.service';

import { BookOrderModalComponent } from './../courses/partials/book-order-modal/book-order-modal.component';
import { CourseConfirmationModalComponent } from './../courses/partials/course-confirmation-modal/course-confirmation-modal.component';
import { CourseAddModalComponent } from './../courses/partials/course-add-modal/course-add-modal.component';

import { BookOrderAppPage } from '../../../e2e/app.po';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
  providers: [BookOrderModalComponent, CourseConfirmationModalComponent, CourseAddModalComponent]
})
export class AdminComponent implements OnInit {
  private departmentInstructors: Array<Instructor>;
  private currentTerms: Array<Term> = new Array<Term>();
  private coursesAvailable: Array<Course>;
  private previousBooks: Array<Book> = [];
  private courseId: number;
  private currentInstructor: Instructor;
  private currentCourse: Course;
  private currentBook: Book;
  private modalState: string;
  private confirmModalState: string;
  private termToDelete: Term;

  constructor(private instructorDataService: InstructorDataService,
              private termDataService: TermDataService,
              private bookOrderService: BookOrderService,
              private courseDataService: CourseDataService,
              private snackBar: MatSnackBar
              ) {
   }

  ngOnInit() {
    
    //TODO
    //0 hardcoded. Once dept ID's are implemented, use those
    this.instructorDataService.generateInstructorsForDeptId(0).subscribe(response => this.departmentInstructors = response.json());
    this.termDataService.generateCurrentAvailableTerms(0).subscribe(response => this.currentTerms = response.json());

  }

  setCurrentInstructor(currentInstructor: Instructor){
    this.currentInstructor = currentInstructor;
    this.coursesAvailable = this.currentInstructor.instructorCourses;
  }

  setCurrentCourse(currentCourse: Course){
    this.currentCourse = currentCourse;
    this.courseId = this.currentCourse.courseId;
  }

  setCurrentBook(currentBook: Book){
    if(currentBook == null) {
      this.currentBook = new Book();
    }
    else {
      this.currentBook = currentBook;
    }
  }

  setTermToDelete(termToDelete: Term) {
    this.termToDelete = termToDelete;
  }

  setModalState(state: string) {
    this.modalState = state;
  }

  setConfirmModalState(state: string) {
    this.confirmModalState = state;
  }

  noBookRequired(course: Course) {
    return course.myBookOrderOption == BookOrderOption.noBook;
  }

  noBookRequiredChecked(course: Course) {
    if(course.myBookOrderOption == BookOrderOption.noBook) {
      if(course.bookArray.length > 0) {
        course.myBookOrderOption = BookOrderOption.hasBook;
      }
      else {
        course.myBookOrderOption = BookOrderOption.noSelection;
      }
    }
    else {
      course.myBookOrderOption = BookOrderOption.noBook;
    }
  }

  stringifySubmittedCoursesForInstructor(instructor: Instructor): string {
      var submittedCount = 0;
  
      for(let course of instructor.instructorCourses) {
        if(course.isSubmitted) {
          submittedCount++;
        }
      }
      return submittedCount + "/" + instructor.instructorCourses.length;    
    }

    stringifyCourseInfo(course: Course): string {
        return course.department + ' ' + course.courseNumber + '-' + course.section + ' ' + course.term + ' ' + course.year;
    }
  
    instructorHasSubmittedAllCourses(instructor: Instructor): boolean {
        var submitted = true;
        if(instructor.instructorCourses.length == 0) {
          return false;
        }
  
        for(let course of instructor.instructorCourses) {
          if(!course.isSubmitted) {
            submitted = false;
            break;
          }
        }
        return submitted;
    }

    getBookStatusMessage(course: Course): string {
      if(course.myBookOrderOption == BookOrderOption.noBook) {
        return "This course does not require a book"
      }
      return null;
    }

    canSubmit(course: Course): boolean {
      if(course.myBookOrderOption == BookOrderOption.noBook || course.bookArray.length > 0) {
        return false;
      }
      return true;
    }

    getPreviousBooks(): void {
      this.bookOrderService.getPreviousBooksForCourse(this.courseId).subscribe(response => this.previousBooks = response.json());
  }

  confirmChanges(): void {
    var hasError = false;
    for(let instructor of this.departmentInstructors) {
      for(let course of instructor.instructorCourses) {
        try {
          this.courseDataService.setCourseSubmitted(course)
          .subscribe(success => { }, error => hasError = true);
        }
        catch {
          //TODO handle
        }

        for(let book of course.bookArray) {
          try {
            this.bookOrderService.editBook(book, course.courseId, book.bookISBN)
            .subscribe(success => { }, error => hasError = true);
          }
          catch {
            //TODO handle
          }
        }
      }

      this.instructorDataService.generateInstructorsForDeptId(0).subscribe(response => this.departmentInstructors = response.json());
      this.termDataService.generateCurrentAvailableTerms(0).subscribe(response => this.currentTerms = response.json());
    }

    if(!hasError) {
      this.snackBar.open('Changes submitted', '', {duration:2000});
    }
    else {
     this.snackBar.open('Failed to submit changes', '', {duration:2000});
    }
  }
}
