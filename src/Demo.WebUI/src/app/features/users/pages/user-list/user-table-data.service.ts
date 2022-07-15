import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, map, Observable } from 'rxjs';
import { ApiUsersClient, SearchUserDto, SearchUserOrderByEnum } from '@api/api.generated.clients';
import { UserEventsService } from '@api/signalr.generated.services';
import { UserStoreService } from '@domain/user/user-store.service';
import {
  ITableFilterCriteria,
  TableDataBase,
  TableDataContext,
  TableDataSearchResult,
  TableFilterCriteria
} from '@shared/base/table-data-base';
import {
  UserListPageSettings,
  UserListPageSettingsService
} from './user-list-page-settings.service';

export declare type UserSortColumn = 'Fullname' | 'Email' | undefined;

export class UserTableFilterCriteria extends TableFilterCriteria implements ITableFilterCriteria {
  override sortColumn: UserSortColumn;

  constructor(pageSettings?: UserListPageSettings) {
    super(pageSettings?.pageSize);
    this.sortColumn = pageSettings?.sortColumn ?? 'Fullname';
    this.sortDirection = pageSettings?.sortDirection ?? 'desc';
  }
}
export interface UserTableDataContext extends TableDataContext<SearchUserDto> {
  criteria: UserTableFilterCriteria | undefined;
}

@Injectable()
export class UserTableDataService extends TableDataBase<SearchUserDto> {
  public searchTerm = new FormControl();

  public observe$: Observable<UserTableDataContext> = combineLatest([this.observeInternal$]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: UserTableDataContext = {
        ...baseContext,
        criteria: baseContext.criteria as UserTableFilterCriteria
      };
      return context;
    })
  );

  protected entityUpdatedInStore$ = this.userStoreService.userUpdatedInStore$;
  protected entityDeletedEvent$ = this.userEventsService.userDeleted$;

  constructor(
    private readonly apiUsersClient: ApiUsersClient,
    private readonly userStoreService: UserStoreService,
    private readonly userEventsService: UserEventsService,
    private readonly userListPageSettingsService: UserListPageSettingsService
  ) {
    super();
    const pageSettings = this.userListPageSettingsService.settings;
    this.init(new UserTableFilterCriteria(pageSettings));
    this.search();
  }

  protected searchFunction = (criteria: UserTableFilterCriteria) => {
    this.userListPageSettingsService.update({
      pageSize: criteria.pageSize,
      sortColumn: criteria.sortColumn,
      sortDirection: criteria.sortDirection
    });

    return this.apiUsersClient
      .search(
        this.sortColumn(criteria.sortColumn),
        this.sortbyDescending(criteria.sortDirection),
        criteria.pageIndex,
        criteria.pageSize,
        this.searchTerm.value
      )
      .pipe(
        map((response) => {
          const result: TableDataSearchResult<SearchUserDto> = {
            items: response.users,
            pageIndex: response.pageIndex,
            pageSize: response.pageSize,
            totalItems: response.totalItems,
            totalPages: response.totalPages,
            hasPreviousPage: response.hasPreviousPage,
            hasNextPage: response.hasNextPage
          };
          return result;
        })
      );
  };

  private sortColumn(sortColumn: UserSortColumn): SearchUserOrderByEnum | undefined {
    switch (sortColumn) {
      case undefined:
        return undefined;
      case 'Fullname':
        return SearchUserOrderByEnum.Fullname;
      case 'Email':
        return SearchUserOrderByEnum.Email;
      default: {
        const exhaustiveCheck: never = sortColumn; // force compiler error on missing options
        throw new Error(exhaustiveCheck);
      }
    }
  }

  protected getByIdFunction = (id: string) =>
    this.userStoreService.getById(id).pipe(
      map((user) => {
        return new SearchUserDto(user);
      })
    );
}
