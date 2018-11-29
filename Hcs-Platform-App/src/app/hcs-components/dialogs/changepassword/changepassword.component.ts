import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HcsValidators } from '../../../hcs-lib/HcsValidators';
import { DataSourceFactory } from '../../../hcs-lib/datasource/DataSourceFactory';
import { ChangePasswordView } from '../../../hcs-models/ChangePasswordView';
import { UserState } from '../../../hcs-lib/UserState';
import { ErrorHelper } from '../../../hcs-lib/ErrorHelper';
import { MatDialogRef } from '../../../../../node_modules/@angular/material';
import { PasswordHash } from '../../../hcs-lib/PasswordHash';
import { MyUserInfo } from '../../../hcs-models/MyUserInfo';

@Component({
  selector: 'hcs-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.scss']
})
export class ChangepasswordComponent implements OnInit {

  public errors = new ErrorHelper();
  public formGroup: FormGroup;
  public username: string;
  constructor(private passwordHash: PasswordHash, private ds: DataSourceFactory, private user: UserState, private dialog: MatDialogRef<boolean>) {
    this.formGroup = new FormGroup({
      OldPassword: new FormControl(null, Validators.required),
      Password: new FormControl(null, Validators.required),
      ConfirmPassword: new FormControl(null, [Validators.required, HcsValidators.sameAs('Password')])
    });
    this.ds.getDataSource(MyUserInfo).get(this.user.id).subscribe(x => {
      this.username = x.Account;
    });
  }
  applyPasswrod() {
    this.errors.fieldI18nPrefix = 'dialog.changePassword';
    this.errors.clear();
    Object.keys(this.formGroup.controls).map(x => this.formGroup.controls[x]).forEach(x => x.markAsTouched());
    if (this.formGroup.valid) {
      this.ds.getDataSource(ChangePasswordView).update(this.user.id, {
        Id: this.user.id,
        Password: this.passwordHash.hash(this.formGroup.value.Password),
        OldPassword: this.passwordHash.hash(this.formGroup.value.OldPassword)
      }).subscribe(resp => {
        this.dialog.close(true);
      }, err => this.errors.addHttpError(err));
    } else {
      this.errors.addError(this.formGroup);
    }
  }
  ngOnInit() {
  }

}
