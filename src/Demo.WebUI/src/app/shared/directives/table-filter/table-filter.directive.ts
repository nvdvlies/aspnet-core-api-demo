import { AfterContentInit, Directive, EventEmitter, Input, OnDestroy, Output, Self } from '@angular/core';
import { NgControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, filter, Subscription } from 'rxjs';

@Directive({
  selector: '[tableFilter]'
})
export class TableFilterDirective implements AfterContentInit, OnDestroy {
  @Input() tableFilter: string | undefined;
  @Input() debounceTime: string | undefined;

  @Output() filterChange = new EventEmitter<any>();

  private subscription: Subscription | undefined;

  constructor(@Self() private ngControl: NgControl) { }

  public ngAfterContentInit(): void {
    this.subscription = this.ngControl.valueChanges?.pipe
      (
        distinctUntilChanged(),
        filter(() => this.ngControl.dirty === true),
        debounceTime(this.debounceTime ? +this.debounceTime : 0)
      )
      .subscribe(() => {
        setTimeout(() => this.filterChange.emit(this.ngControl.value));
      });
  }

  public ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
