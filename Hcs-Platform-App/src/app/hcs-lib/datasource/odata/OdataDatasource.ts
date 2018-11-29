import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { Type } from '@angular/core';
import { IDataSource, FilterDelegate } from '../IDataSource';
import { DataSourceResponse } from '../DataSourceResponse';
import { QueryOptions } from '../QueryOptions';
import { OdataUrlBuilder } from './OdataUrlBuilder';
import { IDatasourceFilter, IFilterOperator } from '../IDatasourceFilter';
import { DatasourceOrder } from '../DatasourceOrder';
import { OdataFilter } from './OdataFilter';
import { IHttpDataSource } from '../IHttpDataSource';
import { ConstructorOf } from '../../ConstructorOf';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IApiEntry } from '../../ApiEntry';
import { UserState } from '../../UserState';

/**
 * Odata資料源
 */
export class OdataDataSource<T> implements IDataSource<T>, IHttpDataSource<T> {
  /**
   * 識別屬性
   */
  public identityProperty: string;
  /**
   * 屬性映射(系統內部使用)
   */
  public porperties: { [property: string]: string };
  public properitesName: { [property: string]: string };

  /**
   * 產生新的Odata資料源
   * @param http HttpClient
   * @param ctor Model
   * @param queryOptions 查詢條件(可省略)
   * @param identityProperty 識別屬性(可由model decorator提供)
   */
  constructor(private http: HttpClient, private ctor: ConstructorOf<T> | string,
    public queryOptions?: QueryOptions, identityProperty?: string) {
    if (!this.queryOptions) {
      this.queryOptions = new QueryOptions();
    }
    if (identityProperty) {
      this.identityProperty = identityProperty;
    } else if ((<IApiEntry><any>ctor).apiEntry) {
      const apiEntry = (<IApiEntry><any>ctor).apiEntry;
      this.identityProperty = apiEntry.identityProperty;
      this.queryOptions.expand = this.combineProperty(this.queryOptions.expand, apiEntry.expand.map(x => x.replace(/\./g, '/'))).join(',');
      this.queryOptions.select = this.combineProperty(this.queryOptions.select, apiEntry.select.map(x => x.replace(/\./g, '/'))).join(',');
    }
    if (typeof ctor === 'function') {
      const m = new ctor();
      this.porperties = m['$$properties'] || {};
      this.properitesName = m['$$properties_name'] || {};
    }
  }
  /**
   * 取得API URL
   */
  public get apiUrl() {
    if (typeof this.ctor === 'string') {
      return this.ctor;
    } else if (this.ctor['apiEntry']) {
      const url = (<IApiEntry><any>this.ctor).apiEntry.path;
      if (!url) {
        throw new Error('cant not found api url from ' + this.ctor.name);
      }
      return url.replace(/[\/]$/, '');
    } else {
      throw new Error('cant not found api url from ' + this.ctor.name);
    }
  }
  public getOdataParams() {
    const odataUrlBuilder = new OdataUrlBuilder(this.queryOptions.clone());
    const params = {};
    odataUrlBuilder.apply(params);
    return params;
  }
  /**
   * 轉為RxJs Observable
   */
  public getObservable(): Observable<DataSourceResponse<T>> {
    if (!this.apiUrl) {
      throw new Error('api url is null');
    }
    const subject = new Subject<DataSourceResponse<T>>();
    const headers = {};
    this.http.get<T[]>(this.apiUrl, { params: this.getOdataParams(), headers: headers, observe: 'response' }).subscribe(resp => {
      let count: number;
      const hc = resp.headers.get('x-hcs-platform-query-total-count');
      if (hc) {
        count = parseInt(hc, 10);
      }
      subject.next(new DataSourceResponse(resp.body, count));
    });
    return subject;
  }
  public createNext(http: HttpClient, ctor: ConstructorOf<T> | string, queryOptions?: QueryOptions, identityProperty?: string) {
    const nds = new OdataDataSource<T>(http, ctor, queryOptions, identityProperty);
    return nds;
  }
  /**
   * 跳過幾筆
   * @param n 筆數
   */
  public skip(n: number): OdataDataSource<T> {
    const next = this.queryOptions.clone();
    next.skip = n;
    const nds = this.createNext(this.http, this.ctor, next, this.identityProperty);
    return nds;
  }
  /**
   * 取幾筆
   * @param n 筆數
   */
  public take(n: number): OdataDataSource<T> {
    const next = this.queryOptions.clone();
    next.take = n;
    return this.createNext(this.http, this.ctor, next, this.identityProperty);
  }
  /**
   * 排序
   * @param property 屬性
   * @param direction 方向
   */
  public orderby(property: string | ((entity: T) => any), direction?: 'asc' | 'desc'): OdataDataSource<T> {
    let p: string;
    if (typeof property === 'function') {
      p = property(this.properitesName as any as T) as string;
    } else if (typeof property === 'string') {
      p = property;
    }
    const next = this.queryOptions.clone();
    next.orderby.push(new DatasourceOrder(p, direction));
    return this.createNext(this.http, this.ctor, next, this.identityProperty);
  }
  protected combineProperty(a: string[] | string, b: string[]): string[] {
    if (!a) { return b; }
    if (typeof a === 'string') {
      a = a.split(',');
    }
    return a.concat(b.filter(x => a.indexOf(x) === -1));
  }

  /**
   * Select新元素
   * @param properties 要Select的屬性 逗號分隔
   */
  public select(properties: string) {
    if (properties) {
      properties = properties.replace(/\./g, '/');
    }
    const next = this.queryOptions.clone();
    const old = (next.select || '').split(',').filter(x => x).map(x => x.replace(/^\s+|^\s*\,|\s+$|\s*\,$/g, ''));
    const news = properties.split(',').map(x => x.replace(/^\s+|^\s*\,|\s+$|\s*\,$/g, ''));
    next.select = this.combineProperty(old, news).join(',');
    return this.createNext(this.http, this.ctor, next, this.identityProperty);
  }
  /**
   * 貪婪載入屬性
   * @param properties 要載入的屬性
   */
  public include(properties: string) {
    if (properties) {
      properties = properties.replace(/\./g, '/');
    }
    const next = this.queryOptions.clone();
    const old = next.expand.split(',').filter(x => x);
    next.expand = old.concat(properties.split(',').map(x => x.replace(/^\s+|^\s*\,|\s+$|\s*\,$/g, '')).filter(x => old.indexOf(x))).join(',');
    return this.createNext(this.http, this.ctor, next, this.identityProperty);
  }
  /**
   * add filter to request
   * @param property
   * @param operator
   * @param parameter
   */
  public where(property: string | ((entity: T) => any), operator: '>' | '>=' | '<' | '<=', parameter: Date | number): OdataDataSource<T>;
  public where(property: string | ((entity: T) => any), operator: '=' | '!=', parameter: Date | number | string | boolean): OdataDataSource<T>;
  public where(property: string | ((entity: T) => any), operator: 'in' | 'notIn', parameter: Date[] | number[] | string[]): OdataDataSource<T>;
  public where(property: string | ((entity: T) => any), operator: 'contains' | 'startswith' | 'endwith', parameter: string): OdataDataSource<T>;
  public where(filter: IDatasourceFilter | string): OdataDataSource<T>;
  /**
   * 過濾資料源
   * @param filter 過濾器
   * @param operator 運算子(filter 為 IDatasourceFilter 不使用)
   * @param parameter 值(filter 為 IDatasourceFilter 不使用)
   */
  public where(filter: string | IDatasourceFilter | OdataFilter | FilterDelegate<T>, operator?: IFilterOperator, parameter?: any): OdataDataSource<T> {
    const next = this.queryOptions.filterLogic.clone();
    if (typeof filter === 'object') {
      filter['propertyType'] = this.porperties[filter.property];
      next.filters.push(filter);
    } else if (typeof filter === 'function' && arguments.length > 1) {
      const p = (filter as any as ((a: any) => string))(this.properitesName);
      if (p === undefined) {
        throw new Error('property undefined');
      }
      const f = new OdataFilter(p, operator, parameter);
      f.propertyType = (filter as any as ((a: any) => string))(this.porperties);
      next.filters.push(f);
    } else if (typeof filter === 'string' && arguments.length > 1) {
      const f = new OdataFilter(filter, operator, parameter);
      f.propertyType = this.porperties[filter];
      next.filters.push(f);
    } else if (typeof filter === 'string') {
      next.filters.push(filter);
    }
    return this.createNext(this.http, this.ctor, this.queryOptions.clone(next), this.identityProperty);
  }
  /**
   * 複製DataSource
   */
  public clone(): IDataSource<T> {
    return this.createNext(this.http, this.ctor, this.queryOptions.clone(), this.identityProperty);
  }

  /**
   * 組合URL
   */
  public compineUrl(url: string, ...append: string[]) {
    const urlParts = url.split('?');
    urlParts[0] = urlParts[0].replace(/[\/]$/, '');
    urlParts[0] += `/${append.join('/')}`;
    if (urlParts[1]) {
      urlParts[1] = `?${urlParts[1]}`;
    }
    return urlParts.join('');
  }
  /**
   * 取得單筆
   */
  public get(key: any): Observable<T> {
    return this.http.get<T>(this.compineUrl(this.apiUrl, key));
  }
  /**
   * 刪除
   */
  public delete(key: any): Observable<T> {
    return this.http.delete<T>(this.compineUrl(this.apiUrl, key));
  }
  /**
   * 修改
   */
  public update(key: any, model: T): Observable<T> {
    return this.http.put<T>(this.compineUrl(this.apiUrl, key), model);
  }
  /**
   * 新增
   */
  public create(model: T): Observable<T> {
    return this.http.post<T>(this.apiUrl, model);
  }
}
