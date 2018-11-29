import { Component, OnInit, Input } from '@angular/core';
import { MenuItem } from '../hcs-lib/MenuItem';
import { PageStatusHolder } from '../hcs-lib/PageStatusHolder';

@Component({
  selector: 'hcs-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  @Input() menuItems: MenuItem[];
  constructor(private pageState: PageStatusHolder) { }
  public menuClick(item: MenuItem) {
    this.pageState.clearNext();
  }
  ngOnInit() {
  }

}
