export class NumberUtils {
  public static isNumeric(value: any): boolean {
    return !isNaN(+value);
  }
}
