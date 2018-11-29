import { StorageType } from './enums/StorageType';
import { Factory } from './Factory';
import { StorageInDetail } from './StorageInDetail';
import { StorageItem } from './StorageItem';
import { StorageOutDetail } from './StorageOutDetail';
import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.Storage')

export class Storage {
    @Key()
    Id: number;
    @Property()
    FactoryId: number;
    Factory: Factory;
    @Property()
    Type: StorageType;
    @Property()
    Code: string;
    @Property()
    Remark: string;
    Stocks: StorageItem[];
    OutStorage: StorageOutDetail[];
    InStorage: StorageInDetail[];
}
