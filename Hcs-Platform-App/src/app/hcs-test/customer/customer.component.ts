import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { OdataDataSource } from '../../hcs-lib/datasource/odata/OdataDataSource';
import { HCS_FUNCTION_NAME, HCS_FUNCTION_ROUTE } from '../../hcs-lib/Tokens';
import { Customer } from '../../hcs-test-model/Customer';

@Component({
  selector: 'hcs-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss'],
  providers: [
    {
      provide: HCS_FUNCTION_NAME,
      useValue: 'PlatformModule.Test.Customer'
    },
    {
      provide: HCS_FUNCTION_ROUTE,
      useValue: 'test/customer'
    }
  ]
})
export class CustomerComponent {

  public filterForm: FormGroup;
  public data: OdataDataSource<Customer>;
  constructor(datasourceFactory: DataSourceFactory) {
    this.data = datasourceFactory.getDataSource(Customer);
    this.filterForm = new FormGroup({
      Name: new FormControl()
    });
  }
}
