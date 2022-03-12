import { Injectable, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject, takeUntil, tap } from 'rxjs';

export interface ILookupEntity {
  id?: string | undefined;
}

@Injectable()
export abstract class LookupBase<T extends ILookupEntity> implements OnInit, OnDestroy {
  protected abstract getByIdFunction: (id: string) => Observable<T | undefined>;
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

  protected addToCache(item: T): void {
    this.items = [...this.items, item];
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
          if (item) {
            this.addToCache(item);
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
