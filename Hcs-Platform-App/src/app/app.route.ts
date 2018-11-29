import { Route } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { LoginComponent } from './login/login.component';
import { RequireLoginGuard } from './hcs-lib/RequireLoginGuard';



export const appRoutes: Route[] = [
    { path: '', component: IndexComponent, canActivate: [RequireLoginGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'basic', loadChildren: './hcs-basic/hcs-basic.module#HcsBasicModule', canActivate: [RequireLoginGuard] },
    { path: 'test', loadChildren: './hcs-test/hcs-test.module#HcsTestModule' }
    // { path: '**', redirectTo: '' }
];
