import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';

import { Course } from './../models/course';
import { Book } from './../models/book';
import { BookOrderOption } from './../models/book-order-option.enum';

@Injectable()
export class CourseDataService {

    private requestOptions = new RequestOptions({
        headers: new Headers({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        })
    });

    private apiEndpoint: string = 'api/courseprocessor';
    private courses: Course[];
  constructor(private http: Http) {
  }

  testApiEndpoint(): any {
      this.http.get(this.apiEndpoint, this.requestOptions).
          subscribe(response => console.log(response.json()));
  }

  courseSubmitUpdate(id: number): Observable<Response> {
    return this.http.put(this.apiEndpoint + "/" + id, this.requestOptions);
  }

generateCourseDataForInstructor(instructorId: number): Observable<Response> {
    return this.http.get(this.apiEndpoint + "/" + instructorId, this.requestOptions);
  }

  deleteCourse(courseId: number): Observable<Response>
  {
    return this.http.delete(this.apiEndpoint, new RequestOptions({
        headers: this.generateRequestHeaders(),
        body: JSON.stringify({'courseId': courseId})
      }));
  }

  deleteBook(bookISBN: string, courseId: number): Observable<Response> {
    return this.http.delete(this.apiEndpoint, new RequestOptions({
      headers: this.generateRequestHeaders(),
      body: JSON.stringify({'bookISBN': bookISBN, 'courseId': courseId})
    }));
  }

  saveCourse(course: Course, instructorId: number): Observable<Response> {
    return this.http.post(this.apiEndpoint, JSON.stringify({ InstructorId: instructorId, Course: course }), this.requestOptions);
  }

  setCourseSubmitted(course: Course): Observable<Response>{
    return this.http.put(this.apiEndpoint, JSON.stringify({Course: course}), this.requestOptions);
  }

  private generateRequestHeaders(): Headers {
    return new Headers({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
  }
}
