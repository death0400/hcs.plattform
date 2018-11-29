import { Component, OnInit, Input, HostBinding, Inject } from '@angular/core';
import { Permission } from '../../../hcs-lib/Permission';
import { IHttpDataSource } from '../../../hcs-lib/datasource/IHttpDataSource';
import { HCS_FUNCTION_NAME } from '../../../hcs-lib/Tokens';

@Component({
  selector: 'hcs-default-button-list',
  templateUrl: './default-button-list.component.html',
  styleUrls: ['./default-button-list.component.scss'],
  providers: [Permission.provider]
})
export class DefaultButtonListComponent implements OnInit {
  @Input() data: IHttpDataSource<any>;
  @Input() key: any;
  @Input() queryParams: any;
  @HostBinding('attr.function-name') functionName: string;
  constructor(public permission: Permission, @Inject(HCS_FUNCTION_NAME) fn: string) {
    this.functionName = fn;
  }
  ngOnInit() {
  }

}
