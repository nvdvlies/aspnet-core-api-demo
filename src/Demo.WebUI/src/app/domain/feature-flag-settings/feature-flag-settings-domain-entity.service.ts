import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
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
import { v4 as uuidv4 } from 'uuid';

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
      enabledForAll: new FormControl(false),
      enabledForUsers: new FormArray([], [this.uniqueUsersValidator()], []),
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      timestamp: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as FeatureFlagFormGroup;
  }

  private uniqueUsersValidator(): ValidatorFn {
    return (formArray: AbstractControl): ValidationErrors | null => {
      let userIds: string[] = [];
      if (formArray instanceof FormArray) {
        for (let formControl of formArray.controls) {
          if (formControl instanceof FormControl) {
            let userId = formControl.value;
            if (!userId) {
              continue;
            }

            if (userIds.includes(userId)) {
              formControl.setErrors({ isDuplicateUser: true });
            } else if (formControl.hasError('isDuplicateUser')) {
              delete formControl.errors?.['isDuplicateUser'];
              formControl.updateValueAndValidity();
            }

            userIds.push(userId);
          }
        }
      }
      return null;
    };
  }

  public addFeatureFlag(featureFlagName: keyof typeof FeatureFlag): FeatureFlagFormGroup {
    const formGroup = this.buildFeatureFlagFormGroup();
    formGroup.controls.id.setValue(uuidv4());
    formGroup.controls.name.setValue(featureFlagName);
    this.featureFlagFormArray.push(formGroup);
    return formGroup;
  }

  public addUser(formArray: FormArray): void {
    formArray.push(new FormControl(null, [Validators.required], []));
  }

  public removeUser(formArray: FormArray, index: number): void {
    if (formArray.length > 0) {
      formArray.removeAt(index);
      if (!this.form.dirty) {
        this.form.markAsDirty();
      }
    }
  }

  public removeFeatureFlag(index: number): void {
    if (this.featureFlagFormArray.length > 0) {
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

  public override getErrorMessage(errorKey: string, errorValue: any): string | undefined {
    switch (errorKey) {
      case 'isDuplicateUser':
        return 'This user already exists.';
      default:
        return super.getErrorMessage(errorKey, errorValue);
    }
  }

  public override reset(): void {
    super.reset();
  }
}
