using System;

namespace City.Views
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class ComponentProviderAttribute: Attribute
    {
        public object Id { get; }

        public ComponentProviderAttribute(object id)
        {
            Id = id;
        }
    }
}