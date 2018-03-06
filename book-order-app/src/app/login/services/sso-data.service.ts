import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';

@Injectable()
export class SsoDataService {

  private requestOptions = new RequestOptions({
    headers: this.generateRequestHeaders()
  });

  private apiEndpoint: string = 'api/singlesignon';

  constructor(private http: Http) { }

  public isAuthenticated(): Observable<Response> {
    return this.http.get(this.apiEndpoint, this.requestOptions);
  }

  public generateSSOLoginUrl(): Observable<Response> {
    return this.http.post(this.apiEndpoint, '', this.requestOptions);
  }

  public authenticateUser(ticket: string): Observable<Response> {
    return this.http.put(this.apiEndpoint, JSON.stringify(ticket), this.requestOptions);
  }

  private generateRequestHeaders(): Headers {
    return new Headers({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
  }

}
