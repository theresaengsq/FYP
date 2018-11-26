using System;
using System.Collections.Generic;

namespace P06.Models
{
    public partial class Pokedex
    {
        public Pokedex()
        {
            CakeOrder = new HashSet<CakeOrder>();
            MugOrder = new HashSet<MugOrder>();
            ShirtOrder = new HashSet<ShirtOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CakeOrder> CakeOrder { get; set; }
        public ICollection<MugOrder> MugOrder { get; set; }
        public ICollection<ShirtOrder> ShirtOrder { get; set; }
    }
}
