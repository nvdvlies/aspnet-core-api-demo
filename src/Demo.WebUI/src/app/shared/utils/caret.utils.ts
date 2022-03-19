export class CaretUtils {
  public static getPosition(element: HTMLInputElement): number {
    return element?.selectionStart ?? -1;
  }

  public static setPosition(element: HTMLInputElement, position: number): void {
    element?.setSelectionRange(position, position);
  }
}
