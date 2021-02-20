import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignGroupAdminComponent } from './assign-group-admin.component';

describe('AssignGroupAdminComponent', () => {
  let component: AssignGroupAdminComponent;
  let fixture: ComponentFixture<AssignGroupAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignGroupAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignGroupAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
