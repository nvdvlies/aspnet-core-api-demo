import { CacheBase } from '@shared/base/cache.base';
import { Observable, of, Subject } from 'rxjs';
import { tap, map, switchMap, filter, finalize } from 'rxjs/operators';

export interface IEntity<T> {
  id: string;
  lastModifiedBy?: string | undefined;
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

export abstract class StoreBase<T extends IEntity<T>> extends CacheBase<T> {
  protected abstract getByIdFunction: (id: string) => Observable<T>;
  protected abstract createFunction: (entity: T) => Observable<string>;
  protected abstract updateFunction: (entity: T) => Observable<void>;
  protected abstract deleteFunction: (id: string) => Observable<void>;

  protected abstract entityUpdatedEvent$: Observable<IEntityUpdatedEvent>;
  protected abstract entityDeletedEvent$: Observable<IEntityDeletedEvent>;

  protected readonly entityUpdatedInStore = new Subject<[IEntityUpdatedEvent, T]>();
  protected readonly entityDeletedFromStore = new Subject<IEntityDeletedEvent>();

  private updateLockEntityId: string | undefined;

  constructor() {
    super();
  }

  protected init(): void {
    this.subscribeToEvents();
  }

  protected getById(
    id: string,
    skipCache: boolean = false,
    getByIdFunction?: (id: string) => Observable<T>
  ): Observable<T> {
    let entityFromCache = !skipCache
      ? this.cache.value.find((x) => x.id.toLowerCase() === id.toLowerCase())
      : null;

    if (entityFromCache != null) {
      return of(entityFromCache);
    }

    getByIdFunction ??= this.getByIdFunction;
    return getByIdFunction(id).pipe(
      map((entity) => {
        this.addToOrReplaceInCache(entity);
        if (skipCache && entityFromCache != null) {
          this.publishEntityUpdatedInStore(entity);
        }
        return entity;
      })
    );
  }

  protected create(entity: T, createFunction?: (entity: T) => Observable<string>): Observable<T> {
    createFunction ??= this.createFunction;
    return createFunction(entity).pipe(
      switchMap((id) => {
        return this.getById(id);
      })
    );
  }

  protected update(entity: T, updateFunction?: (entity: T) => Observable<void>): Observable<T> {
    this.updateLockEntityId = entity.id;
    updateFunction ??= this.updateFunction;
    return updateFunction(entity).pipe(
      switchMap(() => this.getById(entity.id, true)),
      finalize(() => (this.updateLockEntityId = undefined))
    );
  }

  protected delete(
    id: string,
    deleteFunction?: (id: string) => Observable<void>
  ): Observable<void> {
    deleteFunction ??= this.deleteFunction;
    return deleteFunction(id).pipe(
      tap(() => {
        this.removeFromCache(id);
      })
    );
  }

  private subscribeToEvents(): void {
    this.subscribeToUpdatedEvent();
    this.subscribeToDeletedEvent();
  }

  private subscribeToUpdatedEvent(): void {
    this.entityUpdatedEvent$
      .pipe(
        filter((event) => this.existsInCache(event.id)),
        filter((event) => event.id != this.updateLockEntityId), // update() will refresh the entity itself, so we should ignore the event to prevent unnecessary requests
        switchMap((event) => this.getById(event.id, true))
      )
      .subscribe((entity) => {
        this.replaceInCache(entity);
        this.publishEntityUpdatedInStore(entity);
      });
  }

  private subscribeToDeletedEvent(): void {
    this.entityDeletedEvent$
      .pipe(filter((event) => this.existsInCache(event.id)))
      .subscribe((event) => {
        this.removeFromCache(event.id);
        this.entityDeletedFromStore.next(event);
      });
  }

  private publishEntityUpdatedInStore(entity: T): void {
    this.entityUpdatedInStore.next([
      {
        id: entity.id,
        updatedBy: entity.lastModifiedBy
      } as IEntityUpdatedEvent,
      entity
    ]);
  }
}
