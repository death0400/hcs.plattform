import { Contract, ContractItem } from './Contract';
import { EntityStatus } from './EntityStatus';
import { Receipt } from './Receipt';
import { PaymentMethod } from './PaymentMethod';
import { ApiEntry, Property, Key, Select } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Accounting.Model.Invoice')
export class Invoice {
    @Key()
    Id: number;
    Period: number;
    @Property()
    Date: Date | string;
    @Property()
    Comment: string;
    @Property()
    @Select('ContractStatus.ColorCode', 'ContractStatus.Text')
    Contract: Contract;
    @Property()
    ActureAmount: number;
    @Property()
    InvoiceStatusId: number;
    @Property()
    @Select('ColorCode', 'Text')
    InvoiceStatus: EntityStatus;
    @Property()
    ContractId: number;
    @Property()
    Amount: number;
    @Property()
    Receipts: Receipt[];
    @Property()
    InvoiceItems: InvoiceItem[];
}
export class InvoiceItem {
    @Key()
    Id: number;
    @Property()
    InvoiceId: number;
    @Property()
    ContractItemId: number;
    @Property()
    ContractItem: ContractItem;
    @Property()
    Invoice: Invoice;
    @Property()
    InvoiceDetails: InvoiceDetail[];
}
export class InvoiceDetail {
    @Property()
    Id: number;
    @Property()
    PaymentMethodId: number;
    @Property()
    Quantity: number;
    @Property()
    ActureAmount: number;
    @Property()
    PaymentMethod: PaymentMethod;
    @Property()
    InvoiceItemId: number;
    @Property()
    InvoiceItem: InvoiceItem;
}
