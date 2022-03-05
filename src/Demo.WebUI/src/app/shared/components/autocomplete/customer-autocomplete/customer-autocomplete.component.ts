import { FocusMonitor } from '@angular/cdk/a11y';
import { BooleanInput, coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ChangeDetectionStrategy,
  ViewChild,
  Input,
  OnDestroy,
  ElementRef,
  Optional,
  Inject,
  Self,
  OnInit,
  ViewEncapsulation
} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormControl,
  FormGroup,
  NgControl,
  Validators
} from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatFormField, MatFormFieldControl, MAT_FORM_FIELD } from '@angular/material/form-field';
import { ApiCustomersClient, CustomerLookupOrderByEnum } from '@api/api.generated.clients';
import { CustomerLookupService } from '@shared/services/customer-lookup.service';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  filter,
  finalize,
  Subject,
  Subscription
} from 'rxjs';

export class AutocompleteOption {
  constructor(public id: string, public label: string) {}
}

interface IAutocompleteForm {
  id: string;
  search: string;
}

type AutocompleteControls = { [key in keyof IAutocompleteForm]: AbstractControl };
export type AutocompleteFormGroup = FormGroup & { controls: AutocompleteControls };

@Component({
  selector: 'app-customer-autocomplete',
  templateUrl: './customer-autocomplete.component.html',
  styleUrls: ['./customer-autocomplete.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: CustomerAutocompleteComponent
    }
  ],
  host: {
    '[class.floating]': 'shouldPlaceholderFloat',
    '[id]': 'id',
    '[attr.aria-describedby]': 'describedBy'
  }
})
export class CustomerAutocompleteComponent
  implements MatFormFieldControl<string>, ControlValueAccessor, OnInit, OnDestroy
{
  static nextId = 0;
  @ViewChild('idInput') idInput!: HTMLInputElement;
  @ViewChild('searchInput') searchInput!: HTMLInputElement;

  public form: AutocompleteFormGroup;
  public stateChanges = new Subject<void>();
  public focused = false;
  public touched = false;
  public controlType = 'app-customer-autocomplete';
  public id = `app-customer-autocomplete-${CustomerAutocompleteComponent.nextId++}`;
  public onChange = (_: any) => {};
  public onTouched = () => {};

  public get empty() {
    return this.value == null;
  }

  public get shouldLabelFloat() {
    return (
      this.focused || this.form.controls.search.value?.length > 0 || this.isLookupOngoing.value
    );
  }

  @Input('aria-describedby')
  public userAriaDescribedBy: string | undefined;

  @Input()
  public get placeholder(): string {
    return this._placeholder;
  }
  public set placeholder(value: string) {
    this._placeholder = value;
    this.stateChanges.next();
  }
  private _placeholder: string = '';

  @Input()
  public get required(): boolean {
    return (this._required || this.ngControl?.control?.hasValidator(Validators.required)) ?? false;
  }
  public set required(value: BooleanInput) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  private _required = false;

  @Input()
  public get disabled(): boolean {
    return this._disabled;
  }
  public set disabled(value: BooleanInput) {
    this._disabled = coerceBooleanProperty(value);
    this._disabled ? this.form.disable() : this.form.enable();
    this.stateChanges.next();
  }
  private _disabled = false;

  @Input()
  public get value(): string | null {
    return this.form.controls.id.value;
  }
  public set value(value: string | null) {
    if (value != this.form.controls.id.value) {
      this.form.controls.id.setValue(value);
      this.stateChanges.next();
      if (value != null && this.selectedOption == null) {
        this.lookup(value);
      }
      this.onChange(this.value);
    }
    if (value == null && this.form.controls.search != null) {
      this.form.controls.search.setValue(null);
    }
  }

  public get errorState(): boolean {
    return this.form.invalid && this.touched;
  }

  private readonly options = new BehaviorSubject<AutocompleteOption[]>([]);
  public options$ = this.options.asObservable();

  private _selectedOption: AutocompleteOption | undefined;
  private get selectedOption(): AutocompleteOption | undefined {
    return this._selectedOption;
  }
  private set selectedOption(value: AutocompleteOption | undefined) {
    this._selectedOption = value;
  }

  private subscription: Subscription | undefined;

  private readonly isLookupOngoing = new BehaviorSubject<boolean>(false);
  public isLookupOngoing$ = this.isLookupOngoing.asObservable();

  constructor(
    private readonly _focusMonitor: FocusMonitor,
    private readonly _elementRef: ElementRef<HTMLElement>,
    @Optional() @Inject(MAT_FORM_FIELD) public _formField: MatFormField,
    @Optional() @Self() public ngControl: NgControl,
    private readonly apiCustomersClient: ApiCustomersClient,
    private readonly customerLookupService: CustomerLookupService
  ) {
    _focusMonitor.monitor(_elementRef, true).subscribe((origin) => {
      if (this.focused && !origin) {
        this.onTouched();
      }
      this.focused = !!origin;
      this.stateChanges.next();
    });

    if (this.ngControl != null) {
      this.ngControl.valueAccessor = this;
    }

    this.form = new FormGroup({
      id: new FormControl(null),
      search: new FormControl(null)
    } as AutocompleteControls) as AutocompleteFormGroup;
  }

  public ngOnInit(): void {
    const isRequired = this.ngControl?.control?.hasValidator(Validators.required);
    if (isRequired) {
      this.form.controls.id.addValidators(Validators.required);
    }

    this.subscription = this.form.controls.search.valueChanges
      .pipe(
        filter((value) => value != null),
        filter((value) => this.selectedOption == null || value !== this.selectedOption.label),
        debounceTime(350),
        distinctUntilChanged()
      )
      .subscribe((searchTerm) => {
        this.search(searchTerm);
      });
  }

  private search(value: string | undefined): void {
    this.apiCustomersClient
      .lookup(CustomerLookupOrderByEnum.Name, false, 0, 15, value, undefined)
      .subscribe((result) => {
        this.options.next(
          result.customers?.map(
            (customer) =>
              new AutocompleteOption(customer.id!, `${customer.code} - ${customer.name}`)
          ) ?? []
        );
      });
  }

  private lookup(id: string): void {
    this.isLookupOngoing.next(true);
    this.customerLookupService
      .getById(id)
      .pipe(finalize(() => this.isLookupOngoing.next(false)))
      .subscribe((customer) => {
        if (customer) {
          this.selectedOption = new AutocompleteOption(id, `${customer.code} - ${customer.name}`);
          this.form.controls.search.setValue(this.selectedOption.label);
        }
      });
  }

  public onFocus(): void {
    if (!this.form.controls.search.value || this.form.controls.search.value.length === 0) {
      this.form.controls.search.setValue('');
    }
  }

  public onBlur(): void {
    //debugger;
    if (this.selectedOption && this.form.controls.search.value !== this.selectedOption.label) {
      if (this.form.controls.search.value && this.form.controls.search.value.length > 0) {
        this.form.controls.search.setValue(this.selectedOption.label);
      } else {
        this.selectedOption = undefined;
        this.value = null;
      }
    }
  }

  public onSelect(option: AutocompleteOption): void {
    //debugger;
    this.selectedOption = option;
    this.form.controls.search.setValue(option.label);
    this.value = option.id;
  }

  public clearSearchField(): void {
    this.value = null;
  }

  public onFocusIn(event: FocusEvent): void {
    if (!this.focused) {
      this.focused = true;
      this.stateChanges.next();
    }
  }

  public onFocusOut(event: FocusEvent): void {
    if (!this._elementRef.nativeElement.contains(event.relatedTarget as Element)) {
      this.touched = true;
      this.focused = false;
      this.onTouched();
      this.stateChanges.next();
    }
  }

  public setDescribedByIds(ids: string[]): void {
    const controlElement = this._elementRef.nativeElement.querySelector(
      '.app-autocomplete-container'
    )!;
    controlElement.setAttribute('aria-describedby', ids.join(' '));
  }

  public onContainerClick(): void {
    this._focusMonitor.focusVia(this.searchInput, 'program');
    this.onFocus();
  }

  public writeValue(value: string | null): void {
    this.value = value;
  }

  public registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  public ngOnDestroy(): void {
    this.stateChanges.complete();
    this._focusMonitor.stopMonitoring(this._elementRef);

    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
