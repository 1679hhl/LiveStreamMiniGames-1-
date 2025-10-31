using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DropdownProAttribute : DrawerAttribute
    {
        public readonly string ListName;

        public DropdownProAttribute(string listName)
        {
            ListName = listName;
        }
    }
}