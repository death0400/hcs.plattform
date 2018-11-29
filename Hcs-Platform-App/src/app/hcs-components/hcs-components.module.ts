import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardFormComponent } from './card-form/card-form.component';
import {
  MatCardModule, MatDividerModule, MatToolbarModule, MatIconModule, MatButtonModule, MatSelectModule, MatFormFieldModule,
  MatInputModule, MatTooltipModule, MatDialogModule, MatDialog, MatSnackBarModule, MatCheckboxModule, MatBadgeModule, MatAutocompleteModule
} from '@angular/material';
import { DataGridComponent } from './data-grid/data-grid.component';
import { DataGridColumDirective } from './data-grid-colum.directive';
import { DataGridCellDirective } from './data-grid-cell.directive';
import { ListPageComponent } from './list-page/list-page.component';
import { DataGridQueryDirective } from './data-grid-query.directive';
import { PagerComponent } from './data-grid/pager/pager.component';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CopyButtonComponent } from './default-buttons/copy-button/copy-button.component';
import { EditButtonComponent } from './default-buttons/edit-button/edit-button.component';
import { DeleteButtonComponent } from './default-buttons/delete-button/delete-button.component';
import { SearchButtonComponent } from './default-buttons/search-button/search-button.component';
import { CreateButtonComponent } from './default-buttons/create-button/create-button.component';
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component';
import { Dialog } from './dialogs/Dialog';
import { FormPageComponent } from './form-page/form-page.component';
import { FormRowComponent } from './form-row/form-row.component';
import { FormFieldComponent } from './form-field/form-field.component';
import { ErrorMessagePipe } from './error-message.pipe';
import { RouterModule } from '@angular/router';
import { ErrorDialogComponent } from './dialogs/error-dialog/error-dialog.component';
import { DefaultButtonListComponent } from './default-buttons/default-button-list/default-button-list.component';
import { DefaultButtonSearchBarComponent } from './default-buttons/default-button-search-bar/default-button-search-bar.component';
import { FieldsetComponent } from './fieldset/fieldset.component';
import { DataChangeLogComponent } from './data-change-log/data-change-log.component';
import { LocalDatePipe } from './local-date.pipe';
import { ReferencePickerComponent } from './reference-picker/reference-picker.component';
import { ReferenceDialogComponent } from './dialogs/reference-dialog/reference-dialog.component';
import { ErrorComponent } from './error/error.component';
import { FormColumnComponent } from './form-column/form-column.component';
import { PaddingLeftPipe } from './padding-left.pipe';
import { FunctionRoute } from './FunctionRoute';
import { TwTaxPipe } from './tw-tax.pipe';
import { FileComponent } from './file/file.component';
import { FileManager } from '../hcs-lib/FileManager';
import { ExportToolBarComponent } from './default-buttons/export-tool-bar/export-tool-bar.component';
import { EnumOptionsPipe } from './enum-options.pipe';
import { SyncCellWidthDirective } from './sync-cell-width.directive';
import { SyncWidthDirective } from './sync-width.directive';
import { SyncScrollDirective } from './sync-scroll.directive';
import { ChangepasswordComponent } from './dialogs/changepassword/changepassword.component';
import { ErrorSummaryComponent } from './error-summary/error-summary.component';
import { AlertComponent } from './dialogs/alert/alert.component';
import { ReferenceSearchComponent } from './reference-search/reference-search.component';

@NgModule({
  imports: [
    CommonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDividerModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    TranslateModule.forChild(),
    MatSelectModule,
    MatTooltipModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatBadgeModule,
    MatAutocompleteModule
  ],
  entryComponents: [
    ConfirmDialogComponent,
    ErrorDialogComponent,
    ReferenceDialogComponent,
    ChangepasswordComponent,
    AlertComponent
  ],
  providers: [
    {
      provide: Dialog,
      useClass: Dialog,
      deps: [MatDialog]
    },
    FileManager.provider,
    FunctionRoute
  ],
  exports: [
    CardFormComponent,
    DataGridComponent,
    CopyButtonComponent,
    EditButtonComponent,
    DeleteButtonComponent,
    SearchButtonComponent,
    CreateButtonComponent,
    DataGridColumDirective,
    ListPageComponent,
    PagerComponent,
    DataGridQueryDirective,
    FormPageComponent,
    FormRowComponent,
    FormFieldComponent,
    ErrorMessagePipe,
    DefaultButtonListComponent,
    DefaultButtonSearchBarComponent,
    FieldsetComponent,
    LocalDatePipe,
    ReferencePickerComponent,
    ErrorComponent,
    FormColumnComponent,
    PaddingLeftPipe,
    TwTaxPipe,
    FileComponent,
    EnumOptionsPipe,
    SyncCellWidthDirective,
    SyncWidthDirective,
    SyncScrollDirective,
    ReferenceSearchComponent,
    MatAutocompleteModule
  ],
  declarations: [
    SyncScrollDirective,
    SyncCellWidthDirective,
    SyncWidthDirective,
    CardFormComponent,
    DataGridComponent,
    DataGridColumDirective,
    DataGridCellDirective,
    ListPageComponent,
    DataGridQueryDirective,
    PagerComponent,
    CopyButtonComponent,
    EditButtonComponent,
    DeleteButtonComponent,
    SearchButtonComponent,
    CreateButtonComponent,
    ConfirmDialogComponent,
    FormPageComponent,
    FormRowComponent,
    FormFieldComponent,
    ErrorMessagePipe,
    ErrorDialogComponent,
    DefaultButtonListComponent,
    DefaultButtonSearchBarComponent,
    FieldsetComponent,
    DataChangeLogComponent,
    LocalDatePipe,
    ReferencePickerComponent,
    ReferenceDialogComponent,
    ErrorComponent,
    FormColumnComponent,
    PaddingLeftPipe,
    TwTaxPipe,
    FileComponent,
    ExportToolBarComponent,
    EnumOptionsPipe,
    ChangepasswordComponent,
    ErrorSummaryComponent,
    AlertComponent,
    ReferenceSearchComponent
  ]
})
export class HcsComponentsModule { }
