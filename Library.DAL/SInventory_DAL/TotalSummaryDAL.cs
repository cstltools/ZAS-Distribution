using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class TotalSummaryDAL
    {
         ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();


        public DataTable LoadSummaryDAL(DateTime fromdate, DateTime todate)
        {
            string query = @"SELECT C.ComUnitCode,C.ComUnitId,C.ShortName,tblA.NumberofProformaInvoice,tblA.SumofNetProformaAmount,tblC.ComUnitId,
ISNULL(tblC.NumberofInvoiceSold,0)NumberofInvoiceSold,ISNULL(tblC.SumofNetSalesAmount,0)SumofNetSalesAmount,tblD.ComUnitId,
ISNULL(tblD.NumberofReturnInvoice,0)NumberofReturnInvoice,ISNULL(tblD.SumofNetReturnAmount,0)SumofNetReturnAmount 

,ISNULL(tblA.ProTpVat,0)ProTpVat 
,ISNULL(tblc.DelTpVat,0)DelTpVat 
,ISNULL(tblD.TotalPriceVatAmount,0)DelReTpVat 

FROM dbo.tblCompanyUnit C with(NoLock) LEFT JOIN (SELECT I.ComUnitId,count(DISTINCT I.InvoiceId) NumberofProformaInvoice,SUM(ID.NetAmount)-sum(ID.TotalPriceVatAmount)  as 
 SumofNetProformaAmount,SUM(ID.TotalPriceVatAmount)ProTpVat
FROM dbo.tblInvoice I WITH (NOLOCK) INNER JOIN dbo.tblInvoiceDetail ID WITH (NOLOCK) ON ID.InvoiceId = I.InvoiceId 
WHERE I.TpGrandTotal>0 AND I.InvoiceDate BETWEEN '" + fromdate + "' and '" + todate + "'  GROUP BY I.ComUnitId)tblA ON tblA.ComUnitId=C.ComUnitId LEFT JOIN   (SELECT ComUnitId,COUNT(DISTINCT tblInvoice.InvoiceId)NumberofInvoiceSold,SUM(D.DeliveryNetAmount - D.DeliveryTotalPriceVatAmount) SumofNetSalesAmount , SUM(D.DeliveryTotalPriceVatAmount)DelTpVat FROM dbo.tblInvoice  WITH (NOLOCK) INNER JOIN dbo.tblInvoiceDetail D WITH (NOLOCK) ON tblInvoice.InvoiceId = D.InvoiceId   WHERE DeliveryInvoiceStatus IN  ('Partial','Full') AND TpGrandTotal>0 AND UpdateDate BETWEEN '" + fromdate + "' and '" + todate + "'   GROUP BY  ComUnitId)tblc ON tblc.ComUnitId = C.ComUnitId LEFT JOIN (SELECT I.ComUnitId, ((SUM(ID.NetAmount)-sum(ID.TotalPriceVatAmount))- SUM(ID.DeliveryNetAmount - ID.DeliveryTotalPriceVatAmount)) AS SumofNetReturnAmount,COUNT(DISTINCT I.DelivaryInvoiceNo) NumberofReturnInvoice,SUM(ID.TotalPriceVatAmount)-sum(DeliveryTotalPriceVatAmount) as TotalPriceVatAmount FROM dbo.tblInvoice I WITH (NOLOCK) INNER JOIN dbo.tblInvoiceDetail ID WITH (NOLOCK) ON ID.InvoiceId = I.InvoiceId  where ID.DeliveryStatus IN ('Reject','Partial') AND I.UpdateDate BETWEEN '" + fromdate + "' and '" + todate + "' AND I.TpGrandTotal>0  GROUP BY I.ComUnitId)tblD ON tblD.ComUnitId = C.ComUnitId    ";
            
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        
        
    }
}


