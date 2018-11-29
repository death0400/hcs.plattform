import { Component, OnInit, Input, Optional, OnDestroy, Inject } from '@angular/core';
import { DataGridComponent } from '../data-grid.component';
import { IDataSource } from '../../../hcs-lib/datasource/IDataSource';
import { PageStatusHolder, IPageStatusAccessor } from '../../../hcs-lib/PageStatusHolder';
import { Subscription } from 'rxjs';
import { HCS_ENABLE_STATE } from '../../../hcs-lib/Tokens';

@Component({
  selector: 'hcs-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit, OnDestroy {


  @Input() public pageSize = 20;
  @Input() public pageSizeOptions = [20, 50, 100];
  private pageAccessor: IPageStatusAccessor<number>;
  private pageSizeAccessor: IPageStatusAccessor<number>;
  @Input() public page = 1;
  public get total() {
    return this.grid.total;
  }
  public get from() {
    return ((this.page - 1) * this.pageSize) + 1;
  }
  public get totalPage() {
    return Math.ceil(this.grid.total / this.pageSize);
  }

  public get to() {
    if (this.grid.loadedData) {
      return ((this.page - 1) * this.pageSize) + this.grid.loadedData.length;
    }
    return 0;
  }
  private subloaded: Subscription;
  private resetSub: Subscription;
  constructor(public grid: DataGridComponent, pageState: PageStatusHolder, @Inject(HCS_ENABLE_STATE) public enableState: boolean) {
    this.grid.addQuery(this);
    if (enableState) {
      this.pageAccessor = pageState.getAccessor('pager-page');
      this.pageSizeAccessor = pageState.getAccessor('pager-pageSize');
    } else {
      this.pageAccessor = { value: undefined };
      this.pageSizeAccessor = { value: undefined };
    }
    const ps = this.pageSizeAccessor.value;
    this.resetSub = this.grid.onReset.subscribe(g => {
      this.page = 1;
    });
    if (ps) {
      this.pageSize = ps;
    }
    this.subloaded = grid.dataLoaded.subscribe(x => {
      if (x.length === 0 && this.page > 1) {
        this.go(this.page - 1);
      }
    });
  }
  ngOnDestroy(): void {
    this.subloaded.unsubscribe();
  }
  public getSerialNumber(currentPageIndex: number) {
    return (this.page - 1) * this.pageSize + currentPageIndex + 1;
  }
  savePageSize() {
    this.pageSizeAccessor.value = this.pageSize;
    this.page = 1;
    this.grid.queueLoad();
  }
  go(page: number) {
    if (page >= 1 && page <= this.totalPage) {
      this.page = page;
      this.pageAccessor.value = this.page;
      this.grid.queueLoad();
    }
  }
  query(datasource: IDataSource<any>, next: (ds: IDataSource<any>) => void) {
    datasource.queryOptions.skip = ((this.page - 1) * this.pageSize);
    datasource.queryOptions.take = this.pageSize;
    next(datasource);
  }
  ngOnInit() {
    if (this.page === 1 && this.pageAccessor.value >= 2) {
      this.page = this.pageAccessor.value;
    }
  }

}
