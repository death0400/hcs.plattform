import { ApiEntry } from '../hcs-lib/ApiEntry';

@ApiEntry('api/entity/changepassword')
export class ChangePasswordView {
    Id: number;
    Password: string;
    OldPassword: string;
}
