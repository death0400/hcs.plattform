import { Enums } from './enums';
import { ApiEntry, Key, Property } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Hcs.Platform.BaseModels.PlatformGroupRole')
export class PlatformGroupRole {
    @Key()
    public Id: number;
    @Property()
    public PlatformGroupId: number;
    @Property()
    public Permission: Enums.PermissionStatus;
    @Property()
    public FunctionCode: string;
    @Property()
    public FunctionRoleCode: string;

}
