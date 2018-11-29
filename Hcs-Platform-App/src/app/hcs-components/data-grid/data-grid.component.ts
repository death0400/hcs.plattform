import { Component, OnInit, ContentChildren, AfterContentInit, OnDestroy, Input, OnChanges, SimpleChanges, HostBinding, ViewChildren, Query, ContentChild, Inject } from '@angular/core';
import { QueryList } from '@angular/core';
import { Subscription, Observable, ReplaySubject, Subject } from 'rxjs';
import { IDataSource } from '../../hcs-lib/datasource/IDataSource';
import { DataGridQuery, IDataGridQuery } from '../../hcs-lib/DataGridQuery';
import { SortDirection } from '@angular/material';
import { DataGridColumDirective } from '../data-grid-colum.directive';
import { PageStatusHolder, IPageStatusAccessor } from '../../hcs-lib/PageStatusHolder';
import { HCS_ENABLE_STATE } from '../../hcs-lib/Tokens';
export enum DataGridSortDirection {
  asc = 1,
  desc = 2
}
export interface ISortingState { field: string; direction: DataGridSortDirection; }
@Component({
  selector: 'hcs-data-grid',
  templateUrl: './data-grid.component.html',
  styleUrls: ['./data-grid.component.scss']
})
export class DataGridComponent implements OnInit, AfterContentInit, OnDestroy, OnChanges {
  @Input() public showPager = true;
  @Input() autoLoad = false;
  @Input() @HostBinding('class.auto-border-color') border = true;
  @Input() @HostBinding('class.auto-shadow') shadow = true;
  @Input() @HostBinding('class.grid-min-width') minWidth = true;
  @Input() public data: IDataSource<any>;
  @Input() @HostBinding('class.show-footer') public showFooter = true;
  @Input() @HostBinding('class.show-header') public showHeader = true;
  @Input() public autoSelect = true;
  @Input() public enableRowSelect = false;
  @Input() public statePrifix = '';
  public miniHead = false;
  public checkedModel = {};
  private _dataLoaded = new ReplaySubject<any[]>();

  public get dataLoaded(): Observable<any[]> {
    return this._dataLoaded;
  }
  public loadedData: any[];
  @ContentChildren(DataGridColumDirective) public columns: QueryList<DataGridColumDirective>;

  private querys: IDataGridQuery[] = [];
  public subs: Subscription[] = [];
  public query: DataGridQuery<any>;
  public total: number;
  public loadQueued = false;
  public loadedDataSource: IDataSource<any>;
  public get sortingState(): ISortingState[] { return this.gridSortingStateAccessor.value; }
  @Input() public set sortingState(value: ISortingState[]) { this.gridSortingStateAccessor.value = value; }
  @Input() public defaultSortState: ISortingState[];
  public sortingStateMap: { [field: string]: { order: number, direction: DataGridSortDirection } } = {};
  private gridSortingStateAccessor: IPageStatusAccessor<ISortingState[]>;
  private _onReset = new Subject<DataGridComponent>();
  public _onColumnChange = new ReplaySubject<QueryList<DataGridColumDirective>>();
  public get onColumnChange(): Observable<QueryList<DataGridColumDirective>> { return this._onColumnChange; }
  public get onReset(): Observable<DataGridComponent> { return this._onReset; }
  public reset() {
    this._onReset.next(this);
  }
  public getSelectRowCount() {
    return Object.keys(this.checkedModel).filter(x => this.checkedModel[x]).length;
  }
  public getSelectIds() {
    return Object.keys(this.checkedModel).filter(x => this.checkedModel[x]);
  }
  public clearSelected() {
    this.checkedModel = {};
  }
  @Input() public getRowStyle: (entity: any) => any = () => undefined;
  constructor(private pageState: PageStatusHolder, @Inject(HCS_ENABLE_STATE) public enableState: boolean) {
    if (enableState) {
      this.gridSortingStateAccessor = pageState.getAccessor<ISortingState[]>(`${this.statePrifix}-grid-sorting`);
    } else {
      this.gridSortingStateAccessor = { value: [] };
    }
    // sort
    this.addQuery({
      query: (ds, next) => {
        this.sortingStateMap = {};
        (this.sortingState || this.defaultSortState || []).forEach((s, i) => {
          if (typeof s.direction === 'string') {
            s.direction = DataGridSortDirection[s.direction] as any;
          }
          ds = ds.orderby(s.field, DataGridSortDirection[s.direction] as any);
          this.sortingStateMap[s.field] = { order: i + 1, direction: s.direction };
        });
        next(ds);
      }
    });
  }
  public addQuery(query: IDataGridQuery) {
    this.querys.push(query);
  }
  ngOnChanges(changes: SimpleChanges): void {

  }
  buildQuery() {
    this.query = this.querys.reverse().reduce((perviousValue, currentValue, currentIndex, array) => {
      return (ds, query) => currentValue.query(ds, nextds => perviousValue(nextds, query));
    }, (ds, query) => query(ds));
  }
  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
    this._dataLoaded.complete();
    this._onColumnChange.complete();
    this._onReset.complete();
  }
  clickColumn(column: DataGridColumDirective, event: MouseEvent) {
    if ((column.field || column.orderby) && column.sortable) {
      if (!this.sortingState) {
        this.sortingState = [];
      }
      const orderByField = column.orderby || column.field;
      let sortState = this.sortingState.filter(x => x.field === orderByField)[0];

      if (sortState) {
        if (sortState.direction === DataGridSortDirection.asc) {
          sortState.direction = DataGridSortDirection.desc;
          if (!event.shiftKey) {
            this.sortingState = [sortState];
          }
        } else if (sortState.direction === DataGridSortDirection.desc) {
          this.sortingState.splice(this.sortingState.indexOf(sortState), 1);
        }
      } else {
        sortState = { field: orderByField, direction: DataGridSortDirection.asc };
        this.sortingState.push(sortState);
        if (!event.shiftKey) {
          this.sortingState = [sortState];
        }
      }
      this.queueLoad();
    } else {
      console.warn(`column field not specify`);
    }
  }

  ngAfterContentInit(): void {
    this._onColumnChange.next(this.columns);
    this.subs.push(this.columns.changes.subscribe(x => {
      this._onColumnChange.next(this.columns);
    }));
    if (this.autoLoad) {
      this.queueLoad();
    }
  }
  public queueLoad() {
    if (this.loadQueued) {
      return;
    }
    this.loadQueued = true;
    setTimeout(() => {
      this.load();
      this.loadQueued = false;
    });
  }
  private load() {
    this.buildQuery();
    this.loadedData = undefined;
    this.query(this.data, ds => {
      if (this.autoSelect) {
        const select = this.columns.filter(y => y.field !== undefined).map(y => y.field).join(',');
        ds = ds.select(select);
      }
      this.loadedDataSource = ds;
      ds.getObservable().subscribe(resp => {
        this.total = resp.totalCount;
        this.loadedData = resp.data;
        this._dataLoaded.next(resp.data);
      });
    });
  }
  ngOnInit() {

  }

}
