import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, map, Observable, tap } from 'rxjs';
import { InvoiceListPageSettingsService } from './invoice-list-page-settings.service';

const ALL_COLUMNS = [
  'InvoiceNumber',
  'CustomerName',
  'InvoiceDate',
  'PaymentTerm',
  'OrderReference',
  'Status'
];

const DEFAULT_COLUMNS = ['InvoiceNumber', 'CustomerName', 'InvoiceDate'];

export { ALL_COLUMNS, DEFAULT_COLUMNS };

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
  public showListId = 'showList';
  public hideListId = 'hideList';

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
      ALL_COLUMNS.filter((item) => !(this.showColumns.value ?? []).includes(item))
    );
  }

  public drop(event: CdkDragDrop<string[]>): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      let sourceContainer =
        event.container.id === this.showListId ? this.hideColumns.value : this.showColumns.value;
      let targetContainer =
        event.container.id === this.showListId ? this.showColumns.value : this.hideColumns.value;
      transferArrayItem(sourceContainer, targetContainer, event.previousIndex, event.currentIndex);
      this.showColumns.next(
        event.container.id === this.showListId ? targetContainer : sourceContainer
      );
      this.hideColumns.next(
        event.container.id === this.showListId ? sourceContainer : targetContainer
      );
    }
  }

  public save(): void {
    this.invoiceListPageSettingsService.update({ columns: this.showColumns.value ?? [] });
    this.router.navigateByUrl('/invoices');
  }
}
