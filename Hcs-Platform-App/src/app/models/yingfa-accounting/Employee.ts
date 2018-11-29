import { Property, Key, ApiEntry, Select } from '../../hcs-lib/ApiEntry';
import { EntityStatus } from './EntityStatus';

@ApiEntry('api/entity/Yingfa.Accounting.Model.Employee')
export class Employee {
    @Key()
    Id: number;
    @Property()
    Name: string;
    @Property()
    Onboard: Date | string | null;
    @Property()
    BirthDay: Date | string | null;
    @Property()
    Phone: string;
    @Property()
    Address: string;
    @Property()
    IdentityNumber: string;
    @Property()
    FactoryId: number;
    @Select('Text', 'ColorCode')
    EmployeeStatus: EntityStatus;
    @Property()
    EmployeeStatusId: number;
}
