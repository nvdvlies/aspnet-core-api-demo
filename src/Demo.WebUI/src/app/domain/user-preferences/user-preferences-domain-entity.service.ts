import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import {
  UserPreferencesDto,
  IUserPreferencesDto,
  IUserPreferencesPreferencesDto
} from '@api/api.generated.clients';
import { UserPreferencesStoreService } from '@domain/user-preferences/user-preferences-store.service';
import {
  DomainEntityBase,
  DomainEntityContext,
  IDomainEntityContext
} from '@domain/shared/domain-entity-base';

export interface IUserPreferencesDomainEntityContext
  extends IDomainEntityContext<UserPreferencesDto> {}

export class UserPreferencesDomainEntityContext
  extends DomainEntityContext<UserPreferencesDto>
  implements IUserPreferencesDomainEntityContext
{
  constructor() {
    super();
  }
}

type UserPreferencesControls = { [key in keyof IUserPreferencesDto]-?: AbstractControl };
export type UserPreferencesFormGroup = FormGroup & { controls: UserPreferencesControls };

type UserPreferencesPreferencesControls = {
  [key in keyof IUserPreferencesPreferencesDto]-?: AbstractControl;
};
export type UserPreferencesPreferencesFormGroup = FormGroup & {
  controls: UserPreferencesPreferencesControls;
};

@Injectable()
export class UserPreferencesDomainEntityService
  extends DomainEntityBase<UserPreferencesDto>
  implements OnDestroy
{
  protected createFunction = (userPreferences: UserPreferencesDto) =>
    this.userPreferencesStoreService.save(userPreferences);
  protected readFunction = (id?: string) => this.userPreferencesStoreService.get();
  protected updateFunction = (userPreferences: UserPreferencesDto) =>
    this.userPreferencesStoreService.save(userPreferences);
  protected deleteFunction = (id?: string) => of(void 0);
  protected entityUpdatedEvent$ = this.userPreferencesStoreService.userPreferencesUpdatedInStore$;

  public observe$: Observable<UserPreferencesDomainEntityContext> = combineLatest([
    this.observeInternal$
  ]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: UserPreferencesDomainEntityContext = {
        ...baseContext
      };
      return context;
    })
  );

  constructor(
    route: ActivatedRoute,
    private readonly userPreferencesStoreService: UserPreferencesStoreService
  ) {
    super(route);
    super.init();
  }

  public override get form(): UserPreferencesFormGroup {
    return super.form as UserPreferencesFormGroup;
  }
  public override set form(value: UserPreferencesFormGroup) {
    super.form = value;
  }

  protected instantiateForm(): void {
    this.form = this.buildFormGroup();
  }

  private buildFormGroup(): UserPreferencesFormGroup {
    const controls: UserPreferencesControls = {
      id: new FormControl(super.readonlyFormState),
      preferences: new FormGroup({
        setting1: new FormControl(null),
        setting2: new FormControl(null),
        setting3: new FormControl(null),
        setting4: new FormControl(null),
        setting5: new FormControl(null)
      } as UserPreferencesPreferencesControls) as UserPreferencesPreferencesFormGroup,
      createdBy: new FormControl(super.readonlyFormState),
      createdOn: new FormControl(super.readonlyFormState),
      lastModifiedBy: new FormControl(super.readonlyFormState),
      lastModifiedOn: new FormControl(super.readonlyFormState),
      timestamp: new FormControl(super.readonlyFormState)
    };
    return new FormGroup(controls) as UserPreferencesFormGroup;
  }

  protected instantiateNewEntity(): Observable<UserPreferencesDto> {
    return of(new UserPreferencesDto());
  }

  public override read(): Observable<null> {
    return super.read();
  }

  public save(): Observable<UserPreferencesDto> {
    return super.upsert();
  }

  public override reset(): void {
    super.reset();
  }
}
