using Infrastructure.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Group : Enumeration<Group, string>
    {
        public static Group All = new Group("ALL", "All");
        public static Group Any = new Group("Any", "Any");
        public static Group Not = new Group("Not", "Not");

        public Group(string value, string displayName) : base(value, displayName) { }
    }
}
