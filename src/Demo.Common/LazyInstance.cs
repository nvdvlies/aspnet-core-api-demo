using Microsoft.Extensions.DependencyInjection;
using System;

namespace Demo.Common
{
    public class LazyInstance<T> : Lazy<T>
    {
        public LazyInstance(IServiceProvider serviceProvider)
            : base(() => serviceProvider.GetRequiredService<T>())
        {

        }
    }
}
