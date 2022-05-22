export class StringUtils {
  public static isNullOrEmpty(value: any): boolean {
    return !value || value.length === 0;
  }

  public static isNullOrWhitespace(value: any): boolean {
    return !value || value.length === 0 || /^\s*$/.test(value);
  }

  public static isGuid(value: any): boolean {
    return (
      value &&
      /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(value) &&
      value !== '00000000-0000-0000-0000-000000000000'
    );
  }

  public static isEmptyGuid(value: any): boolean {
    return value && value === '00000000-0000-0000-0000-000000000000';
  }
}
