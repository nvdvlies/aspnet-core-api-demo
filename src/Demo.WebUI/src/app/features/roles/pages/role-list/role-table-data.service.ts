import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { combineLatest, debounceTime, map, Observable } from 'rxjs';
import { ApiRolesClient, SearchRoleDto, SearchRoleOrderByEnum } from '@api/api.generated.clients';
import { RoleEventsService } from '@api/signalr.generated.services';
import { RoleStoreService } from '@domain/role/role-store.service';
import {
  ITableFilterCriteria,
  TableDataBase,
  TableDataContext,
  TableDataSearchResult,
  TableFilterCriteria
} from '@shared/base/table-data-base';
import {
  RoleListPageSettings,
  RoleListPageSettingsService
} from './role-list-page-settings.service';

export declare type RoleSortColumn = 'Name' | undefined;

export class RoleTableFilterCriteria extends TableFilterCriteria implements ITableFilterCriteria {
  override sortColumn: RoleSortColumn;

  constructor(pageSettings?: RoleListPageSettings) {
    super(pageSettings?.pageSize);
    this.sortColumn = pageSettings?.sortColumn ?? 'Name';
    this.sortDirection = pageSettings?.sortDirection ?? 'asc';
  }
}
export interface RoleTableDataContext extends TableDataContext<SearchRoleDto> {
  criteria: RoleTableFilterCriteria | undefined;
}

@Injectable()
export class RoleTableDataService extends TableDataBase<SearchRoleDto> {
  public searchTerm = new FormControl();

  public observe$: Observable<RoleTableDataContext> = combineLatest([this.observeInternal$]).pipe(
    debounceTime(0),
    map(([baseContext]) => {
      const context: RoleTableDataContext = {
        ...baseContext,
        criteria: baseContext.criteria as RoleTableFilterCriteria
      };
      return context;
    })
  );

  protected entityUpdatedInStore$ = this.roleStoreService.roleUpdatedInStore$;
  protected entityDeletedEvent$ = this.roleEventsService.roleDeleted$;

  constructor(
    private readonly apiRolesClient: ApiRolesClient,
    private readonly roleStoreService: RoleStoreService,
    private readonly roleEventsService: RoleEventsService,
    private readonly roleListPageSettingsService: RoleListPageSettingsService
  ) {
    super();
    const pageSettings = this.roleListPageSettingsService.settings;
    this.init(new RoleTableFilterCriteria(pageSettings));
    this.search();
  }

  protected searchFunction = (criteria: RoleTableFilterCriteria) => {
    this.roleListPageSettingsService.update({
      pageSize: criteria.pageSize,
      sortColumn: criteria.sortColumn,
      sortDirection: criteria.sortDirection
    });

    return this.apiRolesClient
      .search(
        this.sortColumn(criteria.sortColumn),
        this.sortbyDescending(criteria.sortDirection),
        criteria.pageIndex,
        criteria.pageSize,
        this.searchTerm.value
      )
      .pipe(
        map((response) => {
          const result: TableDataSearchResult<SearchRoleDto> = {
            items: response.roles,
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

  private sortColumn(sortColumn: RoleSortColumn): SearchRoleOrderByEnum | undefined {
    switch (sortColumn) {
      case undefined:
        return undefined;
      case 'Name':
        return SearchRoleOrderByEnum.Name;
      default: {
        const exhaustiveCheck: never = sortColumn; // force compiler error on missing options
        throw new Error(exhaustiveCheck);
      }
    }
  }

  protected getByIdFunction = (id: string) =>
    this.roleStoreService.getById(id).pipe(
      map((role) => {
        return new SearchRoleDto(role);
      })
    );
}
