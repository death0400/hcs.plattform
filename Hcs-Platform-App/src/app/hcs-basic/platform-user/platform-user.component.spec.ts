import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformUserComponent } from './platform-user.component';

describe('PlatformUserComponent', () => {
  let component: PlatformUserComponent;
  let fixture: ComponentFixture<PlatformUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlatformUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
