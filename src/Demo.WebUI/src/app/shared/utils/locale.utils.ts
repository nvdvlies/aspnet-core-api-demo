export class LocaleUtils {
  public static getLocale(): string {
    return navigator.languages?.length ? navigator.languages[0] : navigator.language;
  }
}
