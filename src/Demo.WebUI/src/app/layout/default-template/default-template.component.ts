import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  Input,
  OnDestroy,
  OnInit
} from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { environment } from '@env/environment';
import { ModalService } from '@shared/services/modal.service';
import { SignalrHealthService } from '@shared/services/signalr-health.service';
import { UserPermissionService } from '@shared/services/user-permission.service';
import {
  combineLatest,
  map,
  Observable,
  tap,
  debounceTime,
  Subject,
  takeUntil,
  BehaviorSubject
} from 'rxjs';

interface ViewModel {
  signalrHealthy: boolean;
}

@Component({
  templateUrl: './default-template.component.html',
  styleUrls: ['./default-template.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DefaultTemplateComponent implements OnInit, OnDestroy {
  @Input()
  public flag: boolean = true; // flag to force re-evaluation of menu permissions triggered by changes in permission service

  protected readonly onDestroy = new Subject<void>();

  protected onDestroy$ = this.onDestroy.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$ = combineLatest([this.signalrHealthService.isHealthy$]).pipe(
    debounceTime(0),
    map(([signalrHealthy]) => {
      const vm: ViewModel = {
        signalrHealthy
      };
      return vm;
    }),
    tap((vm) => (this.vm = vm))
  ) as Observable<ViewModel>;

  constructor(
    private readonly authService: AuthService,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly userPermissionService: UserPermissionService,
    private readonly signalrHealthService: SignalrHealthService,
    private readonly modalService: ModalService
  ) {}

  ngOnInit(): void {
    this.userPermissionService.updated$.pipe(takeUntil(this.onDestroy$)).subscribe(() => {
      try {
        this.flag = false;
        this.changeDetectorRef.detectChanges();
      } finally {
        this.flag = true;
      }
    });
  }

  public logout(): void {
    this.authService.logout({ returnTo: environment.auth.redirectUri });
  }

  public showSignalrUnhealthyModal(): void {
    this.modalService.showMessage(
      `Server connection lost. Please check your network connection. If the problem persists contact your administrator for help.`,
      'Connection lost'
    );
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
    this.onDestroy.complete();
  }
}
