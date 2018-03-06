import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { LoginGuard } from './../login/resolvers/login.guard';

import { DashboardComponent } from './../dashboard/dashboard/dashboard.component';
import { CourseOverviewComponent } from './../courses/course-overview/course-overview.component';
import { ReportGeneratorComponent } from './../reports/report-generator/report-generator.component';
import { ContactOverviewComponent } from './../contact/contact-overview/contact-overview.component';
import { AuthenticationComponent } from './../authentication/authentication.component';
import { ErrorComponent } from './../error/error.component';

import { AdminComponent } from './../admin/admin.component';
import { LoginComponent } from './../login/login.component';
import { FileUploadComponentComponent } from './../fileupload/file-upload-component/file-upload-component.component';

const appRoutes: Routes = [
  {
    path: '', canActivate: [LoginGuard], children: [
        { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
        { path: 'dashboard', pathMatch: 'full', component: DashboardComponent },
        { path: 'admin', pathMatch: 'full', component: AdminComponent },
        { path: 'course-overview', pathMatch: 'full', component: CourseOverviewComponent },
        { path: 'report-generator', pathMatch: 'full', component: ReportGeneratorComponent },
        { path: 'contact-overview', pathMatch: 'full', component: ContactOverviewComponent },
        { path: 'file-upload-component', pathMatch: 'full', component: FileUploadComponentComponent },
      ]
    },
    { path: 'authentication', pathMatch: 'full', component: AuthenticationComponent },
    { path: 'error', pathMatch: 'full', component: ErrorComponent },
    { path: 'login', pathMatch: 'full', component: LoginComponent },
    { path: '**', redirectTo: '/dashboard', pathMatch: 'full' } // catch all route
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes),
    NgbModule
  ],
  exports: [ RouterModule ],
  providers: [
    LoginGuard
  ]
})
export class AppRouterModule { }
