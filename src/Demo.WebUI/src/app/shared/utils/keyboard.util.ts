import { NonPrintableKey } from '@shared/enums/non-printable-key.enum';

export class KeyboardUtils {
  public static isNonPrintableKey(event: KeyboardEvent): boolean {
    if (event == null) {
      return false;
    }
    return (
      event.ctrlKey ||
      Object.entries(NonPrintableKey).find(([key, value]) => value === event.key) != null
    );
  }
}
