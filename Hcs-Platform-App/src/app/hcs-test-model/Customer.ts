import { Key, Property, ApiEntry } from '../hcs-lib/ApiEntry';
import { Order } from './Order';
@ApiEntry('api/entity/Hcs.PlatformModule.Test.Model.Entities.Customer')
export class Customer {
    @Key()
    Id: number;
    @Property()
    Name: string;

    Orders: Order[];

}
