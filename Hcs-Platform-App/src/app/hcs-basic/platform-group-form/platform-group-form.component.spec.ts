import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformGroupFormComponent } from './platform-group-form.component';

describe('PlatformGroupFormComponent', () => {
  let component: PlatformGroupFormComponent;
  let fixture: ComponentFixture<PlatformGroupFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlatformGroupFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformGroupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
