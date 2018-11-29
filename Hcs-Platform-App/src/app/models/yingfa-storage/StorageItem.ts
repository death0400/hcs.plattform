import { ItemType } from './enums/ItemType';
import { ItemStatus } from './enums/ItemStatus';
import { StorageItemStatus } from './enums/StorageItemStatus';
import { Color } from './Color';
import { Item } from './Item';
import { Storage } from './Storage';
import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.StorageItem')

export class StorageItem {
    @Key()
    Id: number;
    @Property()
    ItemId: number;
    Item: Item;
    @Property()
    StorageId: number;
    Storage: Storage;
    @Property()
    Type: ItemType;
    @Property()
    ColorId: number | null;
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
