import { Component, OnInit, ChangeDetectionStrategy, HostListener } from '@angular/core';
import {
  CurrentUserDomainEntityService,
  CurrentUserFormGroup,
  ICurrentUserDomainEntityContext
} from '@domain/current-user/current-user-domain-entity.service';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { BehaviorSubject, combineLatest, map, Observable, debounceTime, tap } from 'rxjs';

interface ViewModel extends ICurrentUserDomainEntityContext {
  saved: boolean;
}

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.scss'],
  providers: [
    CurrentUserDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: CurrentUserDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProfileDetailsComponent implements OnInit, IHasForm {
  public read$ = this.currentUserDomainEntityService.read();

  public readonly saved = new BehaviorSubject<boolean>(false);

  public saved$ = this.saved.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([this.currentUserDomainEntityService.observe$, this.saved$]).pipe(
    debounceTime(0),
    map(([domainEntityContext, saved]) => {
      const vm: ViewModel = {
        ...domainEntityContext,
        saved
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  public form: CurrentUserFormGroup = this.currentUserDomainEntityService.form;

  constructor(private readonly currentUserDomainEntityService: CurrentUserDomainEntityService) {}

  ngOnInit(): void {}

  public save(): void {
    this.saved.next(false);

    if (!this.form.valid) {
      return;
    }

    this.currentUserDomainEntityService.save().subscribe(() => {
      this.saved.next(true);
    });
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.form.markAllAsTouched();
    this.save();
    event.preventDefault();
  }
}
