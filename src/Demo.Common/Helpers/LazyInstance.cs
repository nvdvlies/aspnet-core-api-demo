using System;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Common.Helpers
{
    public class LazyInstance<T> : Lazy<T>
    {
        public LazyInstance(IServiceProvider serviceProvider)
            : base(() => serviceProvider.GetRequiredService<T>())
        {
        }
    }
}
