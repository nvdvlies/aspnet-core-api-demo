# dotnet-api-and-angular-frontend

Demo architecture for a .NET backend and Angular frontend. 

Interesting part might be the encapsulation of aggregate root entities with a thing I called a 'DomainEntity'. 

A DomainEntity offers:

 - In the backend:
   - Validation. [View example](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.Domain/Invoice/Validators/NotAllowedToDeleteInvoiceInStatusValidator.cs).
   - Hooks (BeforeCreate, BeforeUpdate, BeforeDelete, AfterCreate, AfterUpdate, AfterDelete). [View example](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.Domain/Invoice/Hooks/SynchronizeInvoicePdfHook.cs).
   - Event publishing with outbox pattern. [View example](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.Domain/Invoice/Hooks/InvoiceStatusEventHook.cs).
   - Access to pristine property values in validators and hooks. [View example](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.Domain/Invoice/Validators/NotAllowedToModifyInvoiceContentInStatusValidator.cs).
   - Auditlogging. [View example](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.Infrastructure/Auditlogging/InvoiceAuditlogger.cs).
 - In the frontend:
   - Manages FormControls, state, API communication and error handling to keep the components thin. View example [component](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.WebUI/src/app/features/customers/customer-details/customer-details.component.ts) and [domain entity](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.WebUI/src/app/domain/invoice/invoice-domain-entity.service.ts).
   - Auto merging of changes others made when making changes yourself. [View example](https://github.com/nvdvlies/dotnet-api-and-angular-frontend/blob/main/src/Demo.WebUI/src/app/domain/shared/domain-entity-base.ts#L279).
