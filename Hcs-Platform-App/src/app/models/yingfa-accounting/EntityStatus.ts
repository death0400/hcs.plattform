import { Property, Key, ApiEntry } from '../../hcs-lib/ApiEntry';

@ApiEntry('api/entity/Yingfa.Accounting.Model.EntityStatus')
export class EntityStatus {
    @Key()
    Id: number;
    @Property()
    Text: string;
    @Property()
    For: string;
    @Property()
    IsClosedStatus: boolean;
    @Property()
    ColorCode: string;
}
