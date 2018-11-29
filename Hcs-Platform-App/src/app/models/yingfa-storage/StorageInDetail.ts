import { ItemType } from './enums/ItemType';
import { ItemStatus } from './enums/ItemStatus';
import { Color } from './Color';
import { Item } from './Item';
import { StorageIn } from './StorageIn';
import { Storage } from './Storage';

export class StorageInDetail {
    Id: number;
    StockInId: number;
    StockIn: StorageIn;
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
