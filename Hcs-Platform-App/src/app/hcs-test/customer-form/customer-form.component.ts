import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { IHttpDataSource } from '../../hcs-lib/datasource/IHttpDataSource';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { HCS_FUNCTION_NAME, HCS_FUNCTION_ROUTE } from '../../hcs-lib/Tokens';
import { FormPageComponent } from '../../hcs-components/form-page/form-page.component';
import { Customer } from '../../hcs-test-model/Customer';

@Component({
  selector: 'hcs-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.scss'],
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
export class CustomerFormComponent {
  formGroup: FormGroup;
  datasource: IHttpDataSource<Customer>;
  array: FormArray;
  @ViewChild(FormPageComponent) formPage: FormPageComponent<Customer>;
  constructor(private datasourceFactory: DataSourceFactory) {
    this.datasource = datasourceFactory.getDataSource(Customer);
    this.formGroup = new FormGroup({
      Id: new FormControl(),
      Name: new FormControl(null, Validators.required),
      Orders: new FormArray([])
    });
    this.array = this.formGroup.get('Orders') as FormArray;
  }
  public addItem() {
    const array = this.array;
    array.push(new FormGroup({
      Id: new FormControl(0),
      Item: new FormControl(null, Validators.required),
      Quantity: new FormControl(null, Validators.required),
    }));
  }
  public modelPreprocess() {
    return (model: Customer) => {
      if (model && model.Orders) {
        const array = this.array;
        while (array.controls.length) {
          array.removeAt(0);
        }
        model.Orders.forEach(x => {
          if (this.formPage.state === 'copy') {
            x.Id = 0;
          }
          this.addItem();
        });
      }
      return model;
    };
  }
}
