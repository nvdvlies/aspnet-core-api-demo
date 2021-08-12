using AutoMapper;
using Demo.Domain.Shared.Entities;
using Demo.Domain.Shared.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace Demo.Application.Shared.Mappings
{
    public static class Extensions
    {
        public static void MapFrom<From, To>(this IBusinessComponent<To> bc, From from, IMapper mapper) where To : Entity
        {
            bc.With(entity => mapper.Map(from, entity));
        }

        public static To MapTo<From, To>(this IBusinessComponent<From> bc, IMapper mapper) where From : Entity where To : new()
        {
            var to = new To();
            bc.With(entity => mapper.Map(entity, to));
            return to;
        }

        public static void PatchFrom<From, To>(this IBusinessComponent<To> bc, JsonPatchDocument<From> from, IMapper mapper) 
            where To : Entity
            where From : class
        {
            var entityJsonPatchDocument = mapper.Map(from, new JsonPatchDocument<To>());
            bc.With(entity => entityJsonPatchDocument.ApplyTo(entity));
        }
    }
}
