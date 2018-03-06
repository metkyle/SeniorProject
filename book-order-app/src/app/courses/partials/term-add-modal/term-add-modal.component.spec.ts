import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TermAddModalComponent } from './term-add-modal.component';

describe('TermAddModalComponent', () => {
  let component: TermAddModalComponent;
  let fixture: ComponentFixture<TermAddModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TermAddModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TermAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
