using System;

namespace Hcs.Platform.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreOnUpdate : System.Attribute
    {
    }
}
