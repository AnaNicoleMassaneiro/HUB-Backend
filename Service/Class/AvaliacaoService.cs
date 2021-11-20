using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using System.Collections.Generic;

namespace HubUfpr.Service.Class
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository)
        {
            _avaliacaoRepository = avaliacaoRepository;
        }

        public List<Avaliacao> GetAvaliacao(int idProduto, int idVendedor, int idCliente)
        {
            return _avaliacaoRepository.GetAvaliacao(idProduto, idVendedor, idCliente);
        }

        public void InsertAvaliacao(int tipoAvaliacao, int idCliente, int idVendedor, int idProduto, int nota, string titulo, string descricao)
        {
            _avaliacaoRepository.InsertAvaliacao(tipoAvaliacao, idCliente, idVendedor, idProduto, nota, titulo, descricao);
        }
    }
}
