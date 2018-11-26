using System;
using System.Collections.Generic;

namespace P06.Models
{
    public partial class CakeOrder
    {
        public int Id { get; set; }
        public string Greeting { get; set; }
        public int Qty { get; set; }
        public string Flavor { get; set; }
        public int PokedexId { get; set; }
        public string UserCode { get; set; }

        public Pokedex Pokedex { get; set; }
        public AppUser UserCodeNavigation { get; set; }
    }
}
