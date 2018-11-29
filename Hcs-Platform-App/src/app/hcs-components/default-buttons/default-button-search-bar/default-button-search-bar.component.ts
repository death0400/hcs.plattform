import { Component, OnInit, Input, HostBinding, Inject, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { Permission } from '../../../hcs-lib/Permission';
import { DataGridComponent } from '../../data-grid/data-grid.component';
import { IPageFormAccessor, PageStatusHolder } from '../../../hcs-lib/PageStatusHolder';
import { PagerComponent } from '../../data-grid/pager/pager.component';
import { HCS_FUNCTION_NAME } from '../../../hcs-lib/Tokens';
import { FormGroup } from '@angular/forms';
import { QueryStringHelper } from '../../../hcs-lib/QueryStringHelper';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'hcs-default-button-search-bar',
  templateUrl: './default-button-search-bar.component.html',
  styleUrls: ['./default-button-search-bar.component.scss'],
  providers: [Permission.provider]
})
export class DefaultButtonSearchBarComponent implements OnInit, OnChanges, OnDestroy {


  private subs: Subscription[] = [];
  @Input() saveParams = true;
  @Input() queryParams: any;
  @Input() grid: DataGridComponent;
  @Input() filterForm: FormGroup;
  @HostBinding('attr.function-name') functionName: string;
  private filterFormAccessor: IPageFormAccessor;
  constructor(public permission: Permission, private queryStringHelper: QueryStringHelper, private route: ActivatedRoute, public pageState: PageStatusHolder, @Inject(HCS_FUNCTION_NAME) fn: string) {
    this.functionName = fn;
  }
  ngOnChanges(changes: SimpleChanges): void {
    if ('filterForm' in changes) {
      const form = changes['filterForm'].currentValue as FormGroup;
      if (form) {
        this.filterFormAccessor = this.pageState.bindFormGroup(form);
        this.subs.push(this.route.queryParams.subscribe(x => {
          form.enable();
          this.queryStringHelper.fillForm(form, x);
        }));
      } else {
        this.filterFormAccessor = null;
      }
    }
  }
  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }
  ngOnInit() {
  }
  public searchClick() {
    this.grid.reset();
    if (this.filterFormAccessor && this.saveParams) {
      this.filterFormAccessor.save();
    }
    this.grid.queueLoad();
  }
}
