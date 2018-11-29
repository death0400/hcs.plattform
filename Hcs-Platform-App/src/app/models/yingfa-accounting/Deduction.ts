import { Receipt } from './Receipt';

export class Deduction {
    Id: number;
    Name: string;
    Amount: number;
    ReceiptId: number;
    Receipt: Receipt;
}
