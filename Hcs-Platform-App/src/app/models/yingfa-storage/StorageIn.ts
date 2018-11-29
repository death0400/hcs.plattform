import { StorageInDetail } from './StorageInDetail';
import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.StorageIn')

export class StorageIn {
    @Key()
    Id: number;
    @Property()
    StockInTime: Date;
    @Property()
    Remark: string;
    @Property()
    StorageInDetail: StorageInDetail[];
}
