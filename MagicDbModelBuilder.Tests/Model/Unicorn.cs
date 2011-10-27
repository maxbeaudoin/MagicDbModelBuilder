using System;

namespace MagicDbModelBuilder.Tests.Model
{
    public class Unicorn
    {
        public int Id { get; set; }
        public int? ChildCount { get; set; }
        public DateTime BornOn { get; set; }
        public DateTime? LastMatedOn { get; set; }
        public Corn Corn { get; set; }
    }
}
