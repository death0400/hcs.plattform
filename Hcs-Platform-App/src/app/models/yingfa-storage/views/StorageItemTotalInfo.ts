import { ItemStatus } from '../enums/ItemStatus';
import { StorageItemStatus } from '../enums/StorageItemStatus';
import { ItemType } from '../enums/ItemType';
import { Color } from '../Color';
import { Item } from '../Item';
import { Storage } from '../Storage';
import { Key, Property, ApiEntry } from '../../../hcs-lib/ApiEntry';

export class StorageItemTotalInfo {
    TotalWeight: number;
    TotalPrice: number;
}

@ApiEntry('api/entity/StorageItemTotalInfo')
export class StorageItemTotalInfoSelect {
    @Key()
    Id: number;
    @Property()
    ItemId: number;
    @Property()
    Item: Item;
    @Property()
    StorageId: number;
    @Property()
    Storage: Storage;
    @Property()
    Type: ItemType;
    @Property()
    ColorId: number | null;
    @Property()
    Color: Color;
    @Property()
    Status: ItemStatus;
    @Property()
    Length: number;
    @Property()
    Quantity: number;
    @Property()
    StorageItemStatus: StorageItemStatus;
}
