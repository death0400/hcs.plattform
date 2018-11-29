import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { IDataSource, FilterDelegate } from '../IDataSource';
import { Subject } from 'rxjs';
import { DataSourceResponse } from '../DataSourceResponse';
import { IDatasourceFilter, IFilterOperator } from '../IDatasourceFilter';
import { QueryOptions } from '../QueryOptions';
import { DatasourceOrder } from '../DatasourceOrder';

export class MemoryDatasource<T> implements IDataSource<T> {


  constructor(protected buffer: T[], public queryOptions?: QueryOptions, public identityProperty?: string) {
    if (!queryOptions) {
      this.queryOptions = new QueryOptions();
    }

  }
  protected dynamicSort(orders: DatasourceOrder[]) {
    return function (a, b) {
      let result = 0;
      for (const o of orders) {
        if (result === 0) {
          result = (a[o.property] < b[o.property]) ? (o.direction === 'asc' ? -1 : 1) : (a[o.property] > b[o.property]) ? (o.direction === 'asc' ? 1 : -1) : 0;
        }
      }

      return result;
    };
  }
  getObservable(): Observable<DataSourceResponse<T>> {
    let result = this.buffer.slice(0);
    if (this.queryOptions.orderby && this.queryOptions.orderby.length) {
      result = result.sort(this.dynamicSort(this.queryOptions.orderby));
    }
    if (this.queryOptions.skip) {
      result = result.slice(this.queryOptions.skip);
    }
    if (this.queryOptions.take) {
      result = result.slice(0, this.queryOptions.take);
    }

    return of(result).pipe(map(x => new DataSourceResponse<T>(x, this.buffer.length)));
  }
  public skip(n: number): MemoryDatasource<T> {
    const next = this.queryOptions.clone();
    next.skip = n;
    return new MemoryDatasource(this.buffer, next, this.identityProperty);
  }
  public take(n: number): MemoryDatasource<T> {
    const next = this.queryOptions.clone();
    next.take = n;
    return new MemoryDatasource(this.buffer, next, this.identityProperty);
  }
  public orderby(property: string, direction?: 'asc' | 'desc'): MemoryDatasource<T> {
    const next = this.queryOptions.clone();
    next.orderby.push(new DatasourceOrder(property, direction));
    return new MemoryDatasource(this.buffer, next, this.identityProperty);
  }
  select(properties: string) {
    const propertiesAr = properties.split(',');
    for (let i = 0, c = propertiesAr.length; i < c; i++) {
      const pathes = propertiesAr[i].split('.');
      const name = pathes[pathes.length - 1];
      propertiesAr[i] = `${name} : x.${propertiesAr[i]}`;
    }
    const selector = new Function('x', `return {${propertiesAr.join(',')}};`);
    const newBuffer = this.buffer.map(value => selector(value));
    return new MemoryDatasource(newBuffer, null, this.identityProperty);
  }

  include(properties: string) {
    return;
  }
  where(property: string | IDatasourceFilter | FilterDelegate<T>, operator?: IFilterOperator, parameter?: any): IDataSource<T> {
    if (typeof property !== 'function') {
      throw new Error('support delegate filter only');
    }
    return new MemoryDatasource(this.buffer.filter(property), this.queryOptions.clone(), this.identityProperty);
  }
  clone(): IDataSource<T> {
    return new MemoryDatasource(this.buffer, this.queryOptions.clone(), this.identityProperty);
  }

}
