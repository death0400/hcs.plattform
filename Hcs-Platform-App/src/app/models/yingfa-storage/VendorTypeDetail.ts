import { VendorType } from './enums/VendorType';
import { Vendor } from './Vendor';

export class VendorTypeDetail {
    Id: number;
    VendorId: number;
    Type: VendorType;
    Vendor: Vendor;
}
