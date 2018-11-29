import { ItemType } from './enums/ItemType';
import { ItemStatus } from './enums/ItemStatus';
import { Color } from './Color';
import { Item } from './Item';
import { StorageOut } from './StorageOut';
import { Storage } from './Storage';

export class StorageOutDetail {
    Id: number;
    StockOutId: number;
    StorageOut: StorageOut;
    ItemId: number;
    Item: Item;
    StorageId: number;
    Storage: Storage;
    Type: ItemType;
    ColorId: number | null;
    Color: Color;
    Status: ItemStatus;
    Length: number;
    Quantity: number;
    Remark: string;
}
