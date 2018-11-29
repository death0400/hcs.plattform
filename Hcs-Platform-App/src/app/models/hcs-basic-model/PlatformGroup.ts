import { ApiEntry, Key, Property } from '../../hcs-lib/ApiEntry';
import { Enums } from './enums';
@ApiEntry('api/entity/Hcs.Platform.BaseModels.PlatformGroup')
export class PlatformGroup {
    @Key()
    public Id: number;
    @Property()
    public Account: string;
    @Property()
    public Name: string;
    @Property()
    public IsEnabled: boolean;
}
