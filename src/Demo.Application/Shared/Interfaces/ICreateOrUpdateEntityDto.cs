using System;

namespace Demo.Application.Shared.Interfaces;

public interface ICreateOrUpdateEntityDto
{
    public Guid? Id { get; set; }
}
