import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditEmailTemplatesComponent } from './edit-email-templates.component';

describe('EditEmailTemplatesComponent', () => {
  let component: EditEmailTemplatesComponent;
  let fixture: ComponentFixture<EditEmailTemplatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditEmailTemplatesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditEmailTemplatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
