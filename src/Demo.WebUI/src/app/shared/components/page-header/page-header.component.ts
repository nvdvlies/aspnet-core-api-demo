import { BooleanInput, coerceBooleanProperty } from '@angular/cdk/coercion';
import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from '@env/environment';

@Component({
  selector: 'app-page-header',
  templateUrl: './page-header.component.html',
  styleUrls: ['./page-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PageHeaderComponent {
  private _headerTitle: string | undefined;
  @Input()
  get headerTitle() {
    return this._headerTitle;
  }
  set headerTitle(value: string | undefined) {
    this._headerTitle = value;
    if (this._headerTitle) {
      this.titleService.setTitle(`${environment.appName} - ${this._headerTitle}`);
    }
  }

  private _showButtonBar = true;
  @Input()
  get showButtonBar() {
    return this._showButtonBar;
  }
  set showButtonBar(value: BooleanInput) {
    this._showButtonBar = coerceBooleanProperty(value);
  }

  constructor(private readonly titleService: Title) {}
}
