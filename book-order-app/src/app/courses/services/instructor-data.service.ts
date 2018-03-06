import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';

import { Instructor } from './../models/instructors';

@Injectable()
export class InstructorDataService {

  private requestOptions = new RequestOptions({
    headers: new Headers({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    })
});

private apiEndpoint: string = 'api/instructorprocessor';
private courses: Instructor[];
constructor(private http: Http) {
}

testApiEndpoint(): any {
  this.http.get(this.apiEndpoint, this.requestOptions).
      subscribe(response => console.log(response.json()));
}

generateInstructorsForDeptId(deptId: number): Observable<Response> {
  return this.http.get(this.apiEndpoint + '/' + deptId, this.requestOptions);
}
}
