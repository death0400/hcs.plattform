import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportToolBarComponent } from './export-tool-bar.component';

describe('ExportToolBarComponent', () => {
  let component: ExportToolBarComponent;
  let fixture: ComponentFixture<ExportToolBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportToolBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportToolBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
