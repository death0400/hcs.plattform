import { Directive, TemplateRef, Input, Optional, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { DataGridComponent } from './data-grid/data-grid.component';
import { Subject } from 'rxjs';

@Directive({
  selector: '[hcsDataGridColum]'
})
export class DataGridColumDirective implements OnChanges {

  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumName') public columnName: string;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumCellClass') public cellClass: string;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumHeaderClass') public headerClass: string;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumWidth') public width: string;
  // tslint:disable-next-line:no-input-rename
  private _field: string;
  @Input('hcsDataGridColumField') public get field() { return this._field; }
  public set field(value: string) {
    this._field = value;
    if (value) {
      const parts = value.split('.');
      this.fieldValueGetter = (entity: any) => {
        let result = entity;
        for (const current of parts) {
          if (typeof result === 'object' && result !== undefined && result !== null) {
            result = result[current];
          } else {
            break;
          }
        }
        return result;
      };
    } else {
      this.fieldValueGetter = () => undefined;
    }
  }
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumOrderby') public orderby: string;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumAlign') public align: string;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumSortable') public sortable = true;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumVisible') public visible = true;
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumExport') public export: boolean | string = false;
  public change = new Subject();
  // tslint:disable-next-line:no-input-rename
  @Input('hcsDataGridColumCellStyle') public cellStyle: any | ((entity: any) => any);

  public fieldValueGetter: (entity: any) => any = () => undefined;
  public ngStyles(entity: any): any {
    const style = this.getStyles();
    const cs = (typeof this.cellStyle === 'function' ? this.cellStyle(entity) : this.cellStyle) || {};
    return Object.assign(style, cs);
  }

  public getStyles() {
    const style = {};
    if (this.width) {
      style['width'] = this.width + 'px';
    }
    return style;
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.change.next(changes);
  }
  constructor(public templateRef: TemplateRef<any>) {

  }

}
