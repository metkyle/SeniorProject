import { Course } from './../models/course';

export class Instructor {
    public instructorId: number;
    public instructorName: string;
    public instructorDept: string;
    public instructorCourses: Array<Course>;
    public instructorEmail: string;
    public instructorIsAdmin: number;
    public instructorUserName: string;

    constructor(instructorId: number, instructorName: string,
                instructorDept: string, instructorCourses: Array<Course>,
                instructorEmail: string, instructorIsAdmin: number,
                instructorUserName: string){
                    this.instructorId = instructorId;
                    this.instructorName = instructorName;
                    this.instructorDept = instructorDept;
                    this.instructorCourses = instructorCourses;
                    this.instructorEmail = instructorEmail;
                    this.instructorIsAdmin = instructorIsAdmin;
                    this.instructorUserName = instructorUserName;
                }

    public getInstructorCourses(): Array<Course>{
        return this.instructorCourses;
    }
}

