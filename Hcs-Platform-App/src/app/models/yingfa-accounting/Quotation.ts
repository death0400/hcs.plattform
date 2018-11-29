
import { Employee } from './Employee';
import { Customer } from './Customer';
import { Account } from './Account';
import { Key, Property, ApiEntry, Expand, Select } from '../../hcs-lib/ApiEntry';
import { EntityStatus } from './EntityStatus';
import { Contract } from './Contract';
@ApiEntry('api/entity/Yingfa.Accounting.Model.Quotation')
export class Quotation {
    @Key()
    Id: number;
    @Property()
    QuotationName: string;
    @Property()
    ReservationDescription: string;
    @Property()
    PaymentDescription: string;
    @Property()
    ConstructionSiteAddress: string;
    @Property()
    EstimateCompleteDate: Date | string | null;
    @Property()
    Contact: string;
    @Property()
    ContactPhone: string;
    @Property()
    Comment: string;
    @Property()
    AccountId: number;
    @Select('Name')
    Account: Account;
    @Property()
    EmployeeId: number;
    @Select('Name')
    Employee: Employee;
    @Property()
    ConvertedContractId: number | null;
    @Property()
    Total: number;

    @Select('Text', 'ColorCode')
    QuotationStatus: EntityStatus;
    QuotationItems: QuotationItem[];
    @Property()
    QuotationStatusId: number;
    @Property()
    CustomerId: number;
    @Select('Name')
    Customer: Customer;
    @Select('ContractNumber', 'Id')
    ConvertedContract: Contract;
}
export class QuotationItem {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    Number: string;
    @Property()
    Quantity: number;
    @Property()
    Price: number;
    @Property()
    Unit: string;
    @Property()
    QuotationId: number;
}
