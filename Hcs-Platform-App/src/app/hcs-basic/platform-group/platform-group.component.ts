import { Component, OnInit } from '@angular/core';
import { OdataDataSource } from '../../hcs-lib/datasource/odata/OdataDataSource';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { HCS_FUNCTION_NAME } from '../../hcs-lib/Tokens';
import { PlatformGroup } from '../../models/hcs-basic-model/PlatformGroup';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'hcs-platform-group',
  templateUrl: './platform-group.component.html',
  styleUrls: ['./platform-group.component.scss'],
  providers: [
    {
      provide: HCS_FUNCTION_NAME,
      useValue: 'Basic.PlatformGroup'
    }
  ]
})
export class PlatformGroupComponent implements OnInit {

  public showStatus = true;
  public filterForm: FormGroup;
  public data: OdataDataSource<PlatformGroup>;
  constructor(datasourceFactory: DataSourceFactory) {
    this.data = datasourceFactory.getDataSource(PlatformGroup);
    this.filterForm = new FormGroup({
      Name: new FormControl(),
      IsEnabled: new FormControl()
    });
  }
  ngOnInit() {

  }

}
