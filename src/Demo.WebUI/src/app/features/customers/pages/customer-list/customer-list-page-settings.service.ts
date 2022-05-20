import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { CustomerSortColumn } from './customer-table-data.service';

export class CustomerListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: CustomerSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
}

@Injectable()
export class CustomerListPageSettingsService {
  private key = CustomerListPageSettings.name;

  private _settings: CustomerListPageSettings | undefined;

  public get settings(): CustomerListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    var json = localStorage.getItem(this.key);
    this._settings = json ? this.tryParse(json) : new CustomerListPageSettings();
    return this._settings;
  }

  public update(settings: Partial<CustomerListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): CustomerListPageSettings {
    try {
      return JSON.parse(json) as CustomerListPageSettings;
    } catch (error) {
      console.error(error);
      return new CustomerListPageSettings();
    }
  }
}
