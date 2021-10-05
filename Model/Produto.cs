using System;

namespace HubUfpr.Model
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        public float Preco { get; set; }
        public string descricao { get; set; }
        public int quantidadeDisponivel { get; set; }
    }
}