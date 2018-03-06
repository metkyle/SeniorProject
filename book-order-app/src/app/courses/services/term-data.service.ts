import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';

import { Term } from '../models/term';
import { Jsonp } from '@angular/http/src/http';

@Injectable()
export class TermDataService {

  private requestOptions = new RequestOptions({
    headers: new Headers({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    })
});

private apiEndpoint: string = 'api/termprocessor';
private terms: Term[]
constructor(private http: Http) {
}

generateCurrentAvailableTerms(deptId: number): Observable<Response> {
  return this.http.get(this.apiEndpoint + "/" + deptId, this.requestOptions);
}

deleteTerm(deptId: number, term: Term): Observable<Response> {
  return this.http.delete(this.apiEndpoint, new RequestOptions({
    headers: this.generateRequestHeaders(),
    body: JSON.stringify({'DeptID': deptId, 'Term': term})
  }));
}

saveTerm(term: Term, deptID: number): Observable<Response>{
  return this.http.post(this.apiEndpoint, JSON.stringify({ DeptID : deptID, Term: term}), this.requestOptions);
}

private generateRequestHeaders(): Headers {
  return new Headers({
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  });
}
}
