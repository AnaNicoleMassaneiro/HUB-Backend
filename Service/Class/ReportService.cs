using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using HubUfpr.Service.Interface;
using System.Collections.Generic;

namespace HubUfpr.Service.Class
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public ReportHeader ReportHeader(string type, int idVendedor, string dateFilter)
        {
            return _reportRepository.ReportHeader(type, idVendedor, dateFilter);
        }

        public List<ReportData> ReportData(string type, int idVendedor, string dateFilter)
        {
            return _reportRepository.ReportData(type, idVendedor, dateFilter);
        }
    }
}
