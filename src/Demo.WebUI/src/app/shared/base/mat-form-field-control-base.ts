import {
  Input,
  HostBinding,
  Component,
  OnDestroy,
  Optional,
  Host,
  SkipSelf,
  ElementRef,
  Self,
  OnInit
} from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import {
  NgControl,
  ControlValueAccessor,
  FormControl,
  ControlContainer,
  FormGroupDirective,
  FormGroupName,
  FormArrayName,
  NgForm
} from '@angular/forms';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { MatFormFieldControl } from '@angular/material/form-field';
import { FocusMonitor } from '@angular/cdk/a11y';

@Component({
  template: ''
})
export abstract class MatFormFieldControlBase<T>
  implements MatFormFieldControl<T>, ControlValueAccessor, OnInit, OnDestroy
{
  protected static nextId: number = 0;

  public readonly controlType: string = 'app-form-field-control';
  @HostBinding()
  public readonly id: string = `${this.controlType}-${MatFormFieldControlBase.nextId++}`;

  public stateChanges = new Subject<void>();
  public focused: boolean = false;
  public touched: boolean = false;

  private _value: any;
  private _placeholder: string = '';
  private _required: boolean = false;
  private _disabled: boolean = false;

  public onChange = (_: any) => {};
  public onTouched = () => {};

  @Input() public formControl!: FormControl;
  @Input() public formControlName: string | undefined;

  protected readonly onDestroy = new Subject<void>();
  protected onDestroy$ = this.onDestroy.asObservable();

  constructor(
    @Optional() protected readonly elementRef: ElementRef<HTMLElement>,
    @Optional() protected readonly focusMonitor: FocusMonitor,
    @Host() @SkipSelf() protected readonly controlContainer: ControlContainer,
    @Optional() @Self() public ngControl: NgControl
  ) {
    if (this.ngControl != null) {
      this.ngControl.valueAccessor = this;
    }

    focusMonitor.monitor(elementRef.nativeElement, true).subscribe((origin) => {
      this.focused = !!origin;
      this.stateChanges.next();
    });
  }

  public ngOnInit(): void {
    if (this.formControl === undefined) {
      if (this.formControlName != undefined) {
        this.formControl = this.controlContainer.control!.get(this.formControlName)! as FormControl;
      }
    }

    let formGroupDirective: FormGroupDirective;
    if (
      this.controlContainer instanceof FormGroupName ||
      this.controlContainer instanceof FormArrayName
    ) {
      formGroupDirective = <FormGroupDirective>this.controlContainer.formDirective;
    } else {
      formGroupDirective = <FormGroupDirective>this.controlContainer;
    }
    formGroupDirective.ngSubmit.pipe(takeUntil(this.onDestroy$)).subscribe(() => {
      if (!this.touched) {
        this.touched = true;
        this.onTouched();
        this.stateChanges.next();
      }
    });
  }

  public writeValue(value: any): void {
    this._value = value;
  }

  public registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  private _describedBy: string | undefined;
  @HostBinding('attr.aria-describedby')
  public get describedBy() {
    return this._describedBy ?? '';
  }

  public set describedBy(value: string) {
    this._describedBy = value;
  }

  @HostBinding('class.floating')
  public get shouldLabelFloat() {
    return this.focused || !this.empty;
  }

  public setDescribedByIds(ids: string[]) {
    this.describedBy = ids.join(' ');
  }

  public onContainerClick(event: MouseEvent) {
    if ((event.target as Element).tagName.toLowerCase() != 'input') {
      (
        (event.target as Element).querySelector(
          'input:not([type=hidden]):not([hidden])'
        ) as HTMLElement
      )?.focus();
    }
  }

  public get errorState(): boolean {
    return this.formControl.invalid && this.touched;
  }

  public get empty() {
    return this._value == null;
  }

  public get required() {
    return this._required;
  }

  @Input()
  public set required(value: boolean) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }

  public get placeholder() {
    return this._placeholder;
  }

  @Input()
  public set placeholder(value: string) {
    this._placeholder = value;
    this.stateChanges.next();
  }

  public get disabled() {
    return this._disabled;
  }

  @Input()
  public set disabled(value: boolean) {
    this._disabled = coerceBooleanProperty(value);
    this.stateChanges.next();
  }

  public set value(value: any | null) {
    if (this._value !== value) {
      this._value = value;
      this.onChange(this._value);
      this.touched = true;
      this.onTouched();
      this.stateChanges.next();
    }
  }

  public get value() {
    return this._value;
  }

  public ngOnDestroy(): void {
    this.stateChanges.complete();
    this.focusMonitor.stopMonitoring(this.elementRef.nativeElement);

    this.onDestroy.next();
    this.onDestroy.complete();
  }
}
