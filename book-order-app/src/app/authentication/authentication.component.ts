import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { SsoDataService } from './../login/services/sso-data.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {

  constructor(private ssoDataService: SsoDataService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.ssoDataService.authenticateUser(params['ticket']).
        subscribe(success => this.router.navigate(['/dashboard']),
          error => this.router.navigate(['/error']));
    });
  }

}
