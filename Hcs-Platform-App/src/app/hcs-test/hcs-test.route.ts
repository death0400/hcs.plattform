import { Route } from '@angular/router';
import { CustomerComponent } from './customer/customer.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';



export const hcsTestRoutes: Route[] = [
    { path: 'customer', component: CustomerComponent },
    { path: 'customer/new', component: CustomerFormComponent },
    { path: 'customer/new/:copy', component: CustomerFormComponent },
    { path: 'customer/:id', component: CustomerFormComponent, data: { readonly: true } },
    { path: 'customer/:id/edit', component: CustomerFormComponent },
    // { path: '**', redirectTo: '' }
];
