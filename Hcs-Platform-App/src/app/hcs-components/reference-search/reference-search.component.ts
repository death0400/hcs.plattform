import { Component, OnInit, ViewChild, Input, forwardRef, Injector, OnDestroy } from '@angular/core';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material';
import { IReferenceSearchSettings } from '../IReferenceSearchSettings';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, NgControl, FormControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { BehaviorSubject, Subscription, Observable } from 'rxjs';
import { ReferencePickerComponent } from '../reference-picker/reference-picker.component';

@Component({
  selector: 'hcs-reference-search',
  templateUrl: './reference-search.component.html',
  styleUrls: ['./reference-search.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => ReferenceSearchComponent),
    multi: true
  },
  {
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => ReferenceSearchComponent),
    multi: true,
  }]
})
export class ReferenceSearchComponent<T> implements OnInit, ControlValueAccessor, Validator, OnDestroy {


  constructor(private injector: Injector) {
  }

  @ViewChild('auto') public autoCompelete: MatAutocomplete;
  @Input() public settings: IReferenceSearchSettings;
  @Input() public connectWithPicker: ReferencePickerComponent<T>;
  private _filter: string;
  @Input() public set filter(value: string) {
    this._filter = value;
    if (value && value.length >= 1) {
      let ds = this.settings.datasource.clone();
      ds.queryOptions.skip = 0;
      if (value) {
        ds = ds.where(this.settings.searchField, this.settings.serachMode === 'contains' ? 'contains' : 'startswith', value);
      }
      this.optionSource = ds.take(10).getObservable();
    } else {
      this.optionSource = undefined;
    }
  }
  public get filter(): string { return this._filter; }
  public entityChange = new BehaviorSubject<T>(null);
  public optionSource: Observable<any>;
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

  public validate(c: FormControl) {
    if (this.ngControl.touched) {
      this.control.markAsTouched();
    } else {
      this.control.markAsUntouched();
    }
    return undefined;
  }
  public format(entity: any) {
    if (entity) {
      return (this.displayFormatter || this.settings.displayFormatter || this.defaultDisplayFormatter)(entity || {}) || '';
    }
    return '';
  }
  selected(event: MatAutocompleteSelectedEvent) {
    this.writeValue(event.option.value);
  }
  writeValue(obj: any): void {
    this.referenceId = obj;
    if (this.connectWithPicker) {
      this.connectWithPicker.referenceId = obj;
    }
    if (this.referenceId) {
      const ds = this.settings.datasource.clone();
      ds.queryOptions.skip = 0;
      ds.where(this.settings.datasource.identityProperty, '=', this.referenceId).take(1).getObservable().subscribe(e => {
        this.changeInternalValue(e.data[0]);
        if (this.connectWithPicker) {
          this.connectWithPicker.changeInternalValue(e.data[0]);
        }
      });
    } else {
      this.changeInternalValue(undefined);
      if (this.connectWithPicker) {
        this.connectWithPicker.changeInternalValue(undefined);
      }
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
