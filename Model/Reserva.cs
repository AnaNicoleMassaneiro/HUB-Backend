using System;

namespace HubUfpr.Model
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public User Cliente { get; set; }
        public Produto Produto { get; set; }
        public string StatusReserva { get; set; }
        public DateTime DataCriacao { get; set; }
        public int QuantidadeDesejada { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}