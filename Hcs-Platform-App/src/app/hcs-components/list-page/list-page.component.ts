import { Component, OnInit, AfterContentInit, ViewChild, ContentChildren, QueryList, AfterViewInit, Input } from '@angular/core';
import { DataSourceFactory } from '../../hcs-lib/datasource/DataSourceFactory';
import { PlatformUser } from '../../models/hcs-basic-model/PlatformUser';
import { DataGridComponent } from '../data-grid/data-grid.component';
import { DataGridColumDirective } from '../data-grid-colum.directive';

@Component({
  selector: 'hcs-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.scss']
})
export class ListPageComponent {
  constructor() {
  }
}
