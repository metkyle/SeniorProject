import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SsoDataService } from './services/sso-data.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private token: string;

  constructor(private ssoDataService: SsoDataService) { }

  ngOnInit() {
    this.ssoDataService.generateSSOLoginUrl().subscribe(this.redirectToEWULogin);
  }

  private redirectToEWULogin(response: any){
    window.location.assign(response.json());
  }

}
