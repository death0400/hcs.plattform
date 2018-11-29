import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultButtonListComponent } from './default-button-list.component';

describe('DefaultButtonListComponent', () => {
  let component: DefaultButtonListComponent;
  let fixture: ComponentFixture<DefaultButtonListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultButtonListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultButtonListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
