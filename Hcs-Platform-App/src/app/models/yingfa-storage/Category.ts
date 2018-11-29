import { ItemType } from './enums/ItemType';
import { Item } from './Item';
import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Storage.Model.Models.Category')

export class Category {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    CategoryType: ItemType;
    Items: Item[];
}
