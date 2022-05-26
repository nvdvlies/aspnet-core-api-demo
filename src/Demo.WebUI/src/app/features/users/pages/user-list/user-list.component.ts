import { ChangeDetectionStrategy, Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { BehaviorSubject, combineLatest, map, Observable, tap } from 'rxjs';
import { UserTableDataSource } from '@users/pages/user-list/user-table-datasource';
import {
  UserTableDataContext,
  UserTableDataService
} from '@users/pages/user-list/user-table-data.service';
import { SearchUserDto } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/base/table-data-base';
import { Router } from '@angular/router';

interface ViewModel extends UserTableDataContext {
  searchInputFocused: boolean;
}

export interface UserListRouteState {
  spotlightIdentifier: string | undefined;
}

@Component({
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserListComponent implements OnInit, OnDestroy {
  public displayedColumns = ['Fullname', 'Email'];
  public dataSource!: UserTableDataSource;
  public searchTerm = this.userTableDataService.searchTerm;

  public readonly searchInputFocused = new BehaviorSubject<boolean>(false);

  public searchInputFocused$ = this.searchInputFocused.asObservable();

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([
    this.userTableDataService.observe$,
    this.searchInputFocused$
  ]).pipe(
    map(([baseContext, searchInputFocused]) => {
      const context: ViewModel = {
        ...baseContext,
        searchInputFocused
      };
      return context;
    }),
    tap((vm) => (this.vm = vm))
  );

  constructor(
    private readonly location: Location,
    private readonly router: Router,
    private readonly userTableDataService: UserTableDataService
  ) {}

  public ngOnInit(): void {
    this.dataSource = new UserTableDataSource(this.userTableDataService);
    this.spotlight();
  }

  private spotlight(): void {
    const state = this.location.getState() as UserListRouteState;
    if (state && state.spotlightIdentifier) {
      this.userTableDataService.spotlight(state.spotlightIdentifier);
    }
  }

  public search(criteria: TableFilterCriteria): void {
    this.userTableDataService.search(criteria);
  }

  public trackById(index: number, item: SearchUserDto): string {
    return item.id;
  }

  public onSearchInputEnter(): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.router.navigate(['/users', this.vm.selectedItem.id]);
  }

  public onSearchInputArrowUp(): void {
    this.userTableDataService.selectedItemIndex -= 1;
  }

  public onSearchInputArrowDown(): void {
    this.userTableDataService.selectedItemIndex += 1;
  }

  @HostListener('document:keydown.shift.alt.n', ['$event'])
  public newUserShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/users/new');
    event.preventDefault();
  }

  public ngOnDestroy(): void {
    this.userTableDataService.clearSpotlight();
  }
}
