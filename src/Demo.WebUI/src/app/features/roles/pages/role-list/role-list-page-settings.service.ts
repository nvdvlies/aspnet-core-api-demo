import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { LoggerService } from '@shared/services/logger.service';
import { RoleSortColumn } from './role-table-data.service';

export class RoleListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: RoleSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
}

@Injectable()
export class RoleListPageSettingsService {
  private key = RoleListPageSettings.name;

  constructor(private readonly loggerService: LoggerService) {}

  private _settings: RoleListPageSettings | undefined;

  public get settings(): RoleListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    this._settings = new RoleListPageSettings();
    var json = localStorage.getItem(this.key);
    if (json) {
      Object.assign(this._settings, this.tryParse(json));
    }
    return this._settings;
  }

  public update(settings: Partial<RoleListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): Partial<RoleListPageSettings> {
    try {
      return JSON.parse(json) as Partial<RoleListPageSettings>;
    } catch (error: any) {
      this.loggerService.logError('Failed to parse RoleListPageSettings', undefined, error);
      localStorage.removeItem(this.key);
      return new RoleListPageSettings();
    }
  }
}
