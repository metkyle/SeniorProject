//TODO flush out what attributes make up a course object
import { Book } from './../models/book';
import { BookOrderOption } from "./../models/book-order-option.enum";

export class Course {
    public courseId: number;//TODO determine if this is the integer of the "newly" created course or not
    public courseNumber: string;//TODO maybe something along the lines of CSCDXXX
    public instructor: string;
    public name: string;
    public department: string;
    public section: string;
    public isSubmitted: boolean;
    public myBookOrderOption: BookOrderOption;
    public bookArray: Array<Book>; //An array of books for each course
    public term: string;
    public year: number;

    constructor(courseId: number, courseNumber: string, instructor: string,
                name: string, department: string, section: string, isSubmitted: boolean,
                orderOption: BookOrderOption, bookArray: Array<Book>){
                    this.courseId = courseId;
                    this.courseNumber = courseNumber;
                    this.instructor = instructor;
                    this.name = name;
                    this.department = department;
                    this.section = section;
                    this.isSubmitted = isSubmitted;
                    this.myBookOrderOption = orderOption;
                    this.bookArray = bookArray;
                }

    public getCourseBooks(): Array<Book>{
        return this.bookArray;
    }

    public noBookRequired(): boolean{
        return this.myBookOrderOption == BookOrderOption.noBook;
    }

    public getCourseName(): string {
        return this.department + ' ' + this.courseNumber;
    }
}

