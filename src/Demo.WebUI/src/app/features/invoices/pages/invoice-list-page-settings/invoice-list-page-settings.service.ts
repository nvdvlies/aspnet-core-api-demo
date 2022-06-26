import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { LoggerService } from '@shared/services/logger.service';
import { InvoiceSortColumn } from '../invoice-list/invoice-table-data.service';
import { defaultColumns } from './invoice-list-page-settings.component';

export class InvoiceListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: InvoiceSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
  public columns: string[] = defaultColumns;
}

@Injectable()
export class InvoiceListPageSettingsService {
  private key = InvoiceListPageSettings.name;

  constructor(private readonly loggerService: LoggerService) {}

  private _settings: InvoiceListPageSettings | undefined;

  public get settings(): InvoiceListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    this._settings = new InvoiceListPageSettings();
    var json = localStorage.getItem(this.key);
    if (json) {
      Object.assign(this._settings, this.tryParse(json));
    }
    return this._settings;
  }

  public update(settings: Partial<InvoiceListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): Partial<InvoiceListPageSettings> {
    try {
      return JSON.parse(json) as Partial<InvoiceListPageSettings>;
    } catch (error: any) {
      this.loggerService.logError('Failed to parse InvoiceListPageSettings', undefined, error);
      localStorage.removeItem(this.key);
      return new InvoiceListPageSettings();
    }
  }
}
