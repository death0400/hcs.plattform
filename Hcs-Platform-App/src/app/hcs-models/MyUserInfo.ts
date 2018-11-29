import { ApiEntry } from '../hcs-lib/ApiEntry';

@ApiEntry('api/entity/getmyinfo')
export class MyUserInfo {
    Id: number;
    Account: string;
    Name: string;
    Email: string;
}
