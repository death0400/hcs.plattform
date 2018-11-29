import { Router } from '@angular/router';
import { MenuItem } from './hcs-lib/MenuItem';
import { Permission } from './hcs-lib/Permission';
export class MenuFactorys {
    public factorys: ((router: Router, permission: Permission) => MenuItem[])[];
    constructor(...providers: ((router: Router, permission: Permission) => MenuItem[])[]) {
        this.factorys = providers;
    }
}
export class Menu {
    public menuItems: MenuItem[];
    constructor(router: Router, permission: Permission, menuFactorys: MenuFactorys) {
        this.menuItems = menuFactorys.factorys.map(x => x(router, permission)).reduce((arr, newItems) => {
            newItems.forEach(n => arr.push(n));
            return arr;
        }, []);
    }
}
