import { Route } from '@angular/router';
import { PlatformUserComponent } from './platform-user/platform-user.component';
import { PlatformGroupComponent } from './platform-group/platform-group.component';
import { PlatformUserFormComponent } from './platform-user-form/platform-user-form.component';
import { PlatformGroupFormComponent } from './platform-group-form/platform-group-form.component';



export const hcsBasicRoutes: Route[] = [
    // user
    { path: 'platform-user', component: PlatformUserComponent },
    { path: 'platform-user/new', component: PlatformUserFormComponent },
    { path: 'platform-user/new/:copy', component: PlatformUserFormComponent },
    { path: 'platform-user/:id', component: PlatformUserFormComponent, data: { readonly: true } },
    { path: 'platform-user/:id/edit', component: PlatformUserFormComponent },
    // group
    { path: 'platform-group', component: PlatformGroupComponent },
    { path: 'platform-group/new', component: PlatformGroupFormComponent },
    { path: 'platform-group/new/:copy', component: PlatformGroupFormComponent },
    { path: 'platform-group/:id', component: PlatformGroupFormComponent, data: { readonly: true } },
    { path: 'platform-group/:id/edit', component: PlatformGroupFormComponent },
    // { path: '**', redirectTo: '' }
];
