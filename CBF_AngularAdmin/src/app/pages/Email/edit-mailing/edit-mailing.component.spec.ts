import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMailingComponent } from './edit-mailing.component';

describe('EditMailingComponent', () => {
  let component: EditMailingComponent;
  let fixture: ComponentFixture<EditMailingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMailingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMailingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
