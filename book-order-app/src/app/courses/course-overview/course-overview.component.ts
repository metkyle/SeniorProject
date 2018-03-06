import { Component, OnInit } from '@angular/core';

import { BookOrderModalComponent } from './../partials/book-order-modal/book-order-modal.component';
import { CourseConfirmationModalComponent } from './../partials/course-confirmation-modal/course-confirmation-modal.component';
import { Observable } from 'rxjs/Observable';
import { CourseDataService } from './../services/course-data.service';
import { BookOrderService } from './../services/book-order.service';
import { Course } from './../models/course';
import { Book } from './../models/book';

import * as _ from 'lodash';
import { BookOrderOption } from 'app/courses/models/book-order-option.enum';

@Component({
    selector: 'app-course-overview',
    templateUrl: './course-overview.component.html',
    styleUrls: ['./course-overview.component.css'],
    providers: [BookOrderModalComponent]
})
export class CourseOverviewComponent implements OnInit {

    private coursesAvailable: Array<Course>;
    // private coursesAvailable: Observable<Object>;
    private booksFromCourses: Array<Book>;
    private newBookArray: Array<Book>;
    private courseIdToAdd: string;
    private currentCourse: Course;
    private checkbox0: boolean = false;
    private bookToDelete: Book;
    private courseIdToDelete: string;
    private modalState: string;
    private currentBook: Book;
    private previousBooks: Array<Book> = [];

  constructor(
    private courseDataService: CourseDataService,
    private bookOrderService: BookOrderService
  ) {
  }

  ngOnInit() {
    this.courseDataService.generateCourseDataForInstructor(1).subscribe(response => this.coursesAvailable = response.json());
  }

    addSelectedBook(theBook: Book, theCourse: Course): void {
        theCourse.bookArray.push(theBook);
    }

    setCourseIdForBookAdd(courseId: string): void {
        this.courseIdToAdd = courseId;
    }

    setCurrentCourse(course: Course): void{
        this.currentCourse = course;
        this.courseIdToAdd = course.courseId.toString();
    }

    setCurrentBook(book: Book) {
        this.currentBook = book;
    }

    setModalState(modalState: string): void {
        this.modalState = modalState;
    }

    confirmBookDelete(courseId: string, bookToDelete: Book){
        this.courseIdToDelete = courseId;
        this.bookToDelete = bookToDelete;
    }

    getPreviousBooks(): void {
        this.bookOrderService.getPreviousBooksForCourse(this.currentCourse.courseId).subscribe(response => this.previousBooks = response.json());
    }

    setCourseId(courseId: string) {
      this.courseIdToDelete = courseId;
    }

    cannotSubmit(course:Course): boolean {
        if(course.isSubmitted || (course.myBookOrderOption !== BookOrderOption.noBook && course.bookArray.length === 0)) {
            return true;
        }
        return false;
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
}


