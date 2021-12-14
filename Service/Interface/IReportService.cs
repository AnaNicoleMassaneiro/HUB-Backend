using HubUfpr.Model;
using System.Collections.Generic;

namespace HubUfpr.Service.Interface
{
    public interface IReportService
    {
        ReportHeader ReportHeader(string type, int idVendedor, string dateFilter);

        List<ReportData> ReportData(string type, int idVendedor, string dateFilter);
    }
}
