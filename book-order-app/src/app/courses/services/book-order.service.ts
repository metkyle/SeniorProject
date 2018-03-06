import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';

import { Book } from './../models/book';

@Injectable()
export class BookOrderService {

  private requestOptions = new RequestOptions({
    headers: this.generateRequestHeaders()
});

  private apiEndpoint: string = 'api/bookprocessor';
  constructor(private http: Http) { }

  saveBook(book: Book, courseId: number): Observable<Response> {
    return this.http.post(this.apiEndpoint, JSON.stringify({ CourseId: courseId, Book: book }), this.requestOptions);
  }

  editBook(book: Book, courseId: number, originalISBN: string): Observable<Response> {
    return this.http.put(this.apiEndpoint, JSON.stringify({ CourseId: courseId, Book: book, OriginalISBN: originalISBN }),
      this.requestOptions);
  }

  getPreviousBooksForCourse(courseId: number): Observable<Response> {
    return this.http.get(this.apiEndpoint + "/getpreviousbooksforcourse/" + courseId, this.requestOptions);
  }

  deleteBook(bookISBN: string, courseId: number): Observable<Response> {
    return this.http.delete(this.apiEndpoint, new RequestOptions({
      headers: this.generateRequestHeaders(),
      body: JSON.stringify({'bookISBN': bookISBN, 'courseId': courseId})
    }));
  }

  private generateRequestHeaders(): Headers {
    return new Headers({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
  }
}
