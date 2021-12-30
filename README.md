# dotnet-api-demo

Demo architecture for a .NET Web API. Similar to Jason Taylor's [CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) template. Notable difference is an encapsulation of aggregate root entities with a thing I called a 'DomainEntity'.
A DomainEntity offers:

 - Validation. [View example](https://github.com/nvdvlies/dotnet-api-demo/blob/main/src/Demo.Domain/Invoice/Validators/NotAllowedToDeleteInvoiceInStatusValidator.cs).
 - Hooks (BeforeCreate, BeforeUpdate, BeforeDelete, AfterCreate, AfterUpdate, AfterDelete). [View example](https://github.com/nvdvlies/dotnet-api-demo/blob/main/src/Demo.Domain/Invoice/Hooks/SynchronizeInvoicePdfDomainEventHook.cs).
 - Event publishing with outbox pattern. [View example](https://github.com/nvdvlies/dotnet-api-demo/blob/main/src/Demo.Domain/Invoice/Hooks/InvoiceStatusDomainEventHook.cs).
 - Access to pristine property values in validators and hooks. [View example](https://github.com/nvdvlies/dotnet-api-demo/blob/main/src/Demo.Domain/Invoice/Validators/NotAllowedToModifyInvoiceContentInStatusValidator.cs).
 - Auditlogging. [View example](https://github.com/nvdvlies/dotnet-api-demo/blob/main/src/Demo.Infrastructure/Auditlogging/InvoiceAuditlogger.cs).
