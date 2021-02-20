import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMailingComponent } from './add-mailing.component';

describe('AddMailingComponent', () => {
  let component: AddMailingComponent;
  let fixture: ComponentFixture<AddMailingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMailingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMailingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
