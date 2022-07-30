using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using Demo.Domain.Shared.Interfaces;
using FluentValidation;

namespace Demo.Domain.Location.Validators;

internal class LocationValidator : AbstractValidator<Location>, Shared.Interfaces.IValidator<Location>
{
    public async Task<IEnumerable<ValidationMessage>> ValidateAsync(IDomainEntityContext<Location> context,
        CancellationToken cancellationToken = default)
    {
        RuleFor(customer => customer.DisplayName).NotEmpty().MaximumLength(255);
        RuleFor(customer => customer.StreetName).NotEmpty().MaximumLength(100);
        RuleFor(customer => customer.HouseNumber).NotEmpty().MaximumLength(10);
        RuleFor(customer => customer.PostalCode).NotEmpty().MaximumLength(10);
        RuleFor(customer => customer.City).NotEmpty().MaximumLength(100);
        RuleFor(customer => customer.CountryCode).NotEmpty().MaximumLength(3);
        RuleFor(customer => customer.Latitude).NotEmpty();
        RuleFor(customer => customer.Longitude).NotEmpty();

        var validationResult = await ValidateAsync(context.Entity, cancellationToken);
        return validationResult.ToValidationMessage();
    }
}
