import { ItemType } from './enums/ItemType';
import { Category } from './Category';
import { ExtrusionParts } from './ExtrusionParts';
import { StorageInDetail } from './StorageInDetail';
import { StorageItem } from './StorageItem';
import { StorageOutDetail } from './StorageOutDetail';
import { Vendor } from './Vendor';
import { Key, Property, ApiEntry, Expand } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.Item')

export class Item {
    @Key()
    Id: number;
    @Property()
    ModelName: string;
    @Property()
    VendorId: number;
    @Property()
    Vendor: Vendor;
    @Property()
    Usage: string;
    @Property()
    Image: string;
    @Property()
    Thickness: number;
    @Property()
    Type: ItemType;
    @Property()
    WeightPerKmm: number;
    @Property()
    Category: Category;
    @Property()
    CategoryId: number | null;
    @Property()
    SurfaceAreaPreM: number | null;
    @Property()
    PricePerKg: number | null;
    @Property()
    Remark: string;
    @Property()
    ExtrusionParts: ExtrusionParts[];
    ParentExtrusions: ExtrusionParts[];
    Stocks: StorageItem[];
    OutStorage: StorageOutDetail[];
    InStorage: StorageInDetail[];
}
