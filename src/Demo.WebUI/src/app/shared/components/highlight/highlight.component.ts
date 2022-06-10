import { Component, ChangeDetectionStrategy, Input, OnChanges } from '@angular/core';

@Component({
  selector: 'app-highlight',
  templateUrl: './highlight.component.html',
  styleUrls: ['./highlight.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HighlightComponent implements OnChanges {
  @Input() value: string | undefined;
  @Input() highlight: string | undefined;

  public result: string[] = [];

  constructor() {}

  ngOnChanges(): void {
    if (this.value && this.highlight) {
      const regEx = new RegExp('(' + this.escapeRegExp(this.highlight) + ')', 'gi');
      this.result = String(this.value).split(regEx);
    } else if (this.value) {
      this.result = [this.value];
    }
  }

  private escapeRegExp(value: string): string {
    return value ? value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') : '';
  }
}
