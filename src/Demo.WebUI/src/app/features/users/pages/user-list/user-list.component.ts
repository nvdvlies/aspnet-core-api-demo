import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  QueryList,
  ViewChild,
  ViewChildren
} from '@angular/core';
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
import { MatPaginator } from '@angular/material/paginator';

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
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
  @ViewChildren('tableRows', { read: ElementRef }) tableRows: QueryList<ElementRef> | undefined;

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

  @HostListener('document:keydown.shift.alt.enter', ['$event'])
  public navigateToDetails(event: Event): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.router.navigate(['/users', this.vm.selectedItem.id]);
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowUp', ['$event'])
  public navigateTableUp(event: Event): void {
    this.userTableDataService.selectedItemIndex -= 1;
    this.tableRows
      ?.get(this.userTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowDown', ['$event'])
  public navigateTableDown(event: Event): void {
    this.userTableDataService.selectedItemIndex += 1;
    this.tableRows
      ?.get(this.userTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.n', ['$event'])
  public newUserShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/users/new');
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowRight', ['$event'])
  public navigateToNextPage(event: KeyboardEvent) {
    if (this.paginator?.hasNextPage) {
      this.paginator?.nextPage();
    }
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowLeft', ['$event'])
  public navigateToPreviousPage(event: KeyboardEvent) {
    if (this.paginator?.hasPreviousPage) {
      this.paginator?.previousPage();
    }
    event.preventDefault();
  }

  public ngOnDestroy(): void {
    this.userTableDataService.clearSpotlight();
  }
}
