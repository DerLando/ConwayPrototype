using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayPrototype.Core.Parsing
{
    public class Token
    {
        public Operation Operation { get; set; }
        public int Numeric { get; set; }
    }
}
