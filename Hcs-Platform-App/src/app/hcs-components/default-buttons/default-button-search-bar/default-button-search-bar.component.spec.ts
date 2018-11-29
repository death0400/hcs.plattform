import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultButtonSearchBarComponent } from './default-button-search-bar.component';

describe('DefaultButtonSearchBarComponent', () => {
  let component: DefaultButtonSearchBarComponent;
  let fixture: ComponentFixture<DefaultButtonSearchBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultButtonSearchBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultButtonSearchBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
