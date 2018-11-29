import { Employee } from './Employee';
import { Customer } from './Customer';
import { EntityStatus } from './EntityStatus';
import { ApiEntry, Key, Property, Select } from '../../hcs-lib/ApiEntry';
import { PaymentMethod } from './PaymentMethod';

@ApiEntry('api/entity/Yingfa.Accounting.Model.Contract')
export class Contract {
    ConvertFromQuotationId?: number;
    @Key()
    Id: number;
    @Property()
    ContractNumber: string;
    @Property()
    ContractName: string;
    @Property()
    CustomerId: number;
    @Property()
    EmployeeId: number;
    @Property()
    @Select('Name')
    Employee: Employee;
    @Property()
    @Select('Name')
    Customer: Customer;
    @Property()
    ReceiptTitle: string;
    @Property()
    AccountId: number;
    @Property()
    @Select('Name')
    Account: Account;
    @Property()
    ContractStatusId: number;
    @Property()
    @Select('Text', 'ColorCode')
    ContractStatus: EntityStatus;
    PaymentMethods: PaymentMethod[];
    ContractItems: ContractItem[];
    // Invoice: Invoice[];
    @Property()
    ReservationDescription: string;
    @Property()
    PaymentDescription: string;
    @Property()
    Comment: string;
    @Property()
    ConstructionSiteContactName: string;
    @Property()
    ConstructionSiteContactPhone: string;
    @Property()
    ConstructionSiteAddress: string;
    @Property()
    ContractDate: Date | string;
    @Property()
    EstimateCompleteDate: Date | string;
    @Property()
    DateOfAccounting: number;
    @Property()
    DateOfPay: number;
    @Property()
    Total: number;
    @Property()
    Arrival: number;
    @Property()
    PercentOfRequested: number;
    @Property()
    Reserved: number;
    @Property()
    Reming: number;
    @Property()
    Deducated: number;
    @Property()
    ThisPeriodRequested: number;
}

@ApiEntry('api/entity/Yingfa.Accounting.Model.Views.ContractItemView')
export class ContractItem {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    IsAttached: boolean;
    @Property()
    AttachDate: Date | string | null;
    @Property()
    Quantity: number;
    @Property()
    Price: number;
    @Property()
    Unit: string;
    @Property()
    Number: string;
    @Property()
    ContractId: number;
    UsedQuotaDetail: ContractItemUsedQuota[];
}
export class ContractItemUsedQuota {
    PaymentMethodId: number;
    UsedQuota: number;
}
