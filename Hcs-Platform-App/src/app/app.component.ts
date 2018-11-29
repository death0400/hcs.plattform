import { Component, OnInit, Renderer2, ViewChild, OnChanges, SimpleChanges, HostBinding, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs';
import { BreakpointState, BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { TranslateService } from '@ngx-translate/core';
import { UserState } from './hcs-lib/UserState';
import { Title } from '@angular/platform-browser';
import { Menu } from './menu';
import { MenuItem } from './hcs-lib/MenuItem';
import { MatSidenav } from '@angular/material';
import { HttpClientStatus } from './hcs-lib/HttpClientStatus';
import { PageStatusHolder, IPageStatusAccessor } from './hcs-lib/PageStatusHolder';
import { Dialog } from './hcs-components/dialogs/Dialog';

@Component({
  selector: 'hcs-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public loading = false;
  public get useDarkTheme() { return this.useDarkThemeAccessor.value; }
  public set useDarkTheme(value) { this.useDarkThemeAccessor.value = value; this.setTheme(value); console.log(value); }
  public menuOpenAccessor: IPageStatusAccessor<boolean>;
  public useDarkThemeAccessor: IPageStatusAccessor<boolean>;
  isHandset$: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  @ViewChild('drawer') public drawer: MatSidenav;
  constructor(private dialog: Dialog, private renderer: Renderer2, private breakpointObserver: BreakpointObserver,
    title: Title, translate: TranslateService, public user: UserState, public menu: Menu, public httpStatus: HttpClientStatus, private cdr: ChangeDetectorRef, private pageState: PageStatusHolder) {
    translate.setDefaultLang('zh-tw');
    translate.use('zh-tw');
    translate.get('app-title').subscribe(x => title.setTitle(x));
    this.useDarkThemeAccessor = pageState.getAccessor<boolean>('use-dark-theme', 'site');
    this.setTheme(this.useDarkTheme);
    this.menuOpenAccessor = pageState.getAccessor<boolean>('menu-open', 'site');
    httpStatus.loading.subscribe(l => {
      setTimeout(() => this.loading = l);
    });
  }

  private setTheme(value) {
    if (value) {
      this.renderer.addClass(document.body, 'dark-theme');
    } else {
      this.renderer.removeClass(document.body, 'dark-theme');
    }
  }
  public changePassword() {
    this.dialog.changePassword().subscribe(result => {
      if (result) {

      }
    });
  }
  public logout() {
    this.user.logout();
  }
  ngOnInit() {
  }

}
