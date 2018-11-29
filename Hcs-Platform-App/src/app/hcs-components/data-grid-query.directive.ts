import { Directive, Input, Optional, OnInit } from '@angular/core';
import { DataGridQuery } from '../hcs-lib/DataGridQuery';
import { DataGridComponent } from './data-grid/data-grid.component';
import { NgModel, NgControl } from '@angular/forms';
import { IFilterOperator } from '../hcs-lib/datasource/IDatasourceFilter';
import { resolveTiming } from '@angular/animations/browser/src/util';

@Directive({
  selector: '[hcsDataGridQuery]'
})
export class DataGridQueryDirective implements OnInit {


  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridQuery') public inputQuery: DataGridQuery<any>;
  @Input() public field: string;
  @Input() public operator: IFilterOperator;
  private defaultQuery(ds, next) {
    if (!this.field) {
      this.field = this.ngControl.name;
    }
    if (this.field && this.operator) {
      const value = this.ngControl.value;
      if (value !== undefined && value !== null) {
        ds = ds.where(this.field, this.operator, value);
      }
    } else {
      console.warn('field or operator is empty');
    }
    next(ds);
  }
  public get query() {
    return this.inputQuery || this.defaultQuery;
  }

  constructor(private grid: DataGridComponent, private ngControl: NgControl) {
    this.grid.addQuery(this);
  }
  ngOnInit(): void {
    this.grid.addQuery(this);
  }
}
