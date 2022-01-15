import { BehaviorSubject } from 'rxjs';

export interface IEntity<T> {
  id: string;
  clone(): T;
}

export class StoreBase<T extends IEntity<T>> {
  protected readonly cache = new BehaviorSubject<T[]>([]);

  protected get items(): T[] {
    return this.cache.getValue();
  }

  protected set items(value: T[]) {
    this.cache.next(value);
  }

  protected addToCache(item: T): void {
    this.items = [
      ...this.items,
      item.clone()
    ];
  }

  protected addToOrReplaceCache(updatedItem: T): void {
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

  protected removeFromCache(id: string): void {
    this.items = this.items.filter(x => x.id !== id);
  }
}
