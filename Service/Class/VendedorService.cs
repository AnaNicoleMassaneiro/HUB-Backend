using System;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using HubUfpr.Data.DapperORM.Interface;
using System.Collections.Generic;

namespace HubUfpr.Service.Class
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IVendedorRepository vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public Vendedor getVendedorById(int id)
        {
            return _vendedorRepository.getVendedorById(id);
        }

        public List<Vendedor> getVendedoresByLocation(float lat, float lon)
        {
            return _vendedorRepository.getVendedoresByLocation(lat, lon);
        }

        public List<Vendedor> getVendedoresByName(string name)
        {
            return _vendedorRepository.getVendedoresByName(name);
        }
    }
}
