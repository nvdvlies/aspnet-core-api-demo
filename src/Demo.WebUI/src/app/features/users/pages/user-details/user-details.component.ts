import { ChangeDetectionStrategy, Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { combineLatest, debounceTime, EMPTY, map, Observable, switchMap, tap } from 'rxjs';
import { DomainEntityService } from '@domain/shared/domain-entity-base';
import {
  UserDomainEntityService,
  UserFormGroup,
  IUserDomainEntityContext,
  UserRoleFormArray
} from '@domain/user/user-domain-entity.service';
import { IHasForm } from '@shared/guards/unsaved-changes.guard';
import { UserListRouteState } from '@users/pages/user-list/user-list.component';
import { ModalService } from '@shared/services/modal.service';
import { ConfirmDeleteModalComponent } from '@shared/modals/confirm-delete-modal/confirm-delete-modal.component';
import { FormControl } from '@angular/forms';

interface ViewModel extends IUserDomainEntityContext {}

@Component({
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss'],
  providers: [
    UserDomainEntityService,
    {
      provide: DomainEntityService,
      useExisting: UserDomainEntityService
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserDetailsComponent implements OnInit, IHasForm {
  public initFromRoute$ = this.userDomainEntityService.initFromRoute();

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([this.userDomainEntityService.observe$]).pipe(
    debounceTime(0),
    map(([domainEntityContext]) => {
      const context: ViewModel = {
        ...domainEntityContext
      };
      return context;
    }),
    tap((vm) => (this.vm = vm))
  );

  public form: UserFormGroup = this.userDomainEntityService.form;
  public get userRoles(): UserRoleFormArray {
    return this.form.controls.userRoles as UserRoleFormArray;
  }

  constructor(
    private readonly router: Router,
    private readonly userDomainEntityService: UserDomainEntityService,
    private readonly modalService: ModalService
  ) {}

  public ngOnInit(): void {}

  public save(): void {
    if (!this.form.valid) {
      return;
    }

    this.userDomainEntityService.upsert().subscribe((user) => {
      this.router.navigateByUrl('/users', {
        state: { spotlightIdentifier: user.id } as UserListRouteState
      });
    });
  }

  public getRoleFormControl(index: number): FormControl {
    return this.userRoles!.at(index) as FormControl;
  }

  public addUserRole(): void {
    this.userDomainEntityService.addUserRole();
  }

  public removeUserRole(index: number): void {
    this.userDomainEntityService.removeUserRole(index);
  }

  public delete(): void {
    this.modalService
      .confirm(ConfirmDeleteModalComponent)
      .pipe(switchMap((confirmed) => (confirmed ? this.userDomainEntityService.delete() : EMPTY)))
      .subscribe(() => {
        this.router.navigateByUrl('/users');
      });
  }

  public resolveMergeConflictWithTakeTheirs(): void {
    this.userDomainEntityService.resolveMergeConflictWithTakeTheirs();
  }

  @HostListener('document:keydown.shift.alt.s', ['$event'])
  public saveShortcut(event: KeyboardEvent) {
    this.save();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.d', ['$event'])
  public deleteShortcut(event: KeyboardEvent) {
    if (!this.vm?.id) {
      return;
    }
    this.delete();
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.c', ['$event'])
  public closeShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/users');
    event.preventDefault();
  }
}
