using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Demo.Common.Helpers;

public class ClassFinder
{
    private IEnumerable<Type> _types;

    public ClassFinder(IEnumerable<Assembly> assemblies)
    {
        _types = assemblies
            .SelectMany(x => x.DefinedTypes.Distinct())
            .Where(t => t.IsClass)
            .Where(t => !t.IsInterface)
            .Where(t => !t.IsAbstract);
    }

    public ClassFinder ClassesThatImplementInterface(Type type)
    {
        if (type.IsGenericType)
        {
            _types = _types.Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type));
        }
        else
        {
            _types = _types.Where(t => type.IsAssignableFrom(t));
        }

        return this;
    }

    public ClassFinder ClassesThatDoNotImplementInterface(Type type)
    {
        if (type.IsGenericType)
        {
            _types = _types.Where(t =>
                !t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type));
        }
        else
        {
            _types = _types.Where(t => !type.IsAssignableFrom(t));
        }

        return this;
    }

    public void ForEach(Action<Type> action)
    {
        foreach (var type in _types)
        {
            action(type);
        }
    }

    public static ClassFinder SearchInAssemblies(IEnumerable<Assembly> assemblies)
    {
        return new ClassFinder(assemblies);
    }

    public static ClassFinder SearchInAssembly(Assembly assembly)
    {
        return SearchInAssemblies(new[] { assembly });
    }
}
