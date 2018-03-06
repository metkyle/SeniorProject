import { Component, OnInit } from '@angular/core';
import { Instructor } from '../../courses/models/instructors';
import { InstructorDataService } from '../../courses/services/instructor-data.service';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { Observable } from "rxjs/Observable";
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'app-contact-overview',
    templateUrl: './contact-overview.component.html',
    styleUrls: ['./contact-overview.component.css']
})

export class ContactOverviewComponent implements OnInit {

  private emailTo: string = "";
  private emailFrom: string= "emailFrom";
  private emailBody: string = "";
  private emailSubject: string = "";
  private recipList: string[];
  private checkboxConnie: boolean;
  private instCheckbox: { name: any, checked: boolean, visited: boolean, email: any, isAdmin: any }[] = [];
  private allFacultyIsChecked: boolean = false;
  private allAdminIsChecked: boolean = false;
  private adminCheckbox = [];
  private firstFill: boolean = true;

  private allInstructors: Array<Instructor> = new Array <Instructor>();

  constructor(private http: Http, private instructorDataService: InstructorDataService, private snackbar: MatSnackBar) {
    
  }

  ngOnInit() {
    this.instructorDataService.generateInstructorsForDeptId(0).subscribe(response => this.allInstructors = response.json());
  }

  submitEmail(): void{
    
    let formData: FormData = new FormData();
    formData.append('emailTo', this.emailTo);
    formData.append('emailFrom', this.emailFrom);
    formData.append('emailSubject', this.emailSubject);
    formData.append('emailBody', this.emailBody);

    let headers = new Headers();
    let options = new RequestOptions({ headers: headers });
    let apiUrl1 = "api/EmailProcessor";
    
    this.http.post(apiUrl1, formData, options)
      .catch(error => Observable.throw(error))
      .subscribe(data => { this.snackbar.open("E-mail Sent Successful", '', { duration: 2000 }); this.clearEmailFields() }, error => this.snackbar.open("Error Sending E-mail", '', { duration: 2000 }));
  }

  clearEmailFields(): void {
    this.emailTo = "";
    this.emailFrom = "";
    this.emailSubject = "";
    this.emailBody = "";

    for (let i = 0; i < this.instCheckbox.length; i++) {
      if (this.instCheckbox[i].checked == true) {
        this.instCheckbox[i].checked = false;
      }
    }
    for (let i = 0; i < this.adminCheckbox.length; i++) {
      if (this.adminCheckbox[i].checked == true) {
        this.adminCheckbox[i].checked = false;
      }
    }
    if (this.allAdminIsChecked || this.allFacultyIsChecked) {
      this.allAdminIsChecked = false;
      this.allFacultyIsChecked = false;
    }

  }

  appendToRecip(): void{
    //go through each checkbox

    for (let i = 0; i < this.instCheckbox.length; i++) {
      //if its checked and is first time checking it
      
        if (this.instCheckbox[i].checked == true && this.instCheckbox[i].visited == false) {
          this.emailTo += this.instCheckbox[i].email + ", ";
          this.instCheckbox[i].visited = true;
        }
        //if not checked
        if (this.instCheckbox[i].checked == false) {
          this.emailTo = this.emailTo.replace(this.instCheckbox[i].email + ", ", "");
          this.instCheckbox[i].visited = false;
        }
      }
    
    
  }

  appendAdminToRecip(): void {
    for (let i = 0; i < this.adminCheckbox.length; i++){
      if (this.adminCheckbox[i].checked == true && this.adminCheckbox[i].visited == false) {
        this.emailTo += this.adminCheckbox[i].email + ", ";
        this.adminCheckbox[i].visited = true;
      }
      //if not checked
      if (this.adminCheckbox[i].checked == false) {
        this.emailTo = this.emailTo.replace(this.adminCheckbox[i].email + ", ", "");
        this.adminCheckbox[i].visited = false;
      }
      }
  }

  selectAllFaculty(): void {
    if (this.allFacultyIsChecked) {
      for (let i = 0; i < this.instCheckbox.length; i++) {
        this.instCheckbox[i].checked = true;
      }
      this.appendToRecip();
    }
    if (this.allFacultyIsChecked == false) {
      for (let i = 0; i < this.instCheckbox.length; i++) {
        this.instCheckbox[i].checked = false;
      }
      this.emailTo = "";
      this.appendToRecip();
    }
    
  }

  selectAllAdmin(): void {
    if (this.allAdminIsChecked) {
      for (let i = 0; i < this.adminCheckbox.length; i++) {
        this.adminCheckbox[i].checked = true;
      }
      this.appendAdminToRecip();
    }
    if (this.allAdminIsChecked == false) {
      for (let i = 0; i < this.adminCheckbox.length; i++) {
        this.adminCheckbox[i].checked = false;
      }
      this.emailTo = "";
      this.appendAdminToRecip();
    }
  }

  fillInstructors(): void {
    //this.instructorDataService.generateInstructorsForDeptId(0).subscribe(response => this.allInstructors = response.json());
    

    if (this.allInstructors != null && this.firstFill == true) {
      for (let i = 0; i < this.allInstructors.length; i++) {
        if (this.allInstructors[i].instructorIsAdmin == 1) {
          this.adminCheckbox.push({ name: this.allInstructors[i].instructorName, checked: false, visited: false, email: this.allInstructors[i].instructorEmail, isAdmin: this.allInstructors[i].instructorIsAdmin });
        }
        else {
          this.instCheckbox.push({ name: this.allInstructors[i].instructorName, checked: false, visited: false, email: this.allInstructors[i].instructorEmail, isAdmin: this.allInstructors[i].instructorIsAdmin });
        }
      }
      this.firstFill = false;
    }
  }

  instructorHasSubmittedAllCourses(instructor: Instructor): boolean {
    var submitted = true;

    if (instructor.instructorCourses === undefined || instructor.instructorCourses.length == 0) {
      return false;
    }

    for (let course of instructor.instructorCourses) {
      if (!course.isSubmitted) {
        submitted = false;
        break;
      }
    }
    return submitted;
  }

  getInstructorByName(instName: string): boolean {
    for (let instructor of this.allInstructors) {
      if (instName === instructor.instructorName)
        return this.instructorHasSubmittedAllCourses(instructor);
    }
    return false;
  }
}
