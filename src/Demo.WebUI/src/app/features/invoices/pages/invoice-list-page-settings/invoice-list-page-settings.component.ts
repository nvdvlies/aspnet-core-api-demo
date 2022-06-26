import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, map, Observable, tap } from 'rxjs';
import { InvoiceListPageSettingsService } from './invoice-list-page-settings.service';

const allColumns = [
  'InvoiceNumber',
  'CustomerName',
  'InvoiceDate',
  'PaymentTerm',
  'OrderReference',
  'Status'
];

const defaultColumns = ['InvoiceNumber', 'CustomerName', 'InvoiceDate'];

export { allColumns, defaultColumns };

class ViewModel {
  showColumns: string[] = [];
  hideColumns: string[] = [];
}

@Component({
  selector: 'app-invoice-list-page-settings',
  templateUrl: './invoice-list-page-settings.component.html',
  styleUrls: ['./invoice-list-page-settings.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InvoicelistPageSettingsComponent implements OnInit {
  private readonly showColumns = new BehaviorSubject<string[]>([]);
  private readonly hideColumns = new BehaviorSubject<string[]>([]);

  protected showColumns$ = this.showColumns.asObservable();
  protected hideColumns$ = this.hideColumns.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([this.showColumns$, this.hideColumns$]).pipe(
    debounceTime(0),
    map(([showColumns, hideColumns]) => {
      const vm: ViewModel = {
        showColumns,
        hideColumns
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  constructor(
    private readonly router: Router,
    private readonly invoiceListPageSettingsService: InvoiceListPageSettingsService
  ) {}

  ngOnInit(): void {
    this.showColumns.next(this.invoiceListPageSettingsService.settings.columns ?? []);
    this.hideColumns.next(
      allColumns.filter((item) => !(this.showColumns.value ?? []).includes(item))
    );
  }

  public drop(event: CdkDragDrop<string[]>): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      let sourceContainer =
        event.container.id === 'showList' ? this.hideColumns.value : this.showColumns.value;
      let targetContainer =
        event.container.id === 'showList' ? this.showColumns.value : this.hideColumns.value;
      transferArrayItem(sourceContainer, targetContainer, event.previousIndex, event.currentIndex);
      this.showColumns.next(event.container.id === 'showList' ? targetContainer : sourceContainer);
      this.hideColumns.next(event.container.id === 'showList' ? sourceContainer : targetContainer);
    }
  }

  public save(): void {
    this.invoiceListPageSettingsService.update({ columns: this.showColumns.value ?? [] });
    this.router.navigateByUrl('/invoices');
  }
}
