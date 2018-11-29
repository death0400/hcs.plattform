import { StorageOutDetail } from './StorageOutDetail';
import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.StorageOut')

export class StorageOut {
    @Key()
    Id: number;
    @Property()
    StockOutTime: Date;
    @Property()
    Remark: string;
    @Property()
    StorageOutDetail: StorageOutDetail[];
}
