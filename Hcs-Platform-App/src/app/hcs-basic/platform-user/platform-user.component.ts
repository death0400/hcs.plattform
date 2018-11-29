import { Component, OnInit, HostBinding, ViewChild } from '@angular/core';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { PlatformUser } from '../../models/hcs-basic-model/PlatformUser';
import { Enums } from '../../models/hcs-basic-model/enums';
import { HCS_FUNCTION_NAME } from '../../hcs-lib/Tokens';
import { OdataDataSource } from '../../hcs-lib/datasource/odata/OdataDataSource';
import { FormGroup, FormControl } from '@angular/forms';
import { Permission } from '../../hcs-lib/Permission';
import { UserState } from '../../hcs-lib/UserState';

@Component({
  selector: 'hcs-platform-user',
  templateUrl: './platform-user.component.html',
  styleUrls: ['./platform-user.component.scss'],
  providers: [
    {
      provide: HCS_FUNCTION_NAME,
      useValue: 'Basic.PlatformUser'
    },
    Permission.provider
  ]
})
export class PlatformUserComponent implements OnInit {
  public showStatus = true;
  public status = Enums.UserStatus;
  public filterForm: FormGroup;
  public data: OdataDataSource<PlatformUser>;
  constructor(datasourceFactory: DataSourceFactory, public permission: Permission, public currentUser: UserState) {
    this.data = datasourceFactory.getDataSource(PlatformUser);
    this.filterForm = new FormGroup({
      Name: new FormControl(),
      Account: new FormControl(),
      Email: new FormControl()
    });
  }
  ngOnInit() {

  }

}
