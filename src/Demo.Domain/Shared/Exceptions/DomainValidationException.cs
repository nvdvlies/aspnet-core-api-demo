using Demo.Domain.Shared.DomainEntity;
using Demo.Domain.Shared.Extensions;
using System;
using System.Collections.Generic;

namespace Demo.Domain.Shared.Exceptions
{
    public class DomainValidationException : Exception
    {
        public IEnumerable<ValidationMessage> ValidationMessages { get; private set; }

        public DomainValidationException()
        { }

        public DomainValidationException(IEnumerable<ValidationMessage> validationMessages)
            : base(validationMessages.AsString())
        {
            ValidationMessages = validationMessages;
        }

        public DomainValidationException(IEnumerable<ValidationMessage> validationMessages, Exception innerException)
            : base(validationMessages.AsString(), innerException)
        {
            ValidationMessages = validationMessages;
        }
    }
}
