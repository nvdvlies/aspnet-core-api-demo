import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { LoggerService } from '@shared/services/logger.service';
import { UserSortColumn } from './user-table-data.service';

export class UserListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: UserSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
}

@Injectable()
export class UserListPageSettingsService {
  private key = UserListPageSettings.name;

  constructor(private readonly loggerService: LoggerService) {}

  private _settings: UserListPageSettings | undefined;

  public get settings(): UserListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    this._settings = new UserListPageSettings();
    var json = localStorage.getItem(this.key);
    if (json) {
      Object.assign(this._settings, this.tryParse(json));
    }
    return this._settings;
  }

  public update(settings: Partial<UserListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): Partial<UserListPageSettings> {
    try {
      return JSON.parse(json) as Partial<UserListPageSettings>;
    } catch (error: any) {
      this.loggerService.logError('Failed to parse UserListPageSettings', undefined, error);
      localStorage.removeItem(this.key);
      return new UserListPageSettings();
    }
  }
}
