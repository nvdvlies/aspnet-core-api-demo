import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, combineLatest, debounceTime, map, Observable, tap } from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  ApplicationSettingsDomainEntityService,
  ApplicationSettingsFormGroup,
  IApplicationSettingsDomainEntityContext
} from '@domain/application-settings/application-settings-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';

interface ViewModel extends IApplicationSettingsDomainEntityContext {
  settingsSaved: boolean;
}

@Component({
  templateUrl: './application-settings-details.component.html',
  styleUrls: ['./application-settings-details.component.scss'],
  providers: [
    ApplicationSettingsDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: ApplicationSettingsDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ApplicationSettingsDetailsComponent implements OnInit, IHasForm {
  public read$ = this.applicationSettingsDomainEntityService.read();

  public readonly settingsSaved = new BehaviorSubject<boolean>(false);

  public settingsSaved$ = this.settingsSaved.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([
    this.applicationSettingsDomainEntityService.observe$,
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

  public form: ApplicationSettingsFormGroup = this.applicationSettingsDomainEntityService.form;

  constructor(
    private readonly router: Router,
    private readonly applicationSettingsDomainEntityService: ApplicationSettingsDomainEntityService
  ) {}

  public ngOnInit(): void {}

  public save(): void {
    this.settingsSaved.next(false);

    if (!this.form.valid) {
      return;
    }

    this.applicationSettingsDomainEntityService.save().subscribe(() => {
      this.settingsSaved.next(true);
    });
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.save();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.c', ['$event'])
  public closeShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/');
    event.preventDefault();
  }
}
