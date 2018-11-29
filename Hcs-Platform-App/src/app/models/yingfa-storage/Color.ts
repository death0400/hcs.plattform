import { ColorType } from './enums/ColorType';
import { PricingType } from './enums/PricingType';
import { Vendor } from './Vendor';
import { StorageItem } from './StorageItem';
import { StorageOutDetail } from './StorageOutDetail';
import { StorageInDetail } from './StorageInDetail';
import { Key, Property, ApiEntry, Expand } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.Color')

export class Color {
    @Key()
    Id: number;
    @Property()
    VendorId: number;
    @Property()
    Vendor: Vendor;
    @Property()
    Code: string;
    @Property()
    Type: ColorType;
    @Property()
    PricingType: PricingType | null;
    @Property()
    SurfaceAreaPrice: number | null;
    @Property()
    KgPrice: number | null;
    @Property()
    ColorName: string;
    @Property()
    Image: string;
    @Property()
    Remark: string;
    Stocks: StorageItem[];
    OutStorage: StorageOutDetail[];
    InStorage: StorageInDetail[];
}
