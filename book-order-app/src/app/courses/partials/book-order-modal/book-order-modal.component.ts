import { Component, OnInit, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { BookOrderService } from './../../services/book-order.service';
import { Book } from './../../models/book';
import { Course } from './../../models/course';

import * as _ from 'lodash';
import { error } from 'selenium-webdriver';

@Component({
    selector: 'app-book-order-modal',
    templateUrl: './book-order-modal.component.html',
    styleUrls: ['./book-order-modal.component.css']
})
export class BookOrderModalComponent implements OnInit {
    @Input() courseId: number;
    @Input() coursesAvailable: Array<Course> = [];
    @Input() bookToAdd: Book;
    @Input() modalState: string;
    @Input() previousBooks;

    public modalForm: FormGroup;
    public authorToAdd: string = '';
    private bookOrderService;
    private originalISBN: string = '';

    constructor(private formbuilder: FormBuilder,
                bookOrderService: BookOrderService,
                private snackBar: MatSnackBar) {
        this.bookToAdd = new Book();
        this.modalForm = formbuilder.group({
            'bookToAdd': formbuilder.group({
                'bookISBN': new FormControl('', Validators.pattern('(\\d{13})')),
                'bookAuthors': '',
                'bookTitle': '',
                'bookPublisher': '',
                'bookEdition': '',
            })
        });
        this.bookOrderService = bookOrderService;
    }

    editBookChanges()
    {
        this.originalISBN = this.bookToAdd.bookISBN;
        var bookObject = {  'bookISBN': this.bookToAdd.bookISBN,
                            'bookAuthors': this.bookToAdd.bookAuthors,
                            'bookTitle': this.bookToAdd.bookTitle,
                            'bookPublisher': this.bookToAdd.bookPublisher,
                            'bookEdition': this.bookToAdd.bookEdition,
                            };
        this.modalForm.patchValue({'bookToAdd': bookObject});
    }

    ngOnInit() {
        this.bookToAdd = new Book();
    }

    ngOnChanges(changes: SimpleChanges) {
        if(this.modalState == "editState"){
            this.editBookChanges();
        } else if(this.modalState === 'addState'){
            this.clearBookInformation();
        }
    }

    submitForm(form: FormGroup): void {
        if(this.editing()) {
            this.submitEditForm(form);
        }
        else if(!form.invalid) {
            this.bookToAdd.bookTitle     = form.value.bookToAdd.bookTitle;
            this.bookToAdd.bookISBN      = form.value.bookToAdd.bookISBN;
            this.bookToAdd.bookEdition   = form.value.bookToAdd.bookEdition;
            this.bookToAdd.bookPublisher = form.value.bookToAdd.bookPublisher;
            this.bookToAdd.bookAuthors   = form.value.bookToAdd.bookAuthors
            this.addBook(this.bookToAdd);
        }
    }

    submitEditForm(form: FormGroup): void {
        if (!form.invalid) {
            this.bookToAdd.bookTitle     = form.value.bookToAdd.bookTitle;
            this.bookToAdd.bookAuthors   = form.value.bookToAdd.bookAuthors;
            this.bookToAdd.bookISBN      = form.value.bookToAdd.bookISBN;
            this.bookToAdd.bookEdition   = form.value.bookToAdd.bookEdition;
            this.bookToAdd.bookPublisher = form.value.bookToAdd.bookPublisher;
            //TODO post updated book info to database
            this.editBook(this.bookToAdd, this.originalISBN);
        }
    }

    editing(): boolean {
        return this.modalState === 'editState';
    }

    private clearBookInformation(): void {
        this.bookToAdd = new Book();
        this.modalForm.reset();
    }

    private addBook(bookToAdd: Book): void {
        try {
            this.bookOrderService.saveBook(bookToAdd, this.courseId).subscribe(response => _.find(this.coursesAvailable,
              (course) => course.courseId === this.courseId).bookArray.push(bookToAdd));
        } catch (Exception) {
          //TODO implement error handling
          console.log('book add failed.');
        }
        this.snackBar.open(bookToAdd.bookTitle + ' added', '', {duration: 2000});
        this.clearBookInformation();
    }

    private editBook(bookToAdd: Book, originalISBN: string): void {
        try {
            this.bookOrderService.editBook(bookToAdd, this.courseId, originalISBN)
            .subscribe(success => this.snackBar.open('Changes submitted for ' + this.bookToAdd.bookTitle, '', {duration:2000}),
                       error => this.snackBar.open('Failed to submit changes for ' + this.bookToAdd.bookTitle, '', {duration:2000}));
        } catch (Exception) {
          //TODO implement error handling
          console.log('book edit failed.');
        }
    }

    isDuplicateBook(isbn: string): boolean {
        return _.find(
                _.find(this.coursesAvailable,
                (course) => course.courseId === this.courseId).bookArray,
                (book) => book.bookISBN === isbn) != undefined;
    }
}
