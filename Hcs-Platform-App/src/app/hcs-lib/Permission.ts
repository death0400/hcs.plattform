import { UserState } from './UserState';
import { Inject, Optional, FactoryProvider } from '@angular/core';
import { HCS_FUNCTION_NAME } from './Tokens';
import { Debug } from './Debug';
export class Permission {
    public static provider = {
        provide: Permission,
        useClass: Permission,
        deps: [UserState, Debug, HCS_FUNCTION_NAME]
    };
    constructor(private user: UserState, private debug: Debug, @Inject(HCS_FUNCTION_NAME) private functionName: string) {

    }
    public hasPermission(permission: string): boolean {
        let pkey = permission;
        if (this.user.roles.indexOf(pkey) >= 0) {
            return true;
        }
        if (this.functionName) {
            pkey = `${this.functionName}.${permission}`;
        }
        const result = this.user.roles.indexOf(pkey) >= 0;
        return result;
    }
}
