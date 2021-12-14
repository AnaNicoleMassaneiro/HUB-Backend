using HubUfpr.Model;
using System.Collections.Generic;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IReportRepository
    {
        ReportHeader ReportHeader(string type, int idVendedor, string dateFilter);

        List<ReportData> ReportData(string type, int idVendedor, string dateFilter);
    }
}
