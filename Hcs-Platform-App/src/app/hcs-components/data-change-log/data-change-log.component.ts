import { Component, OnInit, Input, OnChanges, SimpleChanges, ChangeDetectionStrategy } from '@angular/core';
import { IPlatformEntity } from '../../hcs-platform-model/IPlatformEntity';
import { UserInfo } from '../../hcs-lib/UserInfo';

@Component({
  selector: 'hcs-data-change-log',
  templateUrl: './data-change-log.component.html',
  styleUrls: ['./data-change-log.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DataChangeLogComponent implements OnInit, OnChanges {


  @Input() public data: IPlatformEntity;
  public cname: string;
  public mname: string;
  constructor(private userinfo: UserInfo) {
  }
  ngOnChanges(changes: SimpleChanges): void {
    if ('data' in changes && changes['data'].currentValue) {
      const d = changes['data'].currentValue as IPlatformEntity;
      if (d.CreatedBy) {
        this.userinfo.getUserInfo(d.CreatedBy).subscribe(x => {
          this.cname = x.Name;
        });
      } else {
        this.cname = undefined;
      }
      if (d.LastUpdatedBy) {
        this.userinfo.getUserInfo(d.LastUpdatedBy).subscribe(x => {
          this.mname = x.Name;
        });
      } else {
        this.mname = undefined;
      }
    } else {
      this.cname = undefined;
      this.mname = undefined;
    }
  }
  ngOnInit() {
  }

}
