import { IDataSource } from '../hcs-lib/datasource/IDataSource';

export interface IReferenceSearchSettings {
    displayFormatter?: (entity: any) => string;
    searchField: string;
    serachMode?: 'contains' | 'startsWith';
    displayField?: string;
    datasource: IDataSource<any>;
}
