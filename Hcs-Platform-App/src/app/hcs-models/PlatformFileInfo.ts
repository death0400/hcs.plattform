import { ApiEntry } from '../hcs-lib/ApiEntry';

@ApiEntry('api/file')
export class PlatformFileInfo {
    Name: string;
    Dir: string;
    ETag: string;
    MimeType: string;
    Length: number;
}
