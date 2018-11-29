import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataChangeLogComponent } from './data-change-log.component';

describe('DataChangeLogComponent', () => {
  let component: DataChangeLogComponent;
  let fixture: ComponentFixture<DataChangeLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataChangeLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DataChangeLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
