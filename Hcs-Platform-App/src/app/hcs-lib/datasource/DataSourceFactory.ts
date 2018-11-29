import { OdataDataSource } from './odata/OdataDataSource';
import { MemoryDatasource } from './memory/MemoryDatasource';
import { IDataSource } from './IDataSource';
import { ConstructorOf } from '../ConstructorOf';
import { HttpClient } from '@angular/common/http';
import { Injector } from '@angular/core';
import { UserState } from '../UserState';
export class DataSourceFactory {
  constructor(private http: HttpClient) {

  }
  getDataSource<T>(ctor: T[]): MemoryDatasource<T>;
  getDataSource<T>(ctor: ConstructorOf<T> | string): OdataDataSource<T>;
  getDataSource<T>(ctor: ConstructorOf<T> | string | T[]): IDataSource<T> {
    if (typeof ctor === 'function' || typeof ctor === 'string') {
      const odataDataSource = new OdataDataSource<T>(this.http, ctor);
      return odataDataSource;
    } else if (ctor instanceof Array) {
      return new MemoryDatasource(ctor);
    }
    throw Error('not support type');
  }
}


