using System;
using AutoMapper;
using Demo.Application.Shared.Dtos;
using Demo.Domain.Auditlog;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Application.Shared.Mappings
{
    public class AuditlogMappingProfile : Profile
    {
        public AuditlogMappingProfile()
        {
            CreateMap<AuditlogStatus, AuditlogStatusEnum>();
            CreateMap<AuditlogType, AuditlogTypeEnum>();
            CreateMap<Auditlog, AuditlogDto>();
            CreateMap<AuditlogItem, AuditlogItemDto>()
                .ForMember(dest => dest.CurrentValueAsString,
                    opt => opt.MapFrom<AuditlogValueResolver, string>(src => src.CurrentValueAsString)
                )
                .ForMember(dest => dest.PreviousValueAsString,
                    opt => opt.MapFrom<AuditlogValueResolver, string>(src => src.PreviousValueAsString)
                );
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (AuditlogStatusEnum)src.Status))
            //.ForMember(dest => dest.Type, opt => opt.MapFrom(src => (AuditlogTypeEnum)src.Type));
        }

        private class AuditlogValueResolver : IMemberValueResolver<AuditlogItem, AuditlogItemDto, string, string>
        {
            private readonly ICultureProvider _cultureProvider;
            private readonly ITimeZoneProvider _timeZoneProvider;

            public AuditlogValueResolver(ITimeZoneProvider timeZoneProvider, ICultureProvider cultureProvider)
            {
                _timeZoneProvider = timeZoneProvider;
                _cultureProvider = cultureProvider;
            }

            public string Resolve(AuditlogItem source, AuditlogItemDto destination, string sourceValue,
                string destinationValue, ResolutionContext context)
            {
                if (string.IsNullOrWhiteSpace(sourceValue))
                {
                    return sourceValue;
                }

                switch (source.Type)
                {
                    case AuditlogType.Text:
                        return sourceValue;
                    case AuditlogType.DateOnly:
                    case AuditlogType.DateTime:
                    case AuditlogType.TimeOnly:
                        var sourceValueAsUtcDate = DateTime.Parse(sourceValue);
                        var sourceValueAsLocalDate =
                            TimeZoneInfo.ConvertTime(sourceValueAsUtcDate, _timeZoneProvider.TimeZone);
                        switch (source.Type)
                        {
                            case AuditlogType.DateOnly:
                                return sourceValueAsLocalDate.ToString("d", _cultureProvider.Culture);
                            case AuditlogType.DateTime:
                                return sourceValueAsLocalDate.ToString("g", _cultureProvider.Culture);
                            case AuditlogType.TimeOnly:
                                return sourceValueAsLocalDate.ToString("t", _cultureProvider.Culture);
                        }

                        return null;
                    case AuditlogType.Decimal:
                    case AuditlogType.Currency:
                        var sourceValueAsDecimal = decimal.Parse(sourceValue);
                        switch (source.Type)
                        {
                            case AuditlogType.Decimal:
                                return sourceValueAsDecimal.ToString(_cultureProvider.Culture);
                            case AuditlogType.Currency:
                                return sourceValueAsDecimal.ToString("C", _cultureProvider.Culture);
                        }

                        return null;
                    case AuditlogType.Number:
                        return sourceValue;
                    case AuditlogType.OnOff:
                    case AuditlogType.YesNo:
                        var sourceValueAsBoolean = sourceValue == "1";
                        switch (source.Type)
                        {
                            case AuditlogType.OnOff:
                                return sourceValueAsBoolean ? "On" : "Off";
                            case AuditlogType.YesNo:
                                return sourceValueAsBoolean ? "Yes" : "No";
                        }

                        return null;
                }

                return null;
            }
        }
    }
}