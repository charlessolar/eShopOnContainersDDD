using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Setup.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependsAttribute : Attribute
    {
        public DependsAttribute(params string[] depends)
            : base()
        {
            this.Depends = depends;
        }

        public string[] Depends { get; set; }
    }
}
