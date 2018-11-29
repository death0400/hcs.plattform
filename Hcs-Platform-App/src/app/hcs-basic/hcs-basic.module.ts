import { NgModule } from '@angular/core';
import { HcsPlatformModule } from '../hcs-platform/hcs-platform.module';
import { PlatformUserComponent } from './platform-user/platform-user.component';
import { RouterModule } from '@angular/router';
import { hcsBasicRoutes } from './hcs-basic.route';
import { PlatformUserFormComponent } from './platform-user-form/platform-user-form.component';
import { MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule } from '@angular/material';
import { PlatformGroupComponent } from './platform-group/platform-group.component';
import { PlatformGroupFormComponent } from './platform-group-form/platform-group-form.component';

@NgModule({
  imports: [
    HcsPlatformModule,
    RouterModule.forChild(hcsBasicRoutes)
  ],
  declarations: [PlatformUserComponent, PlatformUserFormComponent, PlatformGroupComponent, PlatformGroupFormComponent]
})
export class HcsBasicModule { }
