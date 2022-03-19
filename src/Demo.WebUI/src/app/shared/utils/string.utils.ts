export class StringUtils {
  public static isNullOrEmpty(value: any): boolean {
    return !value || value.length === 0;
  }

  public static isNullOrWhitespace(value: any): boolean {
    return !value || value.length === 0 || /^\s*$/.test(value);
  }
}
