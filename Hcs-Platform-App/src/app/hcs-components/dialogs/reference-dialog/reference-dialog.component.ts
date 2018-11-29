import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { IDataSource } from '../../../hcs-lib/datasource/IDataSource';
import { FormGroup, FormControl } from '@angular/forms';
import { IReferencePickerSettings } from '../../IReferencePickerSettings';
import { HCS_ENABLE_STATE } from '../../../hcs-lib/Tokens';

@Component({
  selector: 'hcs-reference-dialog',
  templateUrl: './reference-dialog.component.html',
  styleUrls: ['./reference-dialog.component.scss'],
  providers: [
    {
      provide: HCS_ENABLE_STATE,
      useValue: false
    }
  ]
})
export class ReferenceDialogComponent implements OnInit {
  public filterForm: FormGroup;
  public show = false;
  constructor(@Inject(MAT_DIALOG_DATA) public data: IReferencePickerSettings, daialog: MatDialogRef<ReferenceDialogComponent>) {
    const f = {};
    (data.filters || []).forEach(filter => {
      f[filter.field] = new FormControl();
    });
    this.filterForm = new FormGroup(f);
    daialog.afterOpen().subscribe(x => this.show = true);
  }

  ngOnInit() {
  }

}
