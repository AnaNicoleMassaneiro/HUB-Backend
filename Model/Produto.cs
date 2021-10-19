using System;

namespace HubUfpr.Model
{
    public class Produto
    {
        public int Id { get; set; }

        public int idVendedor { get; set; }
        public Vendedor Vendedor { get; set; }
        public string Nome { get; set; }
        public bool IsAtivo { get; set; }
        public float Preco { get; set; }
        public float NotaProduto { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public int QuantidadeDisponivel { get; set; }
    }
}