import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PoolListComponent } from './pool-list.component';

describe('PoolListComponent', () => {
  let component: PoolListComponent;
  let fixture: ComponentFixture<PoolListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PoolListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PoolListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
