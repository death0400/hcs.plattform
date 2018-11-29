import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.Factory')

export class Factory {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    Remark: string;
    Storages: Storage[];
}
