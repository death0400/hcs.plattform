import { Key, Property, ApiEntry } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Accounting.Model.PaymentMethod')
export class PaymentMethod {
    @Key()
    Id: number;
    @Property()
    Method: string;
    @Property()
    Percent: number;
    @Property()
    Comment: string;
    @Property()
    ContractId: number;
}
