using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common.Interfaces;
using Demo.Domain.Auditlog;
using Demo.Domain.Auditlog.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure.Auditlogging;
using Moq;

namespace Demo.Infrastructure.Tests.Auditlogging
{
    public abstract class AuditloggerTestsBase
    {
        protected readonly Mock<ICurrentUser> CurrentUserMock = new Mock<ICurrentUser>();
        protected readonly Mock<IDateTime> DateTimeMock = new Mock<IDateTime>();
        protected readonly Mock<IAuditlogDomainEntity> AuditlogDomainEntityMock = new Mock<IAuditlogDomainEntity>();

        internal readonly Auditlog CapturedAuditlog = new Auditlog();

        protected AuditloggerTestsBase()
        {
            CurrentUserMock.Setup(x => x.Id).Returns(Guid.Parse("BCD06F09-25E6-4D2F-98CA-4A227902CDC4"));
            DateTimeMock.Setup(x => x.UtcNow).Returns(() => DateTime.UtcNow);

            AuditlogDomainEntityMock
                .Setup(x => x.NewAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            AuditlogDomainEntityMock
                .Setup(x => x.With(It.IsAny<Action<Auditlog>>()))
                .Callback((Action<Auditlog> action) => {
                    action(CapturedAuditlog);
                })
                .Verifiable();
            AuditlogDomainEntityMock
                .Setup(x => x.CreateAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        protected void VerifyCreateAsyncCall(Func<Times> times)
        {
            AuditlogDomainEntityMock.Verify(x => x.CreateAsync(It.IsAny<CancellationToken>()), times);
        }
    }
}