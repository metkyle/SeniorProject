import { Component, OnInit, Input, OnChanges, SimpleChange, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

import { Term } from '../../models/term';
import { TermDataService } from '../../services/term-data.service';

@Component({
  selector: 'app-term-add-modal',
  templateUrl: './term-add-modal.component.html',
  styleUrls: ['./term-add-modal.component.css']
})
export class TermAddModalComponent implements OnInit {
  @Input() currentTerms;

  public quarter: string = "Fall";
  public year: number;
  public numbers: Array<number>;
  private dueDate: Date;

  private modalForm: FormGroup;
  constructor(private formbuilder: FormBuilder,
              private termDataService: TermDataService,
              private snackBar: MatSnackBar) { 

                var today = new Date();
                this.year = today.getFullYear();

    this.modalForm = formbuilder.group({
      'termToAdd': formbuilder.group({
          'quarter': this.quarter,
          'year': this.year
      })
  });

  this.numbers = new Array<number>(3);

  for (var _i = 0; _i < this.numbers.length; _i++) {
      this.numbers[_i] = this.year - 1 + _i;
    }
  }

  selectQuarter(quarter: string) {
    this.quarter = quarter;
    this.modalForm.value.termToAdd.quarter = quarter;
  }

  selectYear(year: number) {
    this.year = year;
    this.modalForm.value.termToAdd.year = year;
  }

  ngOnInit() {
  }

  submitForm(form: FormGroup): void {
    if(!form.invalid) {
          var term = new Term(form.value.termToAdd.quarter, form.value.termToAdd.year);

          if(this.isDuplicateTerm(term)) {
            this.snackBar.open('Failure: ' + term.quarter + ' ' + term.year + ' already exists', '', {duration:2000});
          }
          else {
            this.addTerm(term);
          }
    }
  }

  private clearTermInformation(): void {
    this.modalForm.reset();
  }

  private addTerm(termToAdd: Term): void {
    try {
      this.termDataService.saveTerm(termToAdd, 0).subscribe(response => this.currentTerms.push(termToAdd));
    } catch (Exception) {
      //TODO implement error handling
      console.log('course add failed.');
    }
    this.snackBar.open(termToAdd.quarter + ' ' + termToAdd.year + ' added', '', {duration:2000});

}
  private isDuplicateTerm(newTerm: Term): boolean {
    for(let term of this.currentTerms) {
      if(term.year === newTerm.year && term.quarter === newTerm.quarter) {
        return true;
      }
    }
    return false;
  }
}
