import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-inline-copy-icon',
  templateUrl: './inline-copy-icon.component.html',
  styleUrls: ['./inline-copy-icon.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InlineCopyIconComponent {
  @Input()
  public value: string | undefined;

  constructor(private readonly clipboard: Clipboard, private readonly snackBar: MatSnackBar) {}

  public copy(): void {
    if (this.value == null) {
      return;
    }
    const copied = this.clipboard.copy(this.value);
    if (copied) {
      this.snackBar.open('Copied to clipboard.', undefined, { duration: 1000 });
    }
  }
}
