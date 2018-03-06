import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpModule } from '@angular/http';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BootstrapSwitchModule } from 'angular2-bootstrap-switch';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JWBootstrapSwitchModule } from 'jw-bootstrap-switch-ng2';
import { MatSnackBarModule } from '@angular/material';
import { MatCheckboxModule } from '@angular/material';
import { DropdownModule } from "ngx-dropdown";

import { AppRouterModule } from './app-router/app-router.module';
import { FileSelectDirective } from 'ng2-file-upload';

/* Services */
import { CourseDataService } from './courses/services/course-data.service';
import { BookOrderService } from './courses/services/book-order.service';
import { InstructorDataService } from './courses/services/instructor-data.service';
import { TermDataService } from './courses/services/term-data.service';
import { SsoDataService } from './login/services/sso-data.service';

/* Components */
import { AppComponent } from './app.component';
import { CourseOverviewComponent } from './courses/course-overview/course-overview.component';
import { DashboardComponent } from './dashboard/dashboard/dashboard.component';
import { ReportGeneratorComponent } from './reports/report-generator/report-generator.component';
import { ContactOverviewComponent } from './contact/contact-overview/contact-overview.component';
import { LoginComponent } from './login/login.component';
import { BookOrderModalComponent } from './courses/partials/book-order-modal/book-order-modal.component';
import { CourseConfirmationModalComponent } from './courses/partials/course-confirmation-modal/course-confirmation-modal.component';
import { FileUploadComponentComponent } from './fileupload/file-upload-component/file-upload-component.component';
import { AdminComponent } from './admin/admin.component';
import { CourseAddModalComponent } from './courses/partials/course-add-modal/course-add-modal.component';
import { TermAddModalComponent } from './courses/partials/term-add-modal/term-add-modal.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { ErrorComponent } from './error/error.component';


@NgModule({
    declarations: [
      AppComponent,
      CourseOverviewComponent,
      DashboardComponent,
      ReportGeneratorComponent,
      ContactOverviewComponent,
      LoginComponent,
      BookOrderModalComponent,
      CourseConfirmationModalComponent,
      FileUploadComponentComponent,
      FileSelectDirective,
      AdminComponent,
      CourseAddModalComponent,
      TermAddModalComponent,
      AuthenticationComponent,
      ErrorComponent
    ],
    imports: [
        BrowserModule,
        AppRouterModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        NgbModule.forRoot(),
        BootstrapSwitchModule.forRoot(),
        BrowserAnimationsModule,
        MatSnackBarModule,
        JWBootstrapSwitchModule,
        MatCheckboxModule,
        DropdownModule

    ],
    providers: [
        CourseDataService,
        BookOrderService,
        InstructorDataService,
        TermDataService,
        SsoDataService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
