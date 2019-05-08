using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDbInitialiser
{
    public class Head
    {
        public ICollection<Field> Fields { get; set; }

        public Head()
        {
            this.Fields = new List<Field>();
        }

        public Head(ICollection<Field> fields)
        {
            this.Fields = fields;
        }
    }
}
