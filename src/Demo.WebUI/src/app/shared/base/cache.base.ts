import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ICacheItem {
  id: string;
}

@Injectable()
export abstract class CacheBase<T extends ICacheItem> {
  protected readonly cache = new BehaviorSubject<T[]>([]);

  private get items(): T[] {
    return this.cache.getValue();
  }

  private set items(value: T[]) {
    this.cache.next(value);
  }

  protected addToOrReplaceInCache(item: T): void {
    if (!item) {
      return;
    }
    if (this.existsInCache(item?.id)) {
      this.replaceInCache(item);
    } else {
      this.addToCache(item);
    }
  }

  protected addToCache(item: T): void {
    if (!item) {
      return;
    }
    this.items = [...this.items, item];
  }

  protected replaceInCache(updatedItem: T): void {
    if (!updatedItem) {
      return;
    }
    this.items = this.items.map((item, _) => {
      if (item.id !== updatedItem.id) {
        return item;
      }
      return updatedItem;
    });
  }

  protected existsInCache(id: string | undefined): boolean {
    if (!id) {
      return false;
    }
    return this.cache.value.find((x) => x.id?.toLowerCase() === id?.toLowerCase()) != null;
  }

  protected removeFromCache(id: string): void {
    if (!id) {
      return;
    }
    this.items = this.items.filter((x) => x.id !== id);
  }
}
