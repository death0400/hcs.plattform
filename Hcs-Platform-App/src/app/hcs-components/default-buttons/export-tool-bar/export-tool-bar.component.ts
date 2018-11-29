import { Component, OnInit, Input, Inject, OnDestroy } from '@angular/core';
import { Dialog } from '../../dialogs/Dialog';
import { TranslateService } from '@ngx-translate/core';
import { first } from 'rxjs/operators';
import { DataGridComponent } from '../../data-grid/data-grid.component';
import { OdataDataSource } from '../../../hcs-lib/datasource/odata/OdataDataSource';
import { HttpClient } from '@angular/common/http';
import { HCS_FUNCTION_NAME, HCS_EXPORT_LIMIT } from '../../../hcs-lib/Tokens';
import { FileManager } from '../../../hcs-lib/FileManager';
import { Subscription } from 'rxjs';

@Component({
  selector: 'hcs-export-tool-bar',
  templateUrl: './export-tool-bar.component.html',
  styleUrls: ['./export-tool-bar.component.scss']
})
export class ExportToolBarComponent implements OnInit, OnDestroy {
  public subs: Subscription[] = [];
  public showExportBtn = false;
  @Input() processColumns = (columns: string[]) => columns;
  @Input() getFileName = (name) => name;

  constructor(@Inject(HCS_EXPORT_LIMIT) private limit: number,
    private fileManager: FileManager, private dialog: Dialog,
    private translate: TranslateService,
    public grid: DataGridComponent,
    private http: HttpClient,
    @Inject(HCS_FUNCTION_NAME) private fn: string) {
    this.subs.push(grid.dataLoaded.subscribe(() => {
      this.showExportBtn = this.grid.columns.filter(x => !!x.export).length > 0;
    }));
  }
  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }
  public export() {
    this.translate.get(['common.confirmExportTitle', 'common.confirmExport', `functions.${this.fn}`, 'common.export']).pipe(first()).subscribe(values => {
      const selectedCount = this.grid.getSelectRowCount();
      let exportCount = selectedCount;
      if (exportCount === 0) {
        exportCount = this.grid.total;
      }
      if (exportCount > this.limit) {
        this.translate.get('common.exportLimit', { limit: this.limit }).pipe(first()).subscribe(limitText => {
          this.dialog.alert(limitText);
        });
      } else {
        this.dialog.confirm(values['common.confirmExportTitle'], values['common.confirmExport']).subscribe(result => {
          if (result) {
            let ds = this.grid.loadedDataSource.clone();
            if (this.grid.enableRowSelect && selectedCount) {
              ds = ds.where(ds.identityProperty, 'in', this.grid.getSelectIds());
            }
            const ods = (ds as OdataDataSource<any>);
            ods.queryOptions.skip = undefined;
            ods.queryOptions.take = undefined;
            const params = ods.getOdataParams();
            params['export'] = 'true';
            const fields = this.grid.columns.filter(x => !!x.export).map(x => ({ name: x.columnName || x.field, field: typeof x.export === 'string' ? x.export : x.field }));
            const columns = fields.map(x => `${x.field}/${x.name}`);
            params['columns'] = this.processColumns(columns).join(',');
            this.fileManager.downloadFile(this.getFileName(`${values['common.export'] || 'Export'}-${values[`functions.${this.fn}`] || this.fn}.xlsx`), ods.apiUrl, params);
          }
        });
      }
    });

  }
  ngOnInit() {
  }

}
