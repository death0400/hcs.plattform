import { Component, Input, OnChanges, SimpleChanges, OnDestroy, HostBinding, Output, EventEmitter, Optional, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormArray, FormControl, AbstractControl } from '@angular/forms';
import { IHttpDataSource } from '../../hcs-lib/datasource/IHttpDataSource';
import { Subscription, combineLatest, ReplaySubject, Observable } from 'rxjs';
import { first, filter } from 'rxjs/operators';
import { Dialog } from '../dialogs/Dialog';
import { Permission } from '../../hcs-lib/Permission';
import { HttpErrorResponse } from '@angular/common/http';
import { QueryStringHelper } from '../../hcs-lib/QueryStringHelper';
import { MAT_DIALOG_DATA } from '@angular/material';
import { IFormDialogData } from '../IFormDialogData';
import { HCS_FUNCTION_NAME, HCS_FUNCTION_ROUTE } from '../../hcs-lib/Tokens';
import { FunctionRoute } from '../FunctionRoute';
import { ErrorHelper } from '../../hcs-lib/ErrorHelper';
export type FormState = 'edit' | 'copy' | 'new';
@Component({
  selector: 'hcs-form-page',
  templateUrl: './form-page.component.html',
  styleUrls: ['./form-page.component.scss'],
  providers: [Permission.provider]
})
export class FormPageComponent<T> implements OnChanges, OnDestroy {
  @Input() public formGroup: FormGroup;
  @Input() public dataSource: IHttpDataSource<T>;
  @Input() public modelPreprocess: (model: T, type: FormState) => T;
  @Input() public modelPostprocess: (model: T, type: FormState) => T;
  @Input() public backWhenSaved = true;
  @Input() public backToView = false;
  @Input() public backToRoute: string[];
  @Input() public set fieldI18nPrefix(value: string) { this.errors.fieldI18nPrefix = value; }
  public get fieldI18nPrefix() { return this.errors.fieldI18nPrefix; }
  @Output() public modelSaved = new EventEmitter();
  @Input() @HostBinding('class.auto-border-color') border = true;
  @Input() @HostBinding('class.auto-shadow') shadow = true;
  public dataId: any;
  public copyId: any;
  public get copyIdOrDataId() {
    if (this.state === 'copy') {
      return this.copyId;
    } else {
      return this.dataId;
    }
  }
  @HostBinding('class.form-readonly') public isReadonly = false;

  private sub = new ReplaySubject<boolean>();
  public get isReadonlyState(): Observable<boolean> { return this.sub; }
  private routeSub: Subscription;
  private setup = false;
  public originalModel: T;
  public isDialog = false;
  public state: FormState;
  public errors = new ErrorHelper();
  constructor(private route: ActivatedRoute, private queryStringHelper: QueryStringHelper, private router: Router,
    @Inject(HCS_FUNCTION_NAME) public fn, @Optional() @Inject(HCS_FUNCTION_ROUTE) public fr, public functionRoute: FunctionRoute,
    private dialog: Dialog, public permission: Permission, @Optional() @Inject(MAT_DIALOG_DATA) private dialogData: IFormDialogData) {
    this.isDialog = dialogData && dialogData.isDialog;
  }
  private loadModel(model: any) {
    if (this.modelPreprocess) {
      model = this.modelPreprocess(model, this.state);
    }
    this.originalModel = model;
    this.resetForm();
    if (this.isDialog) {
      this.isReadonly = this.dialogData.isReadonly;
      if (this.isReadonly) {
        this.formGroup.disable({ onlySelf: false });
      } else {
        this.formGroup.enable({ onlySelf: false });
      }
      if (this.dialogData.fillProperty && this.state === 'new') {
        this.queryStringHelper.fillForm(this.formGroup, this.dialogData.fillProperty);
      }
      this.sub.next(this.isReadonly);
    } else {
      this.route.data.subscribe(x => {
        this.isReadonly = x['readonly'];
        if (this.isReadonly) {
          this.formGroup.disable({ onlySelf: false });
        } else {
          this.formGroup.enable({ onlySelf: false });
        }
        this.sub.next(this.isReadonly);
      }).unsubscribe();
      this.route.queryParams.subscribe(x => {
        this.queryStringHelper.fillForm(this.formGroup, x);
      }).unsubscribe();
    }
  }
  public saved() {
    this.modelSaved.emit(this.dataId);
    if (this.backWhenSaved && !this.isDialog) {
      if (this.backToView) {
        this.backView();
      } else {
        this.backList();
      }
    }
  }
  public backList() {
    this.router.navigate(this.backToRoute || this.functionRoute.getFunctionRoute(this.fn, this.fr), { queryParamsHandling: 'merge' });
  }
  public backView() {
    this.router.navigate(this.functionRoute.getFunctionRoute(this.fn, this.fr).concat([this.dataId]).filter(x => x), { queryParamsHandling: 'merge' });
  }
  public setupForm() {
    if (this.setup) {
      return;
    }
    this.setup = true;
    if (this.isDialog) {
      this.setupFormInernal(this.dialogData.id, this.dialogData.copyId);
    } else {
      this.routeSub = this.route.params.subscribe(x => {
        this.setupFormInernal(x['id'], x['copy']);
      });
    }
  }
  private setupFormInernal(id: any, copyId: any) {
    this.copyId = copyId;
    this.dataId = id;
    if (id) {
      this.state = 'edit';
      this.dataSource.get(id).subscribe(resp => {
        this.loadModel(resp);
      });
    } else if (copyId) {
      this.state = 'copy';
      this.dataSource.get(copyId).subscribe(resp => {
        resp[this.dataSource.identityProperty] = undefined;
        this.loadModel(resp);
      });
    } else {
      this.state = 'new';
      this.loadModel({});
    }
  }

  ngOnDestroy(): void {
    this.sub.complete();
    if (this.routeSub) {
      this.routeSub.unsubscribe();
    }
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (this.formGroup && this.dataSource) {
      this.setupForm();
    }
  }
  public resetForm() {
    this.errors.clear();
    this.formGroup.reset(this.originalModel, { onlySelf: false });
  }
  protected markAsTouched(group: FormGroup | FormArray) {
    Object.keys(group.controls).map(x => group.get(x)).forEach(control => {
      control.markAsTouched();
      control.updateValueAndValidity({ onlySelf: true });
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markAsTouched(control);
      }
    });
  }
  public saveForm() {
    this.errors.clear();
    this.markAsTouched(this.formGroup);
    const replay = new ReplaySubject<boolean>();
    if (this.formGroup.valid) {
      const model = this.modelPostprocess ? this.modelPostprocess(this.formGroup.getRawValue(), this.state) : this.formGroup.getRawValue();
      if (!model[this.dataSource.identityProperty]) {
        model[this.dataSource.identityProperty] = 0;
      }
      let ob: Observable<any>;
      switch (this.state) {
        case 'edit':
          ob = this.dataSource.update(this.dataId, model);
          break;
        case 'new':
        case 'copy':
          ob = this.dataSource.create(model);
          break;
      }
      ob.subscribe(r => {
        this.dataId = r[this.dataSource.identityProperty];
        replay.next(true);
      }, (error: HttpErrorResponse) => {
        this.errors.addHttpError(error);
        replay.next(false);
      }, () => replay.complete());
    } else {
      this.errors.addError(this.formGroup);
    }
    replay.pipe(first()).subscribe(successed => {
      if (successed) {
        this.saved();
      }
    });
  }
}
