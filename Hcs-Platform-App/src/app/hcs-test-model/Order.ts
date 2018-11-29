import { Key, Property } from '../hcs-lib/ApiEntry';
import { Customer } from './Customer';

export class Order {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    CustomerId: number;
    Customer: Customer;

}
