import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerComponent } from './customer/customer.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';
import { HcsPlatformModule } from '../hcs-platform/hcs-platform.module';
import { RouterModule } from '@angular/router';
import { hcsTestRoutes } from './hcs-test.route';

@NgModule({
  imports: [
    HcsPlatformModule,
    RouterModule.forChild(hcsTestRoutes)
  ],
  declarations: [CustomerComponent, CustomerFormComponent]
})
export class HcsTestModule { }
