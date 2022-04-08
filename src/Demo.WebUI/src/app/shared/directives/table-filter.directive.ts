import { coerceNumberProperty, NumberInput } from '@angular/cdk/coercion';
import {
  AfterContentInit,
  Directive,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
  Self
} from '@angular/core';
import { NgControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, filter, Subscription } from 'rxjs';

@Directive({
  selector: '[appTableFilter]'
})
export class TableFilterDirective implements AfterContentInit, OnDestroy {
  @Input()
  get debounceTime() {
    return this._debounceTime;
  }
  set debounceTime(value: NumberInput) {
    this._debounceTime = coerceNumberProperty(value, 0);
  }
  private _debounceTime = 0;

  @Output() filterChange = new EventEmitter<any>();

  private subscription: Subscription | undefined;

  constructor(@Self() private ngControl: NgControl) {}

  public ngAfterContentInit(): void {
    this.subscription = this.ngControl.valueChanges
      ?.pipe(
        distinctUntilChanged(),
        filter(() => this.ngControl.dirty === true),
        debounceTime(this._debounceTime > 0 ? this._debounceTime : 0)
      )
      .subscribe(() => {
        setTimeout(() => this.filterChange.emit(this.ngControl.value));
      });
  }

  public ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
