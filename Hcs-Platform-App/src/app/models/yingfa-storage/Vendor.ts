import { VendorType } from './enums/VendorType';
import { Color } from './Color';
import { Item } from './Item';
import { VendorTypeDetail } from './VendorTypeDetail';
import { Key, Property, ApiEntry, Expand } from '../../hcs-lib/ApiEntry';

@ApiEntry('api/entity/Yingfa.Storage.Model.Models.Vendor')

export class Vendor {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    Type: VendorType;
    @Property()
    Contact: string;
    @Property()
    Tel: string;
    @Property()
    Fax: string;
    @Property()
    Address: string;
    @Property()
    Remark: string;
    @Property()
    Colors: Color[];
    @Property()
    Items: Item[];
    @Property()
    @Expand()
    VendorTypeDetails: VendorTypeDetail[];
}
