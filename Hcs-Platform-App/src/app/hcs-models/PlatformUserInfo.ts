import { Key, Property, ApiEntry } from '../hcs-lib/ApiEntry';

@ApiEntry('api/entity/user')
export class PlatformUserInfo {
    @Key()
    Id: number;
    @Property()
    Name: string;
}
