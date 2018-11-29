import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Accounting.Model.Account')
export class Account {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    TaxNumber: string;
}
