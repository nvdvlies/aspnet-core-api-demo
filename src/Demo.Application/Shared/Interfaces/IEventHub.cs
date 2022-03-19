﻿using Demo.Application.ApplicationSettings.Events;
using Demo.Application.Customers.Events;
using Demo.Application.Invoices.Events;
using Demo.Application.Users.Events;

namespace Demo.Application.Shared.Interfaces
{
    public interface IEventHub :
        ICustomerEventHub,
        IInvoiceEventHub,
        IApplicationSettingsEventHub,
        IUserEventHub
    {
    }
}
