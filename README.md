# clean-architecture-with-business-components-demo

An example achitecture for a ASP.NET CORE API inspired by Jason Taylor's [CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) template. 
Notable difference is an encapsulation of domain layer entities with a thing I called a 'BusinessComponent'. 
A BusinessComponent encapsulates an aggregate root entity and offers 

 - Validation
 - Hooks (BeforeCreate, BeforeUpdate, BeforeDelete, AfterCreate, AfterUpdate, AfterDelete)
 - Publish domain events
 - Pristine values + IsPropertyDirty check available in validators and hooks
 - Auditlogging (configurable)
 