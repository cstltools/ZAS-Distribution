using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class CustomerCoverageReportDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        
        public DataTable GetMiaWiseCustomerCoverageInfo(string fromDate, string toDate)
        {
            string query = @"select MiaCode,MiaName,sum(TotalCust)TotalCust,sum(TotalColl)TotalColl,sum(TotalExec)TotalExec,sum(TotalRej)TotalRej,SUM(TotalAmnt)TotalAmnt,SUM(CustCov)CustCov 
                            ,convert(decimal(18,2),(convert(decimal(18,2),(SUM(CustCov)*100))/sum(TotalCust)))Cov 
                             FROM
							 (SELECT EmpMasterCode as MIACode,EmpName AS MIAName,COUNT(*)TotalCust,'0'TotalColl,'0'TotalExec,'0'TotalRej,'0'TotalAmnt,'0'CustCov FROM View_CustomerMaster							 
							 LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = View_CustomerMaster.TerritoryId 
							 LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
							  group by EmpMasterCode,EmpName 
                             union all SELECT tblEmpGeneralInfo.EmpMasterCode AS MIACode,tblEmpGeneralInfo.EmpName AS MIAName,'0'TotalCust,count(*)TotalColl,'0'TotalExec,'0'TotalRej,'0'TotalAmnt,'0'CustCov FROM tblInvoice 
							 LEFT JOIN dbo.tblMIOInfo ON tblInvoice.MiaId = tblMIOInfo.MIOId
							 LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId
							 WHERE InvoiceDate between '" + fromDate + "' and '"+ toDate + "' group by tblEmpGeneralInfo.EmpMasterCode,tblEmpGeneralInfo.EmpName "
                             + "union all SELECT tblEmpGeneralInfo.EmpMasterCode AS MIACode,tblEmpGeneralInfo.EmpName AS MIAName,'0'TotalCust,'0'TotalColl,count(*)TotalExec,'0'TotalRej,'0'TotalAmnt,'0'CustCov FROM tblInvoice "
							 + "LEFT JOIN dbo.tblMIOInfo ON tblInvoice.MiaId = tblMIOInfo.MIOId "
							 + "LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId "
							 + " where DeliveryInvoiceStatus in ('Full','Partial') and  InvoiceDate between '" + fromDate + "' and '"+ toDate + "' group by tblEmpGeneralInfo.EmpMasterCode,tblEmpGeneralInfo.EmpName "
							 + "union all SELECT tblEmpGeneralInfo.EmpMasterCode AS MIACode,tblEmpGeneralInfo.EmpName AS MIAName,'0'TotalCust,'0'TotalColl,'0'TotalExec,count(*)TotalRej,'0'TotalAmnt,'0'CustCov FROM tblInvoice "
							 + "LEFT JOIN dbo.tblMIOInfo ON tblInvoice.MiaId = tblMIOInfo.MIOId "
							 + "LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId "
							 + " where DeliveryInvoiceStatus in ('Reject') and  InvoiceDate between '" + fromDate + "' and '"+ toDate + "' group by tblEmpGeneralInfo.EmpMasterCode,tblEmpGeneralInfo.EmpName "
                             + "union all SELECT tblEmpGeneralInfo.EmpMasterCode AS MIACode,tblEmpGeneralInfo.EmpName AS MIAName,'0'TotalCust,'0'TotalColl,'0'TotalExec,'0'TotalRej,sum(DeliveryTpGrandTotal)TotalAmnt,'0'CustCov FROM tblInvoice "
							 + "LEFT JOIN dbo.tblMIOInfo ON tblInvoice.MiaId = tblMIOInfo.MIOId "
							 + "LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId "
							 + "WHERE InvoiceDate between '" + fromDate + "' and '"+ toDate + "' group by tblEmpGeneralInfo.EmpMasterCode,tblEmpGeneralInfo.EmpName "
                             + "union all select MIACode,MIAName,'0'TotalCust,'0'TotalColl,'0'TotalExec,'0'TotalRej,'0'TotalAmnt,count(*)CustCov from(SELECT DISTINCT tblEmpGeneralInfo.EmpMasterCode AS MIACode,tblEmpGeneralInfo.EmpName AS MIAName,CustomerMasterId FROM tblInvoice "
							 + "LEFT JOIN dbo.tblMIOInfo ON tblInvoice.MiaId = tblMIOInfo.MIOId "
							 + "LEFT JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId "
							 + " where InvoiceDate between '" + fromDate + "' and '"+ toDate + "' )as tbltenp group by MIACode,MIAName)as tbltemp "
                             + "group by MiaCode,MiaName HAVING tbltemp.MIACode IS NOT NULL";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetDZSMWiseCustomerCoverageInfo(string fromDate, string toDate)
        {
            throw new NotImplementedException();
        }
    }
}
