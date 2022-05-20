import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { InvoiceSortColumn } from './invoice-table-data.service';

export class InvoiceListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: InvoiceSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
}

@Injectable()
export class InvoiceListPageSettingsService {
  private key = InvoiceListPageSettings.name;

  private _settings: InvoiceListPageSettings | undefined;

  public get settings(): InvoiceListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    var json = localStorage.getItem(this.key);
    this._settings = json ? this.tryParse(json) : new InvoiceListPageSettings();
    return this._settings;
  }

  public update(settings: Partial<InvoiceListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): InvoiceListPageSettings {
    try {
      return JSON.parse(json) as InvoiceListPageSettings;
    } catch (error) {
      console.error(error);
      return new InvoiceListPageSettings();
    }
  }
}
