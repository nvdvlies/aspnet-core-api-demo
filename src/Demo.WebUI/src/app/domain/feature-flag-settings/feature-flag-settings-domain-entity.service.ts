import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import {
  FeatureFlagSettingsDto,
  IFeatureFlagDto,
  IFeatureFlagSettingsDto,
  IFeatureFlagSettingsSettingsDto
} from '@api/api.generated.clients';
import { FeatureFlagSettingsStoreService } from '@domain/feature-flag-settings/feature-flag-settings-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext
} from '@domain/shared/domain-entity-base';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';

export interface IFeatureFlagSettingsDomainEntityContext
  extends IDomainEntityContext<FeatureFlagSettingsDto> {}

export class FeatureFlagSettingsDomainEntityContext
  extends DomainEntityContext<FeatureFlagSettingsDto>
  implements IFeatureFlagSettingsDomainEntityContext
{
  constructor() {
    super();
  }
}

type FeatureFlagSettingsControls = { [key in keyof IFeatureFlagSettingsDto]-?: AbstractControl };
export type FeatureFlagSettingsFormGroup = FormGroup & { controls: FeatureFlagSettingsControls };

type FeatureFlagSettingsSettingsControls = {
  [key in keyof IFeatureFlagSettingsSettingsDto]-?: AbstractControl;
};
export type FeatureFlagSettingsSettingsFormGroup = FormGroup & {
  controls: FeatureFlagSettingsSettingsControls;
};

type FeatureFlagControls = { [key in keyof IFeatureFlagDto]-?: AbstractControl };
export type FeatureFlagFormGroup = FormGroup & {
  controls: FeatureFlagControls;
};

export type FeatureFlagFormArray = FormArray & {
  controls: FeatureFlagFormGroup[];
};

@Injectable()
export class FeatureFlagSettingsDomainEntityService
  extends DomainEntityBase<FeatureFlagSettingsDto>
  implements OnDestroy
{
  protected createFunction = (featureFlagSettings: FeatureFlagSettingsDto) =>
    this.featureFlagSettingsStoreService.save(featureFlagSettings);
  protected readFunction = (id?: string) => this.featureFlagSettingsStoreService.get();
  protected updateFunction = (featureFlagSettings: FeatureFlagSettingsDto) =>
    this.featureFlagSettingsStoreService.save(featureFlagSettings);
  protected deleteFunction = (id?: string) => of(void 0);
  protected entityUpdatedEvent$ =
    this.featureFlagSettingsStoreService.featureFlagSettingsUpdatedInStore$;

  public observe$: Observable<FeatureFlagSettingsDomainEntityContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: FeatureFlagSettingsDomainEntityContext = {
        ...baseContext
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    private readonly featureFlagSettingsStoreService: FeatureFlagSettingsStoreService
  ) {
    super(route);
    super.init();
  }

  public override get form(): FeatureFlagSettingsFormGroup {
    return super.form as FeatureFlagSettingsFormGroup;
  }
  public override set form(value: FeatureFlagSettingsFormGroup) {
    super.form = value;
  }

  public get featureFlags(): FeatureFlagFormGroup[] {
    return this.featureFlagFormArray.controls as FeatureFlagFormGroup[];
  }

  private get featureFlagFormArray(): FeatureFlagFormArray {
    return (this.form.controls.settings as FeatureFlagSettingsSettingsFormGroup).controls
      .featureFlags as FeatureFlagFormArray;
  }

  protected instantiateForm(): void {
    this.form = this.buildFormGroup();
  }

  private buildFormGroup(): FeatureFlagSettingsFormGroup {
    const controls: FeatureFlagSettingsControls = {
      id: new FormControl(super.readonlyFormState),
      settings: new FormGroup({
        featureFlags: new FormArray([] as FeatureFlagFormGroup[]) as FeatureFlagFormArray
      } as FeatureFlagSettingsSettingsControls) as FeatureFlagSettingsSettingsFormGroup,
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      timestamp: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as FeatureFlagSettingsFormGroup;
  }

  private buildFeatureFlagFormGroup(): FeatureFlagFormGroup {
    const controls: FeatureFlagControls = {
      id: new FormControl(super.readonlyFormState),
      name: new FormControl(super.readonlyFormState),
      description: new FormControl(null, [Validators.required], []),
      enabledForAll: new FormControl(null),
      enabledForUsers: new FormArray([]),
      timestamp: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as FeatureFlagFormGroup;
  }

  public addFeatureFlag(featureFlagName: keyof typeof FeatureFlag): FeatureFlagFormGroup {
    const formGroup = this.buildFeatureFlagFormGroup();
    formGroup.controls.name.setValue(featureFlagName);
    this.featureFlagFormArray.push(formGroup);
    return formGroup;
  }

  public removeFeatureFlag(index: number): void {
    if (this.featureFlagFormArray.length > 1) {
      this.featureFlagFormArray.removeAt(index);
    }
  }

  protected instantiateNewEntity(): Observable<FeatureFlagSettingsDto> {
    return of(new FeatureFlagSettingsDto());
  }

  public override read(): Observable<null> {
    return super.read();
  }

  public save(): Observable<FeatureFlagSettingsDto> {
    return super.upsert();
  }

  protected override afterPatchEntityToFormHook(featureFlagSettings: FeatureFlagSettingsDto): void {
    // form.patchValue doesnt modify FormArray structure, so we need to do this manually afterwards.
    this.patchFeatureFlagsToForm(featureFlagSettings);
  }

  private patchFeatureFlagsToForm(featureFlagSettings: FeatureFlagSettingsDto): void {
    this.featureFlagFormArray.clear();
    featureFlagSettings.settings?.featureFlags?.forEach((featureFlag) => {
      const featureFlagFormGroup = this.buildFeatureFlagFormGroup();
      featureFlagFormGroup.patchValue({ ...featureFlag });
      (featureFlagFormGroup.controls.enabledForUsers as FormArray).clear();
      featureFlag.enabledForUsers?.forEach((enabledForUser) => {
        (featureFlagFormGroup.controls.enabledForUsers as FormArray).push(
          new FormControl(enabledForUser)
        );
      });
      this.featureFlagFormArray.push(featureFlagFormGroup);
    });
  }

  public override reset(): void {
    super.reset();
  }
}
