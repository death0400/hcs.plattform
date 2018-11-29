import { ApiEntry, Key, Property } from '../../hcs-lib/ApiEntry';

@ApiEntry('api/entity/tasks')
export class WorkTask {
    @Key()
    Id: string;
    @Property()
    Type: string;
    @Property()
    Title: string;
    @Property()
    Data: any;
}
