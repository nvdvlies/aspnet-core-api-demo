import { BehaviorSubject } from 'rxjs';
import { combineLatest, Observable, of, Subject } from 'rxjs';
import { tap, map, switchMap, filter, mergeMap } from 'rxjs/operators';

export interface IEntity<T> {
  id: string;
  clone(): T;
}

export interface IEntityUpdatedEvent {
  id: string;
  updatedBy: string;
}

export interface IEntityDeletedEvent {
  id: string;
  deletedBy: string;
}

export abstract class StoreBase<T extends IEntity<T>> {
  protected readonly cache = new BehaviorSubject<T[]>([]);

  protected abstract getByIdFunction: (id: string) => Observable<T>;
  protected abstract createFunction: (entity: T) => Observable<void>;
  protected abstract updateFunction: (entity: T) => Observable<void>;
  protected abstract deleteFunction: (id: string) => Observable<void>;

  protected abstract entityUpdatedEvent$: Observable<IEntityUpdatedEvent>;
  protected abstract entityDeletedEvent$: Observable<IEntityDeletedEvent>;

  protected readonly entityUpdatedInStore = new Subject<[IEntityUpdatedEvent, T]>(); 
  protected readonly entityDeletedFromStore = new Subject<IEntityDeletedEvent>(); 
  protected readonly createdBySelf = new Subject<T>();
  
  private get items(): T[] {
    return this.cache.getValue();
  }

  private set items(value: T[]) {
    this.cache.next(value);
  }

  constructor() {
  }

  protected init(): void {
    this.subscribeToEvents();
  }

  protected getById(id: string, skipCache: boolean = false, getByIdFunction?: (id: string) => Observable<T>): Observable<T> {
    let entityFromCache = !skipCache
      ? this.cache.value.find(x => x.id === id)
      : null;

    if (entityFromCache != null) {
      return of(entityFromCache);
    }

    getByIdFunction ??= this.getByIdFunction;
    return getByIdFunction(id)
        .pipe(
          map((entity) => {
            this.addToOrReplaceCache(entity);
            return entity;
          })
        );
  }

  protected create(entity: T, createFunction?: (entity: T) => Observable<void>): Observable<T> {
    createFunction ??= this.createFunction;
    return createFunction(entity)
      .pipe(
        switchMap(_ => {
          return this.getById(entity.id);
        }),
        tap((createdEntity) => {
          this.createdBySelf.next(createdEntity);
        })
      );
  }

  protected update(entity: T, updateFunction?: (entity: T) => Observable<void>): Observable<T> {
    updateFunction ??= this.updateFunction;
    return updateFunction(entity)
      .pipe(
        switchMap(_ => {
          return this.getById(entity.id, true);
        })
      );
  }

  protected delete(id: string, deleteFunction?: (id: string) => Observable<void>): Observable<void> {
    deleteFunction ??= this.deleteFunction;
    return deleteFunction(id)
      .pipe(
        tap(() => {
          this.removeFromCache(id);
        }),
      );
  }

  private addToCache(item: T): void {
    this.items = [
      ...this.items,
      item.clone()
    ];
  }

  private addToOrReplaceCache(updatedItem: T): void {
    if (!this.items.find(x => x.id === updatedItem.id)) {
      this.addToCache(updatedItem);
      return;
    }

    this.items = this.items.map((item, _) => {
      if (item.id !== updatedItem.id) {
        return item;
      }
      return updatedItem;
    });
  }

  private removeFromCache(id: string): void {
    this.items = this.items.filter(x => x.id !== id);
  }

  private subscribeToEvents(): void {
    this.subscribeToUpdatedEvent();
    this.subscribeToDeletedEvent();
  }

  private subscribeToUpdatedEvent(): void {
    this.entityUpdatedEvent$
      .pipe(
        filter(event => this.cache.value.find(x => x.id === event.id) != null),
        // todo: filter created by self
        mergeMap(event => combineLatest([of(event), this.getById(event.id, true)]))
      )
      .subscribe(([event, invoice]) => {
        this.addToOrReplaceCache(invoice);
        this.entityUpdatedInStore.next([event, invoice]);
      });
  }

  private subscribeToDeletedEvent(): void {
    this.entityDeletedEvent$
      .pipe(
        filter(event => this.cache.value.find(x => x.id === event.id) != null),
      )
      .subscribe(event => {
        this.removeFromCache(event.id);
        this.entityDeletedFromStore.next(event);
      });
  }
}