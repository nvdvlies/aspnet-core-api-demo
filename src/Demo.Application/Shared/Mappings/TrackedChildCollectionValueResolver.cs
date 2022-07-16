using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Demo.Application.Shared.Interfaces;
using Demo.Domain.Shared.Interfaces;

namespace Demo.Application.Shared.Mappings;

public class
    TrackedChildCollectionValueResolver<TDto, TEntity, TDtoCollection, TEntityCollection> : IMemberValueResolver<
        TDto, TEntity, List<TDtoCollection>, List<TEntityCollection>>
    where TDtoCollection : ICreateOrUpdateEntityDto
    where TEntityCollection : IEntity
{
    private readonly IMapper _mapper;

    public TrackedChildCollectionValueResolver(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<TEntityCollection> Resolve(TDto source, TEntity destination, List<TDtoCollection> dtoCollection,
        List<TEntityCollection> entityCollection, ResolutionContext context)
    {
        var resultCollection = new List<TEntityCollection>();
        foreach (var item in dtoCollection)
        {
            var existingItem = entityCollection.SingleOrDefault(x => x.Id == item.Id);
            if (existingItem != null)
            {
                _mapper.Map(item, existingItem);
                resultCollection.Add(existingItem);
            }
            else
            {
                resultCollection.Add(_mapper.Map<TEntityCollection>(item));
            }
        }

        return resultCollection;
    }
}
