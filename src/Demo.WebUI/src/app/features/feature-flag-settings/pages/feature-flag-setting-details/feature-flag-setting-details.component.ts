import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, filter, map, Observable, tap } from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  FeatureFlagFormGroup,
  FeatureFlagSettingsDomainEntityService,
  FeatureFlagSettingsFormGroup,
  IFeatureFlagSettingsDomainEntityContext
} from '@domain/feature-flag-settings/feature-flag-settings-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { FeatureFlagSettingListRouteState } from '@feature-flag-settings/pages/feature-flag-setting-list/feature-flag-setting-list.component';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';

interface ViewModel extends IFeatureFlagSettingsDomainEntityContext {
  featureFlagName: string | undefined;
}

@Component({
  templateUrl: './feature-flag-setting-details.component.html',
  styleUrls: ['./feature-flag-setting-details.component.scss'],
  providers: [
    FeatureFlagSettingsDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: FeatureFlagSettingsDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FeatureFlagSettingDetailsComponent implements OnInit, IHasForm {
  public read$ = this.featureFlagSettingsDomainEntityService.read();

  public readonly featureFlagName = new BehaviorSubject<keyof typeof FeatureFlag | undefined>(
    undefined
  );

  public featureFlagName$ = this.featureFlagName.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([
    this.featureFlagSettingsDomainEntityService.observe$,
    this.featureFlagName$
  ]).pipe(
    debounceTime(0),
    map(([domainEntityContext, featureFlagName]) => {
      const vm: ViewModel = {
        ...domainEntityContext,
        featureFlagName
      };

      this.featureFlagFormGroup =
        this.featureFlagSettingsDomainEntityService.featureFlags.find(
          (featureFlags) => featureFlags.controls.name.value === featureFlagName
        ) ?? this.featureFlagSettingsDomainEntityService.addFeatureFlag(featureFlagName!);

      console.log(this.featureFlagFormGroup.value);

      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  public form: FeatureFlagSettingsFormGroup = this.featureFlagSettingsDomainEntityService.form;
  public featureFlagFormGroup: FeatureFlagFormGroup | undefined;

  constructor(
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly featureFlagSettingsDomainEntityService: FeatureFlagSettingsDomainEntityService
  ) {}

  public ngOnInit(): void {
    const name = this.route.snapshot.paramMap.get('name');
    if (name == null) {
      throw new Error(`Couldn't find parameter 'name' in route parameters.`);
    }

    this.featureFlagName.next(name as FeatureFlag);
  }

  public save(): void {
    if (!this.form.valid) {
      return;
    }

    this.featureFlagSettingsDomainEntityService.save().subscribe(() => {
      this.router.navigateByUrl('/feature-flag-settings', {
        state: {
          spotlightIdentifier: this.featureFlagName.value
        } as FeatureFlagSettingListRouteState
      });
    });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.featureFlagSettingsDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.save();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.c', ['$event'])
  public closeShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/feature-flag-settings');
    event.preventDefault();
  }
}
