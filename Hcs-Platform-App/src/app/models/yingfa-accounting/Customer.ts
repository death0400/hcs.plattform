import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
import { CustomerContact } from './CustomerContact';
@ApiEntry('api/entity/Yingfa.Accounting.Model.Customer')
export class Customer {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    TaxNumber: string;
    @Property()
    Phone: string;
    @Property()
    Fax: string;
    @Property()
    Address: string;
    @Property()
    ContactName: string;
    @Property()
    ContactPhone: string;
    @Property()
    ContactEmail: string;
    @Property()
    Comment: string;
    CustomerContacts: CustomerContact[];
}
