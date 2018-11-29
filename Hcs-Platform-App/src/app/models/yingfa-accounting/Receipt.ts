import { Invoice } from './Invoice';
import { EntityStatus } from './EntityStatus';
import { Deduction } from './Deduction';
import { CashDetail } from './CashDetail';
import { ApiEntry, Key, Property, Select } from '../../hcs-lib/ApiEntry';
@ApiEntry('api/entity/Yingfa.Accounting.Model.Receipt')
export class Receipt {
    @Key()
    Id: number;
    @Property()
    InvoiceId: number;
    @Property()
    Invoice: Invoice;
    @Property()
    Date: Date | string;
    @Property()
    Amount: number;
    @Property()
    ReceiptNumber: string;
    @Property()
    ReserveAmount: number;
    @Property()
    ReceiptStatusId: number;
    @Property()
    @Select('Text', 'ColorCode')
    ReceiptStatus: EntityStatus;
    @Property()
    Comment: string;
    @Property()
    Deductions: Deduction[];
    @Property()
    CashDetails: CashDetail[];
}
