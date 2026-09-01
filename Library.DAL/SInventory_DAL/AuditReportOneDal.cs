using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class AuditReportOneDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void GetSalesCenter(DropDownList ddl)
        {
            string queryStr = "SELECT DISTINCT ComUnitId,ComUnitCode,ComUnitName +':'+ ComUnitCode as ComUnitName FROM dbo.tblCompanyUnit  WHERE ComUnitId IN (SELECT ComUnitId FROM dbo.tblCompanyUnit) ";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitCode", queryStr);
        }

        public void GetCustomer(DropDownList ddl)
        {
            string queryStr = "SELECT CustomerMasterId ,CustomerCode FROM dbo.tblCustMaster";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerCode", "CustomerMasterId", queryStr); ;
        }

        public DataTable GetDeleteOrderReport(string parameter)
        {
            string queryStr = @"SELECT ODDtls.ProductCode, ODDtls.ProductName, ODDtls.Quantity, ODDtls.TradePrice, ODDtls.TotalTradePrice, OD.OrderCode, OD.ComUnitCode AS SalesCenterCode, OD.ComUnitName AS SalesCenterName, OD.MIOCode, OD.MIOName,
 VC.AreaName AS TeritoryName,VC.AreaCode AS TeritoryCode ,VC.AreaCode AS FECode, VC.AreaName AS FEName,
VC.RegionCode AS DZSMCode, VC.RegionName AS DZSMName,OD.CustomerCode, OD.CustomerName,OD.GrossValue,
 OD.SubmissionDate, OD.DeleteBy, OD.DeleteDate FROM tblOrderDel AS OD WITH(NOLOCK)
 INNER JOIN  dbo.View_CustomerMaster AS VC ON VC.CustomerCode = OD.CustomerCode
 INNER JOIN  dbo.tblOrderDetailDel  AS ODDtls ON ODDtls.OrderId = OD.OrderId " + parameter;
            return aInternalDal.DataContainerDataTable(queryStr);
        }

        public DataTable GetDeleteOrderNationalReport(string parameter)
        {
            string queryStr = @"SELECT  ODDtls.ProductCode, ODDtls.ProductName, ODDtls.Quantity, ODDtls.TradePrice, ODDtls.TotalTradePrice, OD.OrderCode, OD.ComUnitCode AS SalesCenterCode, OD.ComUnitName AS SalesCenterName, OD.MIOCode, OD.MIOName, VC.AreaName AS TeritoryName,
VC.AreaCode AS TeritoryCode ,VC.AreaCode AS FECode, VC.AreaName AS FEName, VC.RegionCode AS DZSMCode, VC.RegionName AS DZSMName,OD.CustomerCode,
 OD.CustomerName,OD.GrossValue, OD.SubmissionDate, OD.DeleteBy, OD.DeleteDate FROM tblOrderDel AS OD WITH(NOLOCK)
 INNER JOIN  dbo.View_CustomerMaster AS VC ON VC.CustomerCode = OD.CustomerCode 
  INNER JOIN  dbo.tblOrderDetailDel  AS ODDtls ON ODDtls.OrderId = OD.OrderId " + parameter;
            return aInternalDal.DataContainerDataTable(queryStr);
        }
    }
}
