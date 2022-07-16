using System;

namespace Demo.Domain.Shared.Interfaces;

public interface IAuditableEntity : IEntity
{
    public Guid CreatedBy { get; }

    public DateTime CreatedOn { get; }

    public Guid LastModifiedBy { get; }

    public DateTime? LastModifiedOn { get; }

    internal void SetCreatedByAndCreatedOn(Guid createdBy, DateTime createdOn);
    internal void SetLastModifiedByAndLastModifiedOn(Guid lastModifiedBy, DateTime lastModifiedOn);
    internal void ClearCreatedAndLastModified();
}
