using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicDbModelBuilder.Tests
{
    public class Unicorn
    {
        public int Id { get; set; }
        public int? CornCount { get; set; }
        public DateTime BornOn { get; set; }
        public DateTime? LastMatedOn { get; set; }
    }
}
