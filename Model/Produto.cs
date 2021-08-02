using System;

namespace HubUfpr.Model
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public float Preco { get; set; }
        public string descricao { get; set; }
        public int qtdProdutosDisponiveis { get; set; }
    }
}