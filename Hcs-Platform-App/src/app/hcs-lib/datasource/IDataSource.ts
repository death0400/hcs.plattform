import { Observable } from 'rxjs';
import { DataSourceResponse } from './DataSourceResponse';
import { IDatasourceFilter, IFilterOperator } from './IDatasourceFilter';
import { QueryOptions } from './QueryOptions';
export type FilterDelegate<T> = (element: T) => boolean;
export interface IDataSource<T> {
  identityProperty?: string;
  queryOptions?: QueryOptions;
  getObservable(): Observable<DataSourceResponse<T>>;
  skip(n: number): IDataSource<T>;
  take(n: number): IDataSource<T>;
  orderby(property: string, direction?: 'asc' | 'desc'): IDataSource<T>;
  select(properties: string): IDataSource<T>;
  include(properties: string);
  where(filter: IDatasourceFilter | string | FilterDelegate<T>);
  where(property: string | IDatasourceFilter | FilterDelegate<T> | ((entity: T) => any), operator: IFilterOperator, parameter: any): IDataSource<T>;
  clone(): IDataSource<T>;
}
