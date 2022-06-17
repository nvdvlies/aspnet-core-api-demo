using System;
using System.Collections.Generic;
using System.Reflection;

namespace Demo.SignalrTypescript.Generator.Models
{
    internal class Parameter
    {
        public Parameter(ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;
            CSharpType = parameterInfo.ParameterType.Name;
            var typeMapping = new Dictionary<Type, string>
            {
                { typeof(int), "number" },
                { typeof(string), "string" },
                { typeof(Guid), "string" },
                { typeof(DateTime), "Date" }
            };
            TypescriptType = typeMapping[parameterInfo.ParameterType];
        }

        public string Name { get; }
        public string CSharpType { get; }
        public string TypescriptType { get; }
    }
}