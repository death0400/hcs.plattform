import { ApiEntry, Key, Property } from '../hcs-lib/ApiEntry';

@ApiEntry('api/entity/functions')
export class PlatformFunction {
    @Key()
    public Code: string;
    @Property()
    public Permissions: string[];
}
