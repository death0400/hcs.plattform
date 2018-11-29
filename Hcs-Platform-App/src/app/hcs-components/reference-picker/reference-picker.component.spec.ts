import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferencePickerComponent } from './reference-picker.component';

describe('ReferencePickerComponent', () => {
  let component: ReferencePickerComponent<any>;
  let fixture: ComponentFixture<ReferencePickerComponent<any>>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReferencePickerComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferencePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
