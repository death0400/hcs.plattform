import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformGroupComponent } from './platform-group.component';

describe('PlatformGroupComponent', () => {
  let component: PlatformGroupComponent;
  let fixture: ComponentFixture<PlatformGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlatformGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
