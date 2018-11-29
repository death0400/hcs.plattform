import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferenceSearchComponent } from './reference-search.component';

describe('ReferenceSearchComponent', () => {
  let component: ReferenceSearchComponent;
  let fixture: ComponentFixture<ReferenceSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferenceSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferenceSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
