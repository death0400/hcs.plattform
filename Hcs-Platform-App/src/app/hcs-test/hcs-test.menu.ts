import { Permission } from '../hcs-lib/Permission';
import { Router } from '@angular/router';
import { MenuItem } from '../hcs-lib/MenuItem';

export function testMenuProbider(router: Router, permission: Permission) {
    return [
        new MenuItem('menu.Test.GroupName', 'group', null, null, [
            new MenuItem('menu.Test.Customer', 'person', ['/', 'test', 'customer'], null, null, () => permission.hasPermission('PlatformModule.Test.Customer.View')),
        ], () => true, '#85C1E9')
    ];
}
