using System;

namespace Demo.Common.Interfaces
{
    public interface ICorrelationIdProvider
    {
        Guid Id { get; }
        void SwitchToCorrelationId(Guid id);
    }
}