import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
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
export type ApplicationSettingsFormGroup = FormGroup & { controls: ApplicationSettingsControls };

type ApplicationSettingsSettingsControls = {
  [key in keyof IApplicationSettingsSettingsDto]-?: AbstractControl;
};
export type ApplicationSettingsSettingsFormGroup = FormGroup & {
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
    private readonly applicationSettingsStoreService: ApplicationSettingsStoreService
  ) {
    super(route);
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
      id: new FormControl(super.readonlyFormState),
      settings: new FormGroup({
        setting1: new FormControl(null),
        setting2: new FormControl(null),
        setting3: new FormControl(null),
        setting4: new FormControl(null),
        setting5: new FormControl(null)
      } as ApplicationSettingsSettingsControls) as ApplicationSettingsSettingsFormGroup,
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      timestamp: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as ApplicationSettingsFormGroup;
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
