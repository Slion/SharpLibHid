using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLib.Hid.Property
{
    public class String: Base
    {
        public String(string aName, string aValue)
        {
            Name = aName;
            Value = aValue;
        }

        public string Value { get; private set; }
        public override string ToString() { return Value; }
    }
}
