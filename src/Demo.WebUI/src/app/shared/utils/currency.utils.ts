import { NumberUtils } from '@shared/utils/number.utils';

export interface LocaleSpecificTokens {
  currency: string;
  groupSeparator: string;
  decimalSeparator: string;
}

export interface FormatOptions {
  locale: string;
  currency: string;
  fractionDigits: number;
}

export interface ParseOptions {
  locale: string;
  fractionDigits: number;
}

export class CurrencyUtils {
  public static DefaultFormatOptions: FormatOptions = {
    locale: 'nl-NL',
    currency: 'EUR',
    fractionDigits: 2
  } as FormatOptions;

  public static DefaultParseOptions: ParseOptions = {
    locale: 'nl-NL',
    fractionDigits: 2
  } as ParseOptions;

  public static getLocaleSpecificTokens(locale: string): LocaleSpecificTokens {
    const parts = Intl.NumberFormat(locale).formatToParts(123456789);
    return {
      currency: parts.find((part) => part.type === 'currency')?.value ?? '€',
      groupSeparator: parts.find((part) => part.type === 'group')?.value ?? '.',
      decimalSeparator: parts.find((part) => part.type === 'decimal')?.value ?? ','
    } as LocaleSpecificTokens;
  }

  public static format(value: number | null | undefined, options?: FormatOptions): string | null {
    if (!value) {
      return null;
    }
    if (!NumberUtils.isNumeric(value)) {
      return null;
    }
    options ??= CurrencyUtils.DefaultFormatOptions;
    return new Intl.NumberFormat(options.locale, {
      style: 'currency',
      currency: options.currency,
      maximumFractionDigits: options.fractionDigits,
      minimumFractionDigits: options.fractionDigits,
      minimumIntegerDigits: 1
    }).format(value);
  }

  public static parse(value: any, options?: ParseOptions): number | null {
    if (!value) {
      return null;
    }
    options ??= CurrencyUtils.DefaultFormatOptions;
    if (NumberUtils.isNumeric(value)) {
      return +Number(value).toFixed(options.fractionDigits);
    }
    const tokens = CurrencyUtils.getLocaleSpecificTokens(options.locale);
    let unformattedValue = value
      .replaceAll(tokens.groupSeparator, '')
      .replaceAll(tokens.decimalSeparator, '.')
      .replaceAll(tokens.currency, '');
    if (NumberUtils.isNumeric(unformattedValue)) {
      return +Number(unformattedValue).toFixed(options.fractionDigits);
    } else {
      return null;
    }
  }
}
