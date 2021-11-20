using System;

namespace HubUfpr.Model
{
    public class Vendedor
    {
        public int IdVendedor { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
        public bool IsAtivo { get; set; }
        public bool IsOpen { get; set; }
    }
}
