using AutoMapper;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Application.Shared.Mappings
{
    public static class Extensions
    {
        public static void MapFrom<From, To>(this IDomainEntity<To> domainEntity, From from, IMapper mapper)
            where To : Entity
        {
            domainEntity.With(entity => mapper.Map(from, entity));
        }

        public static To MapTo<From, To>(this IDomainEntity<From> domainEntity, IMapper mapper)
            where From : Entity where To : new()
        {
            var to = new To();
            domainEntity.With(entity => mapper.Map(entity, to));
            return to;
        }
    }
}