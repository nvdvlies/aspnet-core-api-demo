import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import {
  ApplicationSettingsDto,
  IApplicationSettingsDto,
  IApplicationSettingsSettingsDto
} from '@api/api.generated.clients';
import { ApplicationSettingsStoreService } from '@domain/application-settings/application-settings-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext
} from '@domain/shared/domain-entity-base';
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';

export interface IApplicationSettingsDomainEntityContext
  extends IDomainEntityContext<ApplicationSettingsDto> {}

export class ApplicationSettingsDomainEntityContext
  extends DomainEntityContext<ApplicationSettingsDto>
  implements IApplicationSettingsDomainEntityContext
{
  constructor() {
    super();
  }
}

type ApplicationSettingsControls = { [key in keyof IApplicationSettingsDto]-?: AbstractControl };
export type ApplicationSettingsFormGroup = UntypedFormGroup & { controls: ApplicationSettingsControls };

type ApplicationSettingsSettingsControls = {
  [key in keyof IApplicationSettingsSettingsDto]-?: AbstractControl;
};
export type ApplicationSettingsSettingsFormGroup = UntypedFormGroup & {
  controls: ApplicationSettingsSettingsControls;
};

@Injectable()
export class ApplicationSettingsDomainEntityService
  extends DomainEntityBase<ApplicationSettingsDto>
  implements OnDestroy
{
  protected createFunction = (applicationSettings: ApplicationSettingsDto) =>
    this.applicationSettingsStoreService.save(applicationSettings);
  protected readFunction = (id?: string) => this.applicationSettingsStoreService.get();
  protected updateFunction = (applicationSettings: ApplicationSettingsDto) =>
    this.applicationSettingsStoreService.save(applicationSettings);
  protected deleteFunction = (id?: string) => of(void 0);
  protected entityUpdatedEvent$ =
    this.applicationSettingsStoreService.applicationSettingsUpdatedInStore$;
  protected readPermission?: keyof typeof Permission = 'ApplicationSettingsRead';
  protected writePermission?: keyof typeof Permission = 'ApplicationSettingsWrite';

  public observe$: Observable<ApplicationSettingsDomainEntityContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: ApplicationSettingsDomainEntityContext = {
        ...baseContext
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    userPermissionService: UserPermissionService,
    private readonly applicationSettingsStoreService: ApplicationSettingsStoreService
  ) {
    super(route, userPermissionService);
    super.init();
  }

  public override get form(): ApplicationSettingsFormGroup {
    return super.form as ApplicationSettingsFormGroup;
  }
  public override set form(value: ApplicationSettingsFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildFormGroup();
  }

  private buildFormGroup(): ApplicationSettingsFormGroup {
    const controls: ApplicationSettingsControls = {
      id: new UntypedFormControl(super.readonlyFormState),
      settings: new UntypedFormGroup({
        setting1: new UntypedFormControl(null),
        setting2: new UntypedFormControl(null),
        setting3: new UntypedFormControl(null, [Validators.required], []),
        setting4: new UntypedFormControl(null, [Validators.required], []),
        setting5: new UntypedFormControl(null, [Validators.required], [])
      } as ApplicationSettingsSettingsControls) as ApplicationSettingsSettingsFormGroup,
      createdBy: new UntypedFormControl(super.readonlyFormState),
      createdOn: new UntypedFormControl(super.readonlyFormState),
      lastModifiedBy: new UntypedFormControl(super.readonlyFormState),
      lastModifiedOn: new UntypedFormControl(super.readonlyFormState),
      xmin: new UntypedFormControl(super.readonlyFormState)
    };
    return new UntypedFormGroup(controls) as ApplicationSettingsFormGroup;
  }

  protected instantiateNewEntity(): Observable<ApplicationSettingsDto> {
    return of(new ApplicationSettingsDto());
  }

  public override read(): Observable<null> {
    return super.read();
  }

  public save(): Observable<ApplicationSettingsDto> {
    return super.upsert();
  }

  public override reset(): void {
    super.reset();
  }
}
