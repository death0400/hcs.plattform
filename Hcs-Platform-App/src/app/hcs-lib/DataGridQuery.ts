import { IDataSource } from './datasource/IDataSource';

export type DataGridQuery<T> = (ds: IDataSource<T>, query: (next: IDataSource<T>) => void) => void;

export interface IDataGridQuery {
    query: DataGridQuery<any>;
}
