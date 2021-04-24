using Demo.Domain.Shared.BusinessComponent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Shared.Interfaces
{
    internal interface IValidator<T> where T : IEntity
    {
        Task<IEnumerable<ValidationMessage>> ValidateAsync(IBusinessComponentContext<T> context, CancellationToken cancellationToken = default);
    }
}
