using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Inventory
    {
        public string ItemName { get; set; }
        public int CharacterId { get; set; }
        public int Quantity { get; set; }

        public virtual Characters Character { get; set; }
    }
}
