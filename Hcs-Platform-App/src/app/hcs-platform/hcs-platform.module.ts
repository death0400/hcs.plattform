import { NgModule, ModuleWithProviders, LOCALE_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PlatformTranslateLoader } from '../hcs-lib/PlatformTranslateLoader';
import { HcsComponentsModule } from '../hcs-components/hcs-components.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import {
  MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule, MatTooltipModule, MatRadioModule,
  MatSelect, MatSelectModule, MatDividerModule, MatCheckboxModule, MatDatepickerModule, MatNativeDateModule
} from '@angular/material';
import { HcsHttpInterceptor } from '../hcs-lib/HcsHttpInterceptor';
import { HttpClientStatus } from '../hcs-lib/HttpClientStatus';
import { UserState } from '../hcs-lib/UserState';
import { UserInfo } from '../hcs-lib/UserInfo';
import { registerLocaleData } from '@angular/common';
import localZhHant from '@angular/common/locales/zh-Hant';
registerLocaleData(localZhHant, 'zh-tw');
@NgModule({
  imports: [
    HttpClientModule,
    HcsComponentsModule,
    CommonModule,
    ReactiveFormsModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useClass: PlatformTranslateLoader,
        deps: [HttpClient]
      }
    }),
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatRadioModule,
    MatSelectModule,
    MatDividerModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  providers: [
    {
      provide: LOCALE_ID,
      useValue: 'zh-tw'
    },
    HcsHttpInterceptor.provider,
    UserInfo.provider],
  exports: [
    CommonModule,
    HcsComponentsModule,
    HttpClientModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    MatRadioModule,
    MatSelectModule,
    MatDividerModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  declarations: []
})
export class HcsPlatformModule { }
