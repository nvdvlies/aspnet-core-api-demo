import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, map, Observable, tap } from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  UserPreferencesDomainEntityService,
  UserPreferencesFormGroup,
  IUserPreferencesDomainEntityContext
} from '@domain/user-preferences/user-preferences-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';

interface ViewModel extends IUserPreferencesDomainEntityContext {
  settingsSaved: boolean;
}

@Component({
  templateUrl: './user-preferences-details.component.html',
  styleUrls: ['./user-preferences-details.component.scss'],
  providers: [
    UserPreferencesDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: UserPreferencesDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPreferencesDetailsComponent implements IHasForm {
  public read$ = this.userPreferencesDomainEntityService.read();

  public readonly settingsSaved = new BehaviorSubject<boolean>(false);

  public settingsSaved$ = this.settingsSaved.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([
    this.userPreferencesDomainEntityService.observe$,
    this.settingsSaved$
  ]).pipe(
    debounceTime(0),
    map(([domainEntityContext, settingsSaved]) => {
      const vm: ViewModel = {
        ...domainEntityContext,
        settingsSaved
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  public form: UserPreferencesFormGroup = this.userPreferencesDomainEntityService.form;

  constructor(
    private readonly router: Router,
    private readonly userPreferencesDomainEntityService: UserPreferencesDomainEntityService
  ) {}

  public save(): void {
    this.settingsSaved.next(false);

    if (!this.form.valid) {
      return;
    }

    this.userPreferencesDomainEntityService.save().subscribe(() => {
      this.settingsSaved.next(true);
    });
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.form.markAllAsTouched();
    this.save();
    event.preventDefault();
  }
}
