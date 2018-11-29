import { ApiEntry, Property } from '../../hcs-lib/ApiEntry';

@ApiEntry('api/entity/Hcs.Platform.BaseModels.PlatformUserGroup')
export class PlatformUserGroup {
    @Property()
    PlatformUserId: number;
    @Property()
    PlatformGroupId: number;
}
