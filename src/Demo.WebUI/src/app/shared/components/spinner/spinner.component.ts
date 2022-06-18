import {
  BooleanInput,
  coerceBooleanProperty,
  coerceNumberProperty,
  NumberInput
} from '@angular/cdk/coercion';
import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SpinnerComponent implements OnInit {
  private _diameter = 50;
  @Input()
  get diameter() {
    return this._diameter;
  }
  set diameter(value: NumberInput) {
    this._diameter = coerceNumberProperty(value, 0);
  }

  private _fullHeight = false;
  @Input()
  get fullHeight() {
    return this._fullHeight;
  }
  set fullHeight(value: BooleanInput) {
    this._fullHeight = coerceBooleanProperty(value);
  }

  constructor() {}

  ngOnInit(): void {}
}
