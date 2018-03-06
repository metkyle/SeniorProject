import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookOrderModalComponent } from './book-order-modal.component';

describe('BookOrderModalComponent', () => {
  let component: BookOrderModalComponent;
  let fixture: ComponentFixture<BookOrderModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookOrderModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookOrderModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
