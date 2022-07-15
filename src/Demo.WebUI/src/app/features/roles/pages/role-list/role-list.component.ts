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
import { combineLatest, map, Observable, tap } from 'rxjs';
import { RoleTableDataSource } from '@roles/pages/role-list/role-table-datasource';
import {
  RoleTableDataContext,
  RoleTableDataService
} from '@roles/pages/role-list/role-table-data.service';
import { SearchRoleDto } from '@api/api.generated.clients';
import { TableFilterCriteria } from '@shared/base/table-data-base';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';

interface ViewModel extends RoleTableDataContext {}

export interface RoleListRouteState {
  spotlightIdentifier: string | undefined;
}

@Component({
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RoleListComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
  @ViewChildren('tableRows', { read: ElementRef }) tableRows: QueryList<ElementRef> | undefined;

  public displayedColumns = ['Name'];
  public dataSource!: RoleTableDataSource;
  public searchTerm = this.roleTableDataService.searchTerm;

  private vm: Readonly<ViewModel> | undefined;

  public vm$: Observable<ViewModel> = combineLatest([this.roleTableDataService.observe$]).pipe(
    map(([baseContext]) => {
      const context: ViewModel = {
        ...baseContext
      };
      return context;
    }),
    tap((vm) => (this.vm = vm))
  );

  constructor(
    private readonly location: Location,
    private readonly router: Router,
    private readonly roleTableDataService: RoleTableDataService
  ) {}

  public ngOnInit(): void {
    this.dataSource = new RoleTableDataSource(this.roleTableDataService);
    this.spotlight();
  }

  private spotlight(): void {
    const state = this.location.getState() as RoleListRouteState;
    if (state && state.spotlightIdentifier) {
      this.roleTableDataService.spotlight(state.spotlightIdentifier);
      history.replaceState({ ...state, spotlightIdentifier: null }, '');
    }
  }

  public search(criteria: TableFilterCriteria): void {
    this.roleTableDataService.search(criteria);
  }

  public trackById(index: number, item: SearchRoleDto): string {
    return item.id;
  }

  public navigateToDetailsById(id: string, index?: number): void {
    if (index != undefined) {
      this.roleTableDataService.selectedItemIndex = index;
    }
    this.router.navigate(['/roles', id]);
  }

  @HostListener('document:keydown.shift.alt.enter', ['$event'])
  public navigateToDetails(event: Event): void {
    if (!this.vm?.selectedItem) {
      return;
    }
    this.navigateToDetailsById(this.vm.selectedItem.id);
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowUp', ['$event'])
  public navigateTableUp(event: Event): void {
    this.roleTableDataService.selectedItemIndex -= 1;
    this.tableRows
      ?.get(this.roleTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.ArrowDown', ['$event'])
  public navigateTableDown(event: Event): void {
    this.roleTableDataService.selectedItemIndex += 1;
    this.tableRows
      ?.get(this.roleTableDataService.selectedItemIndex)
      ?.nativeElement.scrollIntoView(false, { behavior: 'auto', block: 'end' });
    event.preventDefault();
  }

  @HostListener('document:keydown.shift.alt.n', ['$event'])
  public newRoleShortcut(event: KeyboardEvent) {
    this.router.navigateByUrl('/roles/new');
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
    this.roleTableDataService.clearSpotlight();
  }
}
