import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { CourseDataService } from '../../courses/services/course-data.service';
import { Course } from '../../courses/models/course';
import { Book } from "../../courses/models/book";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    private ssoToken: string;

    public coursesAvailable: Array<Course> = new Array<Course>();
    private currentInstructor: string = "Test";
    constructor(private courseDataService: CourseDataService) { }

  ngOnInit() {
    this.courseDataService.generateCourseDataForInstructor(1).subscribe(response => this.coursesAvailable = response.json());
    
  }

}
