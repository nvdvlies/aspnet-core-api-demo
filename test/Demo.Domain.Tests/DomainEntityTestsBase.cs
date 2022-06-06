using System;
using System.Collections.Generic;
using Demo.Common;
using Demo.Common.Helpers;
using Demo.Common.Interfaces;
using Demo.Domain.Shared.Interfaces;
using Demo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Configuration;

namespace Demo.Domain.Tests
{
    public abstract class DomainEntityTestsBase<T> where T: IEntity
    {
        protected ServiceProvider ServiceProvider;

        protected readonly Mock<ICurrentUser> CurrentUserMock = new Mock<ICurrentUser>();
        //protected readonly Mock<IDateTime> DateTimeMock = new Mock<IDateTime>();
        //protected readonly Mock<IDbCommand<T>> DbCommandMock = new Mock<IDbCommand<T>>();

        protected readonly Mock<IOutboxEventCreator> OutboxEventCreatorMock = new Mock<IOutboxEventCreator>();
        protected readonly Mock<IOutboxMessageCreator> OutboxMessageCreatorMock = new Mock<IOutboxMessageCreator>();

        protected DomainEntityTestsBase()
        {
            CurrentUserMock.Setup(x => x.Id).Returns(Guid.Parse("BCD06F09-25E6-4D2F-98CA-4A227902CDC4"));
            //DateTimeMock.Setup(x => x.UtcNow).Returns(() => DateTime.UtcNow);

            var myConfiguration = new Dictionary<string, string>
            {
                {"ConnectionStrings:SqlDatabase", "Data Source=localhost;initial catalog=Demo;Integrated Security=True; MultipleActiveResultSets=True"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddCommon();
            services.AddDomain();
            services.AddInfrastructure(configuration);
            services.AddSingleton(CurrentUserMock.Object);
            //services.AddSingleton(DateTimeMock.Object);
            //services.AddSingleton(DbCommandMock.Object);

            //services.AddSingleton(OutboxEventCreatorMock.Object);
            //services.AddSingleton(OutboxMessageCreatorMock.Object);
            //services.AddSingleton(DbCommandMock.Object);
            //services.AddSingleton(typeof(IJsonService<>), typeof(JsonService<>));
            //services.AddTransient(typeof(IAuditlogger<>), typeof(Auditlogger<>));

           // services.AddTransient(typeof(Lazy<>), typeof(LazyInstance<>));

            /*AddInfra + replace
             IEventPublisher
IMessageSender
ApplicationDbContext
IDistributedMemoryCache
IAuth0UserManagementClient
IAuth0ManagementApiClientProvider
             */
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}