import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { Observable, ReplaySubject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';
import { ReferenceDialogComponent } from './reference-dialog/reference-dialog.component';
import { IReferencePickerSettings } from '../IReferencePickerSettings';
import { IFormDialogData } from '../IFormDialogData';
import { ComponentType } from '@angular/cdk/portal/typings';
import { ChangepasswordComponent } from './changepassword/changepassword.component';
import { AlertComponent } from './alert/alert.component';

export class Dialog {
    constructor(private matDialog: MatDialog) {

    }
    public form<T>(data: IFormDialogData, formComponent: ComponentType<T>) {
        data.isDialog = true;
        this.matDialog.open(formComponent, {
            data: data,
            width: '90%',
            maxHeight: '90vh'
        });
    }
    public reference(data: IReferencePickerSettings): Observable<any> {
        const relay = new ReplaySubject<any>();
        const dialogRef = this.matDialog.open(ReferenceDialogComponent, {
            data: data,
            height: '80vh',
            width: '80vw'
        }).afterClosed().subscribe(result => {
            relay.next(result);
            relay.complete();
        });
        return relay;
    }
    public httpError(httpError: HttpErrorResponse) {
        const dialogRef = this.matDialog.open(ErrorDialogComponent, {
            data: httpError
        });
    }
    public alert(message: string): Observable<void> {
        const relay = new ReplaySubject<void>();
        const dialogRef = this.matDialog.open(AlertComponent, {
            data: {
                message: message
            }
        }).afterClosed().subscribe(result => {
            relay.next();
            relay.complete();
        });
        return relay;
    }
    public confirm(title: string, message: string): Observable<boolean> {
        const relay = new ReplaySubject<boolean>();
        const dialogRef = this.matDialog.open(ConfirmDialogComponent, {
            data: {
                message: message,
                title: title
            }
        }).afterClosed().subscribe(result => {
            relay.next(result);
            relay.complete();
        });
        return relay;
    }
    public changePassword(): Observable<boolean> {
        const relay = new ReplaySubject<boolean>();
        const dialogRef = this.matDialog.open(ChangepasswordComponent, {
            width: '640px'
        }).afterClosed().subscribe(result => {
            relay.next(result);
            relay.complete();
        });
        return relay;
    }
}
