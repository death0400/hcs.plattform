import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { OdataDataSource } from '../../../hcs-lib/datasource/odata/OdataDataSource';
import { DataGridComponent } from '../../data-grid/data-grid.component';
import { Dialog } from '../../dialogs/Dialog';
import { TranslateService } from '@ngx-translate/core';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'hcs-delete-button',
  templateUrl: './delete-button.component.html',
  styleUrls: ['./delete-button.component.scss']
})
export class DeleteButtonComponent implements OnInit {
  @Input() datasource: OdataDataSource<any>;
  @Input() key: any;
  public deleteClick() {
    combineLatest([this.tanslate.get('common.deleteTitle'), this.tanslate.get('common.deleteMessage')]).subscribe(resp => {
      this.dialog.confirm(resp[0], resp[1]).subscribe(ok => {
        if (ok) {
          this.datasource.delete(this.key).subscribe(x => {
            this.grid.queueLoad();
          }, (error) => {
            this.dialog.httpError(error);
          });
        }
      });
    });



  }
  constructor(private grid: DataGridComponent, private dialog: Dialog, private tanslate: TranslateService) { }
  ngOnInit() {
  }

}
