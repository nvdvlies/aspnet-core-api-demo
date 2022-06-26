import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { LoggerService } from '@shared/services/logger.service';
import { CustomerSortColumn } from './customer-table-data.service';

export class CustomerListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: CustomerSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
}

@Injectable()
export class CustomerListPageSettingsService {
  private key = CustomerListPageSettings.name;

  constructor(private readonly loggerService: LoggerService) {}

  private _settings: CustomerListPageSettings | undefined;

  public get settings(): CustomerListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    this._settings = new CustomerListPageSettings();
    var json = localStorage.getItem(this.key);
    if (json) {
      Object.assign(this._settings, this.tryParse(json));
    }
    return this._settings;
  }

  public update(settings: Partial<CustomerListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): Partial<CustomerListPageSettings> {
    try {
      return JSON.parse(json) as Partial<CustomerListPageSettings>;
    } catch (error: any) {
      this.loggerService.logError('Failed to parse CustomerListPageSettings', undefined, error);
      localStorage.removeItem(this.key);
      return new CustomerListPageSettings();
    }
  }
}
