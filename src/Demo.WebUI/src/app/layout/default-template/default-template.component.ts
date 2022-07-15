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
import { UserPermissionService } from '@shared/services/user-permission.service';
import { Subject, takeUntil } from 'rxjs';

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

  constructor(
    public readonly authService: AuthService,
    private readonly changeDetectorRef: ChangeDetectorRef,
    private readonly userPermissionService: UserPermissionService
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

  ngOnDestroy(): void {
    this.onDestroy.next();
    this.onDestroy.complete();
  }
}
