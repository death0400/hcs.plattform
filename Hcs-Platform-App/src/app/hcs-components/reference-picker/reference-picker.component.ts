import { Component, OnInit, forwardRef, Input, Type, Injector, OnDestroy } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, FormControl, NgControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { IDataSource } from '../../hcs-lib/datasource/IDataSource';
import { ReferenceDialogComponent } from '../dialogs/reference-dialog/reference-dialog.component';
import { Dialog } from '../dialogs/Dialog';
import { IReferencePickerSettings } from '../IReferencePickerSettings';
import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';


@Component({
  selector: 'hcs-reference-picker',
  templateUrl: './reference-picker.component.html',
  styleUrls: ['./reference-picker.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => ReferencePickerComponent),
    multi: true
  },
  {
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => ReferencePickerComponent),
    multi: true,
  }]
})
export class ReferencePickerComponent<T> implements OnInit, ControlValueAccessor, OnDestroy, Validator {

  @Input() settings: IReferencePickerSettings = {} as any;
  @Input() dialogComponent: Type<any> = ReferenceDialogComponent;
  public entityChange = new BehaviorSubject<T>(null);
  public currentEntity: T | any;
  private _displayText = '';
  public get displayText() {
    return this._displayText;
  }
  public referenceId: any;
  public subs: Subscription[] = [];
  private ngControl: NgControl;
  public control = new FormControl(null, c => this.ngControl ? this.ngControl.errors : undefined);
  @Input() disabled: boolean;
  private onChange: (value) => void;
  private onTouched: () => void;
  @Input() displayFormatter: (entity: T) => string;
  private defaultDisplayFormatter = (entity) => this.settings.displayField ? entity[this.settings.displayField] : '';
  constructor(private dialog: Dialog, private injector: Injector) {

  }

  public validate(c: FormControl) {
    if (this.ngControl.touched) {
      this.control.markAsTouched();
    } else {
      this.control.markAsUntouched();
    }
    return undefined;
  }
  showSearchDialog() {
    this.onTouched();
    this.dialog.reference(this.settings).subscribe(x => {
      if (x !== undefined) {
        this.writeValue(x);
      }
    });
  }
  clear() {
    this.onTouched();
    this.writeValue(undefined);
  }
  public format(entity: any) {
    if (entity) {
      return (this.displayFormatter || this.settings.displayFormatter || this.defaultDisplayFormatter)(entity || {}) || '';
    }
    return '';
  }
  writeValue(obj: any): void {
    this.referenceId = obj;
    if (this.referenceId) {
      const ds = this.settings.datasource.clone();
      ds.queryOptions.skip = 0;
      ds.where(this.settings.datasource.identityProperty, '=', this.referenceId).take(1).getObservable().subscribe(e => {
        this.changeInternalValue(e.data[0]);
      });
    } else {
      this.changeInternalValue(undefined);
    }
    if (this.onChange) {
      this.onChange(obj);
    }
  }
  public changeInternalValue(entity: T) {
    this.currentEntity = entity;
    this.entityChange.next(entity);
    this._displayText = this.format(entity);
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = () => {
      this.control.markAsTouched();
      fn();
    };
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
    this.disabled ? this.control.disable() : this.control.enable();
  }
  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
    this.entityChange.complete();
  }
  ngOnInit() {
    this.ngControl = this.injector.get(NgControl) as NgControl;
  }
}
