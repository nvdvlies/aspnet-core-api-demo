import { Injectable } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { LoggerService } from '@shared/services/logger.service';
import { FeatureFlagSettingSortColumn } from './feature-flag-setting-table-data.service';

export class FeatureFlagSettingListPageSettings {
  public pageSize: number | undefined;
  public sortColumn: FeatureFlagSettingSortColumn | undefined;
  public sortDirection: SortDirection | undefined;
}

@Injectable()
export class FeatureFlagSettingListPageSettingsService {
  private key = FeatureFlagSettingListPageSettings.name;

  constructor(private readonly loggerService: LoggerService) {}

  private _settings: FeatureFlagSettingListPageSettings | undefined;

  public get settings(): FeatureFlagSettingListPageSettings {
    if (this._settings) {
      return this._settings;
    }
    this._settings = new FeatureFlagSettingListPageSettings();
    var json = localStorage.getItem(this.key);
    if (json) {
      Object.assign(this._settings, this.tryParse(json));
    }
    return this._settings;
  }

  public update(settings: Partial<FeatureFlagSettingListPageSettings>): void {
    Object.assign(this.settings, settings);
    localStorage.setItem(this.key, JSON.stringify(this.settings));
  }

  private tryParse(json: string): Partial<FeatureFlagSettingListPageSettings> {
    try {
      return JSON.parse(json) as Partial<FeatureFlagSettingListPageSettings>;
    } catch (error: any) {
      this.loggerService.logError(
        'Failed to parse FeatureFlagSettingListPageSettings',
        undefined,
        error
      );
      localStorage.removeItem(this.key);
      return new FeatureFlagSettingListPageSettings();
    }
  }
}
