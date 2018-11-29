import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformUserFormComponent } from './platform-user-form.component';

describe('PlatformUserFormComponent', () => {
  let component: PlatformUserFormComponent;
  let fixture: ComponentFixture<PlatformUserFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlatformUserFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformUserFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
