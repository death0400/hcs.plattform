import { Component, OnInit, Inject } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MAT_DIALOG_DATA } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'hcs-error-dialog',
  templateUrl: './error-dialog.component.html',
  styleUrls: ['./error-dialog.component.scss']
})
export class ErrorDialogComponent implements OnInit {

  public type: string;
  public value: any;
  constructor(@Inject(MAT_DIALOG_DATA) public data: HttpErrorResponse, public sanitizer: DomSanitizer) {
    this.type = data.headers.get('content-type').split(';')[0];
    if (this.type === 'text/html') {
      this.value = this.sanitizer.bypassSecurityTrustResourceUrl(`data:text/html,${encodeURIComponent(this.data.error)}`);
    } else if (this.type === 'application/json') {
      this.value = this.processHttpError(this.data.error);
    } else {
      this.value = this.data.error;
    }
  }
  processHttpError(error: { [key: string]: { Title: string, Message: string, Data: string }[] }) {
    const buildError = (e: {
      Title: string;
      Message: string;
      Data: string;
    }[]) => {
      const errObj = {};
      e.forEach(x => errObj[x.Message] = { data: (errObj[x.Message] ? errObj[x.Message].data : undefined) || x.Data || true });
      return errObj;
    };
    const errors = [];
    Object.keys(error).forEach(x => {
      if (error[x] && error[x].length) {
        errors.push({
          field: x,
          message: buildError(error[x])
        });
      }
    });
    return errors;
  }
  ngOnInit() {
  }

}
