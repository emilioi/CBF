import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SiteSidebarComponent } from './site-sidebar.component';

describe('SiteSidebarComponent', () => {
  let component: SiteSidebarComponent;
  let fixture: ComponentFixture<SiteSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SiteSidebarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SiteSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
