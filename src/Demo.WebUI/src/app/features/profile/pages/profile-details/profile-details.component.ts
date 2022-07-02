import { Component, OnInit, ChangeDetectionStrategy, HostListener } from '@angular/core';
import { ApiCurrentUserClient, ChangePasswordCommand } from '@api/api.generated.clients';
import {
  CurrentUserDomainEntityService,
  CurrentUserFormGroup,
  ICurrentUserDomainEntityContext
} from '@domain/current-user/current-user-domain-entity.service';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { ModalService } from '@shared/services/modal.service';
import { BehaviorSubject, combineLatest, map, Observable, debounceTime, tap, finalize } from 'rxjs';

interface ViewModel extends ICurrentUserDomainEntityContext {
  saved: boolean;
  isChangingPassword: boolean;
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
  public readonly isChangingPassword = new BehaviorSubject<boolean>(false);

  public saved$ = this.saved.asObservable();
  public isChangingPassword$ = this.isChangingPassword.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([
    this.currentUserDomainEntityService.observe$,
    this.saved$,
    this.isChangingPassword$
  ]).pipe(
    debounceTime(0),
    map(([domainEntityContext, saved, isChangingPassword]) => {
      const vm: ViewModel = {
        ...domainEntityContext,
        saved,
        isChangingPassword
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  public form: CurrentUserFormGroup = this.currentUserDomainEntityService.form;

  constructor(
    private readonly currentUserDomainEntityService: CurrentUserDomainEntityService,
    private readonly apiCurrentUserClient: ApiCurrentUserClient,
    private readonly modalService: ModalService
  ) {}

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

  public changePassword(): void {
    this.isChangingPassword.next(true);
    this.apiCurrentUserClient
      .changePassword(new ChangePasswordCommand())
      .pipe(finalize(() => this.isChangingPassword.next(false)))
      .subscribe(() =>
        this.modalService.showMessage(
          `You will receive an e-mail with a reset password link on address ${this.form.controls.email.value} within a few minutes.`,
          'Change password'
        )
      );
  }
}
