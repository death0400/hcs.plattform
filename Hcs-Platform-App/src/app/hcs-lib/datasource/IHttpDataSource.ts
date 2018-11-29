import { Observable } from 'rxjs';
export interface IHttpDataSource<T> {
  identityProperty: string;
  get(key: any): Observable<T>;
  delete(key: any): Observable<T>;
  update(key: any, model: T): Observable<T>;
  create(model: T): Observable<T>;
}
