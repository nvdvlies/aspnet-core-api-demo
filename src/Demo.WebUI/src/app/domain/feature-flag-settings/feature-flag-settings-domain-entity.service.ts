import { Injectable, OnDestroy } from '@angular/core';
import {
  AbstractControl,
  UntypedFormArray,
  UntypedFormControl,
  UntypedFormGroup,
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
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';

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
export type FeatureFlagSettingsFormGroup = UntypedFormGroup & { controls: FeatureFlagSettingsControls };

type FeatureFlagSettingsSettingsControls = {
  [key in keyof IFeatureFlagSettingsSettingsDto]-?: AbstractControl;
};
export type FeatureFlagSettingsSettingsFormGroup = UntypedFormGroup & {
  controls: FeatureFlagSettingsSettingsControls;
};

type FeatureFlagControls = { [key in keyof IFeatureFlagDto]-?: AbstractControl };
export type FeatureFlagFormGroup = UntypedFormGroup & {
  controls: FeatureFlagControls;
};

export type FeatureFlagFormArray = UntypedFormArray & {
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
  protected readPermission?: keyof typeof Permission = 'FeatureFlagSettingsRead';
  protected writePermission?: keyof typeof Permission = 'FeatureFlagSettingsWrite';

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
    userPermissionService: UserPermissionService,
    private readonly featureFlagSettingsStoreService: FeatureFlagSettingsStoreService
  ) {
    super(route, userPermissionService);
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
      id: new UntypedFormControl(super.readonlyFormState),
      settings: new UntypedFormGroup({
        featureFlags: new UntypedFormArray([] as FeatureFlagFormGroup[]) as FeatureFlagFormArray
      } as FeatureFlagSettingsSettingsControls) as FeatureFlagSettingsSettingsFormGroup,
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as FeatureFlagSettingsFormGroup;
  }

  private buildFeatureFlagFormGroup(): FeatureFlagFormGroup {
    const controls: FeatureFlagControls = {
      id: new UntypedFormControl(super.readonlyFormState),
      name: new UntypedFormControl(super.readonlyFormState),
      description: new UntypedFormControl(null, [Validators.required], []),
      enabledForAll: new UntypedFormControl(false),
      enabledForUsers: new UntypedFormArray([], [this.uniqueUsersValidator()], []),
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as FeatureFlagFormGroup;
  }

  private uniqueUsersValidator(): ValidatorFn {
    return (formArray: AbstractControl): ValidationErrors | null => {
      let userIds: string[] = [];
      if (formArray instanceof UntypedFormArray) {
        for (let formControl of formArray.controls) {
          if (formControl instanceof UntypedFormControl) {
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

  public addUser(formArray: UntypedFormArray): void {
    formArray.push(new UntypedFormControl(null, [Validators.required], []));
  }

  public removeUser(formArray: UntypedFormArray, index: number): void {
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
      (featureFlagFormGroup.controls.enabledForUsers as UntypedFormArray).clear();
      featureFlag.enabledForUsers?.forEach((enabledForUser) => {
        (featureFlagFormGroup.controls.enabledForUsers as UntypedFormArray).push(
          new UntypedFormControl(enabledForUser)
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
