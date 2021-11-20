using System;

namespace HubUfpr.Model
{
    public class Avaliacao
    {
        public int IdAvaliacao { get; set; }
        public int TipoAvaliacao { get; set; }
        public Cliente Cliente { get; set; }
        public Vendedor Vendedor { get; set; }
        public Produto Produto { get; set; }
        public int Nota { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}