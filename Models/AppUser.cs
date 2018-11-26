using System;
using System.Collections.Generic;

namespace P06.Models
{
    public partial class AppUser
    {
        public AppUser()
        {
            CakeOrder = new HashSet<CakeOrder>();
            MugOrder = new HashSet<MugOrder>();
            ShirtOrder = new HashSet<ShirtOrder>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public string Role { get; set; }
        public DateTime? LastLogin { get; set; }

        public ICollection<CakeOrder> CakeOrder { get; set; }
        public ICollection<MugOrder> MugOrder { get; set; }
        public ICollection<ShirtOrder> ShirtOrder { get; set; }
    }
}
