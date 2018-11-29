import { ApiEntry, Key, Property } from '../../hcs-lib/ApiEntry';
import { Enums } from './enums';
@ApiEntry('api/entity/Hcs.Platform.BaseModels.PlatformUser')
export class PlatformUser {
    @Key()
    public Id: number;
    @Property()
    public Account: string;
    @Property()
    public Name: string;
    public Password: string;
    @Property()
    public Email: string;
    @Property()
    public Status: Enums.UserStatus;
}
