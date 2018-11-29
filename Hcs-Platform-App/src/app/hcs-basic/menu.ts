import { Permission } from '../hcs-lib/Permission';
import { Router } from '@angular/router';
import { MenuItem } from '../hcs-lib/MenuItem';

export function basicMenuProbider(router: Router, permission: Permission) {
    return [
        new MenuItem('menu.UserAndGroup', 'group', null, null, [
            new MenuItem('menu.PlatformUser', 'person', ['/', 'basic', 'platform-user'], null, null, () => permission.hasPermission('Basic.PlatformUser.View')),
            new MenuItem('menu.PlatformGroup', 'group', ['/', 'basic', 'platform-group'], null, null, () => permission.hasPermission('Basic.PlatformGroup.View'))
        ], () => true, '#85C1E9')
    ];
}
