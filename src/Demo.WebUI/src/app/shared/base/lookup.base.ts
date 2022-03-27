import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject, takeUntil, tap } from 'rxjs';

export interface ILookupEntity {
  id?: string | undefined;
}

@Injectable()
export abstract class LookupBase<T extends ILookupEntity> implements OnInit, OnDestroy {
  protected abstract getByIdFunction: (id: string) => Observable<T | undefined>;
  protected abstract getBatchByIdFunction: (ids: string[]) => Observable<T[]>;
  protected abstract entityUpdatedEvent$: Observable<string>;

  protected readonly onDestroy = new Subject<void>();
  protected onDestroy$ = this.onDestroy.asObservable();

  protected readonly cache = new BehaviorSubject<T[]>([]);

  protected get items(): T[] {
    return this.cache.getValue();
  }

  protected set items(value: T[]) {
    this.cache.next(value);
  }

  private addToOrReplaceInCache(item: T): void {
    if (this.existsInCache(item)) {
      this.replaceInCache(item);
    } else {
      this.addToCache(item);
    }
  }

  private addToCache(item: T): void {
    this.items = [...this.items, item];
  }

  private replaceInCache(updatedItem: T): void {
    this.items = this.items.map((item, _) => {
      if (item.id !== updatedItem.id) {
        return item;
      }
      return updatedItem;
    });
  }

  private existsInCache(item: T): boolean {
    if (!item || !item.id) {
      return false;
    }
    return this.cache.value.find((x) => x.id?.toLowerCase() === item?.id?.toLowerCase()) != null;
  }

  protected removeFromCache(id: string): void {
    this.items = this.items.filter((x) => x.id !== id);
  }

  public ngOnInit(): void {
    this.subscribeToEntityUpdatedEvent();
  }

  public getById(id: string, skipCache: boolean = false): Observable<T | undefined> {
    const cachedItem = !skipCache
      ? this.cache.value.find((x) => x.id?.toLowerCase() === id.toLowerCase())
      : null;
    if (cachedItem) {
      return of(cachedItem);
    } else {
      return this.getByIdFunction(id).pipe(
        tap((item) => {
          if (item && item.id) {
            this.addToOrReplaceInCache(item);
          }
        })
      );
    }
  }

  public getBatchById(ids: string[], skipCache: boolean = false): Observable<T[]> {
    const cachedItems = !skipCache
      ? this.cache.value.filter((x) => ids.some((id) => x.id?.toLowerCase() === id.toLowerCase()))
      : [];
    if (cachedItems.length === ids.length) {
      return of(cachedItems);
    } else {
      return this.getBatchByIdFunction(ids).pipe(
        tap((items) => {
          if (items) {
            for (const item of items) {
              if (item && item.id) {
                this.addToOrReplaceInCache(item);
              }
            }
          }
        })
      );
    }
  }

  private subscribeToEntityUpdatedEvent(): void {
    this.entityUpdatedEvent$.pipe(takeUntil(this.onDestroy$)).subscribe((id) => {
      this.removeFromCache(id);
    });
  }

  public ngOnDestroy(): void {
    this.onDestroy.next();
    this.onDestroy.complete();
  }
}
