import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import {
  MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule,
  MatListModule, MatFormFieldModule, MatInputModule, MatCardModule, MatSlideToggleModule, MatLineModule, MatProgressSpinnerModule, MAT_LABEL_GLOBAL_OPTIONS
} from '@angular/material';
import { AppComponent } from './app.component';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { IndexComponent } from './index/index.component';
import { RouterModule, Router } from '@angular/router';
import { appRoutes } from './app.route';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { PlatformTranslateLoader } from './hcs-lib/PlatformTranslateLoader';
import { I18nIndex } from './hcs-lib/I18nIndex';
import { LoginComponent } from './login/login.component';
import { UserState } from './hcs-lib/UserState';
import { RequireLoginGuard } from './hcs-lib/RequireLoginGuard';
import { HcsComponentsModule } from './hcs-components/hcs-components.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Menu, MenuFactorys } from './menu';
import { DataSourceFactory } from './hcs-lib/datasource/DataSourceFactory';
import { HcsHttpInterceptor } from './hcs-lib/HcsHttpInterceptor';
import { HttpClientStatus } from './hcs-lib/HttpClientStatus';
import { PasswordHash } from './hcs-lib/PasswordHash';
import { PasswordHashSha512 } from './hcs-lib/PasswordHashSha512';
import { Permission } from './hcs-lib/Permission';
import { Debug } from './hcs-lib/Debug';
import { HCS_FUNCTION_NAME, HCS_EXPORT_LIMIT, HCS_ENABLE_STATE } from './hcs-lib/Tokens';
import { MenuComponent } from './menu/menu.component';
import { PageStatusHolder } from './hcs-lib/PageStatusHolder';
import { registerLocaleData } from '@angular/common';
import localZhHant from '@angular/common/locales/zh-Hant';
import { basicMenuProbider } from './hcs-basic/menu';
import { QueryStringHelper } from './hcs-lib/QueryStringHelper';
import { Ng2GoogleChartsModule } from 'ng2-google-charts';
import { testMenuProbider } from './hcs-test/hcs-test.menu';

registerLocaleData(localZhHant, 'zh-tw');
@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    LoginComponent,
    MenuComponent
  ],
  providers: [
    {
      provide: I18nIndex,
      useValue: new I18nIndex('basic', 'hcs-test')
    },
    {
      provide: MenuFactorys,
      useValue: new MenuFactorys(basicMenuProbider, testMenuProbider)
    },
    {
      provide: LOCALE_ID,
      useValue: 'zh-tw'
    },
    QueryStringHelper,
    RequireLoginGuard,
    Debug,
    {
      provide: PasswordHash,
      useClass: PasswordHashSha512
    },
    {
      provide: HCS_ENABLE_STATE,
      useValue: true
    },
    {
      provide: HCS_FUNCTION_NAME,
      useValue: null
    },
    {
      provide: HCS_EXPORT_LIMIT,
      useValue: 300
    },
    Permission.provider,
    {
      provide: HttpClientStatus,
      useClass: HttpClientStatus
    },
    HcsHttpInterceptor.provider,
    {
      provide: UserState,
      useClass: UserState,
      deps: [HttpClient, Router, PasswordHash]
    },
    {
      provide: Menu,
      useClass: Menu,
      deps: [Router, Permission, MenuFactorys]
    },
    {
      provide: DataSourceFactory,
      useClass: DataSourceFactory,
      deps: [HttpClient]
    },
    PageStatusHolder.provider,
    {
      provide: MAT_LABEL_GLOBAL_OPTIONS,
      useValue: {
        float: 'always'
      }
    }
  ],
  imports: [
    HcsComponentsModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: PlatformTranslateLoader,
        deps: [HttpClient, I18nIndex]
      }
    }),
    RouterModule.forRoot(appRoutes),
    BrowserModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatButtonModule,
    MatLineModule,
    MatSlideToggleModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    Ng2GoogleChartsModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
