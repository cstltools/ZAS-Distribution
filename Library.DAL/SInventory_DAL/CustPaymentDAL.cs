using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class CustPaymentDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();



        private DB_Manager aManager = new DB_Manager();
        public bool SaveCustPayment(CustPayment aCustPayment)
        {
            string insertQuery = @"INSERT INTO dbo.tblCustomerPay
        ( CustPayId ,
          
          CustomerMasterId ,
            
          PaymentDate ,
          PaymentAmount ,
          PayType ,
          RefNo ,
          RefDate ,
          CreateBy ,
          CreateDate 
          
        )

            values (" + aCustPayment.CustPayId + "," +
                   
                    "" + aCustPayment.CustomerMasterId + "," +
                 
                    "'" + aCustPayment.PaymentDate + "'," +
                    "'" + aCustPayment.PaymentAmount + "'," +
                    "'" + aCustPayment.PayType + "'," +
                    "'" + aCustPayment.RefNo + "'," +
                    "'" + (object)(aCustPayment.RefDate ?? (object)DBNull.Value) + "'," +
                    "'" + aCustPayment.CreateBy + "'," +
                    "'" + aCustPayment.CreateDate + "'" +
                    //"'" + aCustPayment.UpdateBy + "'," +
                    //"'" + aCustPayment.UpdateDate + "'" +
                                 
                    ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public int SaveFullInvoice(CustPayment aCustPayment)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@CustPayId", aCustPayment.CustPayId));
            aSqlParameterList.Add(new SqlParameter("@CustomerMasterId", aCustPayment.CustomerMasterId));
            aSqlParameterList.Add(new SqlParameter("@PaymentDate", aCustPayment.PaymentDate));
            aSqlParameterList.Add(new SqlParameter("@PaymentAmount", aCustPayment.PaymentAmount));
            aSqlParameterList.Add(new SqlParameter("@PayType", aCustPayment.PayType));
            aSqlParameterList.Add(new SqlParameter("@RefNo", aCustPayment.RefNo));
            aSqlParameterList.Add(new SqlParameter("@RefDate", aCustPayment.RefDate));
            aSqlParameterList.Add(new SqlParameter("@CreateBy", aCustPayment.CreateBy));
            return aCommonInternalDal.RunStoreProcedure("sp_Save_SaveCustPayment", aSqlParameterList, "SSIDB");
        }


        public int SaveCode()
        {
            int id = 0;
            try
            {
                aManager.CreateConnection("SalesRollDB_ZAS");
                List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

                aSqlParameterList.Add(new SqlParameter("@CreateBy", HttpContext.Current.Session["UserId"].ToString()));
                id = aManager.SaveAction("sp_Save_PayCode", aSqlParameterList, "@PayCodeId");

                return aCommonInternalDal.SaveAction("sp_Save_SaveCustPayment", aSqlParameterList, "@PayCodeId");
            }
            catch (Exception)
            {
                aManager.CloseConnection();
            }
            finally
            {
                aManager.CloseConnection();
            }

            return id;
        }



        public bool SaveCustDetail(CustPaymentDetail aCustPaymentDetail)
        {



            string insertQuery = @"INSERT INTO dbo.tblCustPayDetail
        ( CustPayDetailId ,
          InvoiceId ,
          PaymentAmount ,
          CustPayId,
          BankAccId,
          CashAccId,
          TransctionDetailId,
          ProcessTime, 
          AIT, Discount,
          PaymentRemarks 
        )
            values (" + aCustPaymentDetail.CustPayDetailId+ "," +
                   "'" + aCustPaymentDetail.InvoiceId + "'," +
                    "" + aCustPaymentDetail.PaymentAmount + "," +
                    "" + aCustPaymentDetail.CustPayId + "," +
                    "" + aCustPaymentDetail.BankAccId + "," +
                    "" + aCustPaymentDetail.CashAccId + "," +
                    "" + aCustPaymentDetail.TransctionDetailId + "," +
                    "'" + aCustPaymentDetail.ProcessTime + "'," +
                    "'" + aCustPaymentDetail.AIT + "'," +
                    "'" + aCustPaymentDetail.Discount + "'," +
                    "'" + aCustPaymentDetail.Remarks + "')";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }
        public void LoadSC(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyUnit";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDC(DropDownList ddl, string companyId)
        {
            string queryStr = "select * from tblCompanyUnit where CompanyId='" + companyId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }

        public void LoadDcWiseAreaAloByDc(DropDownList ddl,string DCId)
        {
            string query = @"SELECT DCRA.AreaId, AR.AreaShortName + ' : ' + AR.AreaName AS AreaName FROM dbo.tblDcWiseAreaInfo AS DCRA
                            INNER JOIN tblArea AS AR ON DCRA.AreaId = AR.AreaId
                            WHERE DCRA.IsActive =  'True' AND DCRA.DCId = '" + DCId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AreaName", "AreaId", query);
        }


        public void GetCompanyDropdownList(DropDownList ddl)
        {
            const string queryStr = @"SELECT CompanyId, CmpShortName + ' : ' + CompanyName AS CompanyName FROM tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
        }

        public void LoadSC(DropDownList ddl, string userId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            //            string queryStr = @"SELECT * FROM dbo.tblCompanyUnit
            //                                LEFT JOIN dbo.tblUserCompanyUnit ON dbo.tblCompanyUnit.ComUnitId=dbo.tblUserCompanyUnit.CompanyUnitId
            //                                WHERE UserId='"+userId+"'";


            string queryStr = "select ComUnitId, ComUnitName  from tblCompanyUnit WHERE " +
                               " ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId='" + userId.Trim() + "')";


            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ComUnitName", "ComUnitId", queryStr);
        }
        public void LoadManufac(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblManufacturer";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ManufacName", "ManufacId", queryStr);
        }
        public bool UpdateInvoicePaymentAmount(string amount,string status,string id)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvoiceId", id));

            return aCommonInternalDal.UpdateAction("sp_UD_InvoicePayment", aSqlParameterlist);


            //string query = @"UPDATE dbo.tblInvoice SET PaymentAmount='" + amount + "',PaymentStatus='"+status+"' WHERE InvoiceId='" + id + "'";
            //return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        
        public void LoadCustomerMaster(DropDownList ddl, string marketId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCustMaster WHERE CustomerMasterId IN (SELECT DISTINCT CustomerMasterId FROM dbo.View_CustomerMaster WHERE MarketId='" + marketId + "')";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }
        public void PaymentTypeLoad(DropDownList aDropDownList)
        {
            string query = @"select * from tblPaymentType";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "PaymentTypeName", "PaymentTypeId", query, "SSIDB");
        }

        public void LoadMarket(DropDownList ddl, string comunitId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT DISTINCT CM.MarketId,CM.MarketCode,CM.MarketName FROM dbo.tblCompanyUnit Cu
                inner JOIN dbo.View_CustomerMaster CM ON CM.ComUnitCode = Cu.ComUnitCode
                inner JOIN dbo.tblMarket ON tblMarket.MarketCode = CM.MarketCode
                WHERE CU.ComUnitId='" + comunitId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }

        public void LoadCustomer(DropDownList ddl, string batchId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT DISTINCT tblCustMaster.CustomerMasterId,CustomerName FROM dbo.tblInvoice
INNER JOIN dbo.tblCustMaster ON tblCustMaster.CustomerMasterId = tblInvoice.CustomerMasterId
WHERE BatchId='"+batchId+"' AND DeliveryTpGrandTotal > 0 AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }
        public DataTable LoadInvoiceNew(string batchId, string customerId)
        {
//            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
//            INNER JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
//            WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId + "' AND dbo.tblInvoice.CustomerMasterId='" + customerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL ";

            string query = @"SELECT PaymentTypeName,CustomerCode,ISNULL(PaymentAmount,0)PaymentAmount,*,(ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) AS Due,
tblCompanyInfo.CompanyId,tblCustMaster.CustomerMasterId FROM dbo.tblInvoice   
LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount) AS PP,SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(Discount,0)) DiscountAmount FROM tblCustPayDetail GROUP BY InvoiceId) AS P ON tblInvoice.InvoiceId = P.InvoiceId 
LEFT JOIN (SELECT InvoiceId,SUM(DeliveryNetAmount) AS TotalDelivery FROM tblInvoiceDetail AS IVD WITH(NOLOCK) GROUP BY InvoiceId) AS TD ON tblInvoice.InvoiceId = TD.InvoiceId 
LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
LEFT JOIN tblPaymentType ON tblInvoice.PaymentTypeId = tblPaymentType.PaymentTypeId
WHERE (ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) > 0 AND BatchId ='" + batchId
                             + "' AND DelivaryInvoiceNo IS NOT NULL AND tblInvoice.InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (DeliveryTpGrandTotal-isnull(PaymentAmount,0)) > 0";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

        }
        public DataTable LoadInvoice(string comUnitId,string customerId,string marketId)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice
            
            inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
            WHERE  DeliveryTpGrandTotal > 0 AND MarketId='" + marketId + "' AND dbo.tblInvoice.ComUnitId='" + comUnitId + "' AND dbo.tblInvoice.CustomerMasterId='" + customerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable LoadInvoice(string comUnitId, string marketId)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice
            
            inner JOIN dbo.View_CustomerMaster ON dbo.tblInvoice.CustomerMasterId=dbo.View_CustomerMaster.CustomerMasterId
            WHERE  DeliveryTpGrandTotal > 0 AND MarketId='" + marketId + "' AND dbo.tblInvoice.ComUnitId='" + comUnitId + "'  AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND (DelivaryInvoiceNo IS NOT NULL) AND (DeliveryInvoiceStatus ='Partial' or DeliveryInvoiceStatus ='Full')";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadCoustomerPaymentMaster(string param)
        {
            string query = @"Select CustPayId,PaymentDate,PaymentCode from tblCustomerPay WHere PaymentCode IS NOT NULL "+param+" ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable GetPrevAmount(string invoiceId)
        {
            string query = @"SELECT PaymentAmount, 
         FROM dbo.tblInvoice WHERE InvoiceId='" + invoiceId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetCustomerInfo(string custId)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster WHERE CustomerMasterId='"+custId+"'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustPayMaster(string custId)
        {
            string query = @"Select  com.ComUnitName,Cus.CustomerName,I.InvoiceNo, M.PaymentDate,P.AIT,P.PaymentAmount, P.Discount, M.PaymentCode as Blank1 ,'' as Blank2, '' as Blank3 from  tblCustomerPay M
              left join  tblCustPayDetail P ON M.CustPayId = P.CustPayId
              LEFt JOIN tblInvoice I On P.InvoiceId= I.InvoiceId
              LEFT join tblCustMaster Cus ON I.CustomerMasterId= Cus.CustomerMasterId
              left join tblCompanyUnit com On I.ComUnitId= com.ComUnitId
              Where M.CustPayId='" + custId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public CustomerMaster CustomerLoad(string CustomerCode)
        {
            string query = @"SELECT CustomerMasterId FROM dbo.tblCustMaster WHERE CustomerCode= '" + CustomerCode + "'";
                             
            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CustomerMaster aCustomerMaster = new CustomerMaster();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCustomerMaster.CustomerMasterId = Int32.Parse(dataReader["CustomerMasterId"].ToString());
                }
            }
            return aCustomerMaster;
        }
        public CustomerMaster DetailCustomerLoad(string CustomerCode)
        {
            string query = @"SELECT * FROM dbo.tblCustMaster WHERE CustomerCode= '" + CustomerCode + "'";

            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            CustomerMaster aCustomerMaster = new CustomerMaster();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aCustomerMaster.CustomerMasterId = Int32.Parse(dataReader["CustomerMasterId"].ToString());
                    aCustomerMaster.CustomerCode = (dataReader["CustomerCode"].ToString());
                    aCustomerMaster.CustomerName = (dataReader["CustomerName"].ToString());
                }
            }
            return aCustomerMaster;
        }
        public Product DetailProductLoad(string ProductCode)
        {
            string query = @"SELECT * FROM dbo.tblProduct WHERE ProductCode= '" + ProductCode + "'";

            IDataReader dataReader = aCommonInternalDal.DataContainerDataReader(query, "SSIDB");
            Product aProduct = new Product();
            if (dataReader != null)
            {
                while (dataReader.Read())
                {
                    aProduct.ProductId = Int32.Parse(dataReader["ProductId"].ToString());
                    aProduct.ProductCode = (dataReader["ProductCode"].ToString());
                    aProduct.ProductName = (dataReader["ProductName"].ToString());
                }
            }
            return aProduct;
        }


        public void GetTerritoryOnDropdownList(DropDownList ddl, string areaId)
        {
            string queryStr = @"SELECT distinct TTR.TerritoryId, TTR.TerritoryCode + ' : ' + TTR.TerritoryName AS TerritoryName FROM tblTerritory AS TTR 
                                WHERE TTR.IsActive ='True' AND TTR.AreaId = '" + areaId + "'";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "TerritoryName", "TerritoryId", queryStr);
        }

        public void GetMarketNameByTerritoryId(DropDownList ddl, string terrId)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT MKT.MarketId ,MKT.MarketCode + ' : ' + MKT.MarketName AS MarketName FROM dbo.tblMarket AS MKT WHERE MKT.IsActive ='True' AND TerritoryId = '" + terrId + "'";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "MarketName", "MarketId", queryStr);
        }

        public DataTable CustomerPaymentDueNew(string parameter)
        {
            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
            LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            WHERE  DeliveryTpGrandTotal > 0 AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL " + parameter;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU 
            WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDal.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");


        }

        public void GetCustomerInfoLoad(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = @"SELECT CSTMR.CustomerMasterId,CAST(CSTMR.CustomerCode AS NVARCHAR(50)) + ':' + CSTMR.CustomerName + ' ( Region: ' + ISNULL(RGN.RegionName, '')   + '; Area: ' +  ISNULL(AR.AreaName, '')  + '; Territory:' + ISNULL(TTR.TerritoryName, '')  + '; Market: ' + ISNULL(MKT.MarketName, '')  + ' )' AS CustomerName FROM tblCustMaster AS CSTMR 
                                    INNER JOIN tblMarket AS MKT ON CSTMR.MarketId = MKT.MarketId
                                    INNER JOIN tblTerritory AS TTR On MKT.TerritoryId = TTR.TerritoryId
                                    INNER JOIN tblArea AS AR On TTR.AreaId = AR.AreaId
                                    INNER JOIN tblRegion AS RGN ON AR.RegionId = RGN.RegionId";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CustomerName", "CustomerMasterId", queryStr);
        }

        public DataTable GetInvoiceByCustomer(string custmerId,string dcId)
        {
//            string query = @"SELECT tblPaymentType.PaymentTypeName,ISNULL(PaymentAmount,0)PaymentAmount,*,(ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) AS Due FROM dbo.tblInvoice  
//
//LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount) AS PP,SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(Discount,0)) DiscountAmount FROM tblCustPayDetail GROUP BY InvoiceId) AS P ON tblInvoice.InvoiceId = P.InvoiceId 
//LEFT JOIN (SELECT InvoiceId,SUM(DeliveryNetAmount) AS TotalDelivery FROM tblInvoiceDetail AS IVD WITH(NOLOCK) GROUP BY InvoiceId) AS TD ON tblInvoice.InvoiceId = TD.InvoiceId 
//            LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
//            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
//            LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
//            LEFT JOIN tblPaymentType ON tblPaymentType.PaymentTypeId = tblInvoice.PaymentTypeId
//            WHERE  ISNULL(TotalDelivery,0) > 0 AND tblCustMaster.CustomerMasterId='" + custmerId
//            + "'  AND DelivaryInvoiceNo IS NOT NULL AND  tblInvoice.InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) >= 1 ";


            string query = @"SELECT INV.InvoiceId,INV.CustomerMasterId,CustomerCode,CI.CompanyId,INV.ComUnitId,
            TermOfPayment PaymentTypeName,ComUnitName,InvoiceNo,InvoiceDate,DelivaryInvoiceNo,INV.UpdateDate,DeliveryTpGrandTotal,
            (ISNULL(INV.PaymentAmount,0) + ISNULL(INV.AIT,0) + ISNULL(INV.DiscountOnPayment,0)) PaymentAmount,
            (ISNULL(INV.DeliveryTpGrandTotal, 0) - (ISNULL(INV.PaymentAmount, 0) + ISNULL(INV.AIT, 0) + ISNULL(INV.DiscountOnPayment, 0))) - ISNULL(RTN.ReturnAmount,0) AS Due FROM tblInvoice AS INV
            LEFT JOIN (SELECT M.CommonPK_ID,SUM(DebitAmountBDT) AS ReturnAmount FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
            			LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
            			LEFT JOIN tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
            			WHERE ISNULL(M.IsInventoryValuation,0) = 0 AND M.TableID = 2018 GROUP BY M.CommonPK_ID) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
            
            
            LEFT JOIN tblCustMaster AS CST ON INV.CustomerMasterId = CST.CustomerMasterId
            LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
            INNER JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId
            
            WHERE INV.CustomerMasterId = '" + custmerId + "' AND   INV.InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (ISNULL(INV.DeliveryTpGrandTotal, 0) - (ISNULL(INV.PaymentAmount, 0) + ISNULL(INV.AIT, 0) + ISNULL(INV.DiscountOnPayment, 0))) - ISNULL(RTN.ReturnAmount,0) >= 1";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");  
        }


        public DataTable GetInvoiceByCustomer(string custmerId, string userId, string dcId)
        {
//            string query = @"SELECT tblPaymentType.PaymentTypeName,ISNULL(PaymentAmount,0)PaymentAmount,*,(ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) AS Due FROM dbo.tblInvoice  
//
//LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount) AS PP,SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(Discount,0)) DiscountAmount FROM tblCustPayDetail GROUP BY InvoiceId) AS P ON tblInvoice.InvoiceId = P.InvoiceId 
//LEFT JOIN (SELECT InvoiceId,SUM(DeliveryNetAmount) AS TotalDelivery FROM tblInvoiceDetail AS IVD WITH(NOLOCK) GROUP BY InvoiceId) AS TD ON tblInvoice.InvoiceId = TD.InvoiceId 
//            LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
//            LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
//            LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
//            LEFT JOIN tblPaymentType ON tblPaymentType.PaymentTypeId = tblInvoice.PaymentTypeId
//            WHERE  ISNULL(TotalDelivery,0) > 0 AND tblCustMaster.CustomerMasterId='" + custmerId + "' AND DelivaryInvoiceNo IS NOT NULL AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId
//            + " ) AND tblInvoice.InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0))) >= 1 ";



            string query = @"SELECT INV.InvoiceId,INV.CustomerMasterId,CustomerCode,CI.CompanyId,INV.ComUnitId,
TermOfPayment PaymentTypeName,ComUnitName,InvoiceNo,InvoiceDate,DelivaryInvoiceNo,INV.UpdateDate,DeliveryTpGrandTotal,
(ISNULL(INV.PaymentAmount,0) + ISNULL(INV.AIT,0) + ISNULL(INV.DiscountOnPayment,0)) PaymentAmount,
(ISNULL(DeliveryTpGrandTotal, 0) - (ISNULL(INV.PaymentAmount,0) + ISNULL(INV.AIT,0) + ISNULL(INV.DiscountOnPayment,0))) - ISNULL(RTN.ReturnAmount,0) AS Due FROM tblInvoice AS INV
LEFT JOIN (SELECT M.CommonPK_ID,SUM(DebitAmountBDT) AS ReturnAmount FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
			LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
			LEFT JOIN tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
			WHERE ISNULL(M.IsInventoryValuation,0) = 0 AND M.TableID = 2018 GROUP BY M.CommonPK_ID) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID

LEFT JOIN tblCustMaster AS CST ON INV.CustomerMasterId = CST.CustomerMasterId
LEFT JOIN tblCompanyUnit AS UNT ON INV.ComUnitId = UNT.ComUnitId
INNER JOIN tblCompanyInfo AS CI ON UNT.CompanyId = CI.CompanyId

WHERE INV.CustomerMasterId = '" + custmerId + "'   AND INV.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + " )  AND INV.InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (ISNULL(DeliveryTpGrandTotal, 0) - (ISNULL(INV.PaymentAmount,0) + ISNULL(INV.AIT,0) + ISNULL(INV.DiscountOnPayment,0))) - ISNULL(RTN.ReturnAmount,0) >= 1";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCustomerInfoByCode(string customerCode)
        {
            string query = @"SELECT INV.CustomerMasterId FROM dbo.tblInvoice AS INV 
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON INV.CustomerMasterId = CSTMR.CustomerMasterId
                             WHERE CSTMR.CustomerCode ='" + customerCode + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetCreditBalanceInfo(string customerId)
        {
            string query = @"SELECT CDJ.CustomerMasterId,SUM(CDJ.Amount) AS Balance FROM tblCreditAdjustment AS CDJ
                             WHERE CDJ.CustomerMasterId = '" + customerId + "' GROUP BY CDJ.CustomerMasterId";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable LoadInvoiceNewByUser(string batchId, string customerId, string userId)
        {
            //            string query = @"SELECT ISNULL(PaymentAmount,0)PaymentAmount,*,(DeliveryTpGrandTotal-isnull(PaymentAmount,0)) AS Due FROM dbo.tblInvoice           
            //            INNER JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
            //            WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId + "' AND dbo.tblInvoice.CustomerMasterId='" + customerId + "' AND (PaymentStatus IS NULL OR PaymentStatus='Partial') AND DelivaryInvoiceNo IS NOT NULL ";

            string query = @"SELECT tblPaymentType.PaymentTypeName,ISNULL(PaymentAmount,0)PaymentAmount,*,(ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0)))  - ISNULL(RTN.ReturnAmount,0)  - ISNULL(RTN.ReturnAmount,0) AS Due,
                             tblCompanyInfo.CompanyId,tblCustMaster.CustomerMasterId FROM dbo.tblInvoice    


							 LEFT JOIN (SELECT M.CommonPK_ID,SUM(DebitAmountBDT) AS ReturnAmount FROM CSTL_ACCDB..tblDebitCreditVoucherMaster AS M
			LEFT JOIN CSTL_ACCDB..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
			LEFT JOIN tblInvoice AS INV ON M.CommonPK_ID = INV.InvoiceId
			WHERE ISNULL(M.IsInventoryValuation,0) = 0 AND M.TableID = 2018 GROUP BY M.CommonPK_ID) AS RTN ON InvoiceId = RTN.CommonPK_ID    

LEFT JOIN (SELECT InvoiceId,SUM(PaymentAmount) AS PP,SUM(ISNULL(AIT,0)) AIT, SUM(ISNULL(Discount,0)) DiscountAmount FROM tblCustPayDetail GROUP BY InvoiceId) AS P ON tblInvoice.InvoiceId = P.InvoiceId 
LEFT JOIN (SELECT InvoiceId,SUM(DeliveryNetAmount) AS TotalDelivery FROM tblInvoiceDetail AS IVD WITH(NOLOCK) GROUP BY InvoiceId) AS TD ON tblInvoice.InvoiceId = TD.InvoiceId 
   
                             LEFT JOIN dbo.tblCustMaster  ON dbo.tblInvoice.CustomerMasterId=dbo.tblCustMaster.CustomerMasterId
                             LEFT JOIN dbo.tblCompanyUnit ON tblCompanyUnit.ComUnitId = tblInvoice.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo ON tblCompanyInfo.CompanyId = tblCompanyUnit.CompanyId
                             LEFT JOIN tblPaymentType ON tblPaymentType.PaymentTypeId = tblInvoice.PaymentTypeId
                             WHERE  DeliveryTpGrandTotal > 0 AND BatchId='" + batchId + "'  AND DelivaryInvoiceNo IS NOT NULL AND tblInvoice.ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId
                             + ") AND tblInvoice.InvoiceId NOT IN  ( SELECT InvoiceId FROM dbo.tblPaymentTranscationMaster INNER JOIN dbo.tblPaymentTranscationDetail ON tblPaymentTranscationDetail.CustPayId = tblPaymentTranscationMaster.CustPayId WHERE IsProcess = 0 AND IsReject = 0) AND (ISNULL(TotalDelivery,0) - (ISNULL(PP,0) + ISNULL(P.AIT,0) + ISNULL(P.DiscountAmount,0)))  - ISNULL(RTN.ReturnAmount,0)  - ISNULL(RTN.ReturnAmount,0) >= 1"; 

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadBankAccount(DropDownList ddl, string companyId)
        {
//            string query = @"SELECT CB.BankAccountId,BankName + ' (' + AccountName + ' - ' + AccountNo + ')' AS BankAcount FROM ZAS_ACCDB..tblBankAccount AS CB
//                             LEFT JOIN ZAS_ACCDB..tblBankInformation AS B ON b.BankId = CB.BankId
//                             LEFT JOIN ZAS_ACCDB..tblMultipleCompanySameBankAccount AS CABC ON CABC.BankAccountId = CB.BankAccountId
//                             WHERE CB.BankAccountId IN (
//                             SELECT BankAccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE BankAccountId IS NOT NULL )
//                             AND CABC.CompanyId = " + companyId;

            string query = @"SELECT CB.BankAccountId,BankName + ' (' + AccountName + ' - ' + AccountNo + ')' AS BankAcount,CABC.CompanyUnitId FROM CSTL_ACCDB..tblBankAccount AS CB
                             LEFT JOIN CSTL_ACCDB..tblBankInformation AS B ON b.BankId = CB.BankId
                             LEFT JOIN CSTL_ACCDB..tblBankAccountUnitdtl AS CABC ON CABC.BankAccountId = CB.BankAccountId
                             WHERE CB.BankAccountId IN (
                             SELECT BankAccountId FROM CSTL_ACCDB..tblChartOfAccounts WHERE BankAccountId IS NOT NULL) AND CABC.CompanyUnitId = " + companyId;

            aCommonInternalDal.LoadDropDownValue(ddl, "BankAcount", "BankAccountId", query, "SSIDB");
        }


        public void LoadCashAccount(DropDownList ddl, string companyId)
        {
//            string query = @"SELECT CA.CashAccountId,AccountName FROM ZAS_ACCDB..tblCashAccount AS CA
//                             LEFT JOIN ZAS_ACCDB..tblMultipleCompanySameCashAccount CASC ON CA.CashAccountId = CASC.CashAccountId
//                             WHERE CA.CashAccountId IN (
//                             SELECT CashAccountId FROM ZAS_ACCDB..tblChartOfAccounts WHERE CashAccountId IS NOT NULL )
//                             AND CASC.CompanyId = " + companyId;

            string query = @"SELECT CA.CashAccountId,AccountName FROM CSTL_ACCDB..tblCashAccount AS CA
                             LEFT JOIN CSTL_ACCDB..tblMultipleCompanySameCashAccount CASC ON CA.CashAccountId = CASC.CashAccountId
                             WHERE CA.CashAccountId IN (
                             SELECT CashAccountId FROM CSTL_ACCDB..tblChartOfAccounts WHERE CashAccountId IS NOT NULL )
                             AND CASC.CompanyId = " + companyId;

            aCommonInternalDal.LoadDropDownValue(ddl, "AccountName", "CashAccountId", query, "SSIDB");
        }


        public bool PaymentJournalPosting(int custPayId, int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@CustPayId", custPayId));
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            return aCommonInternalDal.UpdateAction("sp_Sales_CustomerPaymentPosting", aSqlParameterList);
            //return aCommonInternalDal.UpdateAction("sp_CustomerPaymentPosting", aSqlParameterList);
        }

        //private bool DeletePaymentMasterInfo(CustPayment aCustPayment)
        //{
        //    string query = @"DELETE FROM tblCustomerPay WHERE CustPayId =  " + aCustPayment.CustPayId;
        //    return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        //}
        public bool DeletePaymentMasterInfo(CustPayment aCustPayment)
        {
            string query = @"DELETE FROM tblCustomerPay WHERE CustPayId =  " + aCustPayment.CustPayId;
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable GetInvoiceExistOrNot(int invoiceId)
        {
            string query = @"SELECT * FROM tblCustPayDetail AS PD
                             LEFT JOIN tblCustomerPay AS CP ON PD.CustPayId = CP.CustPayId
                             WHERE DATEDIFF(MINUTE, ProcessTime, GETDATE()) <= 5 AND InvoiceId IS NOT NULL AND PaymentDate BETWEEN '" + DateTime.Today + "' AND '" + DateTime.Today + "' AND InvoiceId = " + invoiceId;

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable CheckCustomerPaymentExistOrNot(CustPaymentDetail aCustPayment)
        {
            string query = @"SELECT CONVERT(DATE,PaymentDate) FROM dbo.tblCustPayDetail AS PD
            LEFT JOIN tblCustomerPay AS PM ON PD.CustPayId = PM.CustPayId
            WHERE CONVERT(DATE,PaymentDate) = CONVERT(DATE,GETDATE()) AND PD.InvoiceId='" + aCustPayment.InvoiceId + "' AND PD.PaymentAmount='" + aCustPayment.PaymentAmount + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public void LoadInvoiceForPaymentDelete(DropDownList ddl, string comUnitId, string marketId)
        {
            string query = @"SELECT DISTINCT INV.InvoiceId,
                             CST.CustomerCode + ' : ' + CST.CustomerName + ' [' + ISNULL(INV.DelivaryInvoiceNo, INV.InvoiceNo) + ']' AS InvoiceText
                             FROM dbo.tblInvoice AS INV
                             INNER JOIN dbo.tblCustMaster AS CST ON INV.CustomerMasterId = CST.CustomerMasterId
                             WHERE INV.ComUnitId = '" + comUnitId + "' AND CST.MarketId = '" + marketId + @"'
                             AND EXISTS (SELECT 1 FROM dbo.tblCustPayDetail AS PD WHERE PD.InvoiceId = INV.InvoiceId)";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "InvoiceText", "InvoiceId", query);
        }

        public DataTable GetPaymentDetailListByInvoiceId(string invoiceId)
        {
            string query = @"SELECT PD.CustPayDetailId, PD.CustPayId,
                             ISNULL(INV.DelivaryInvoiceNo, INV.InvoiceNo) AS InvoiceNo,
                             PD.PaymentAmount, CP.PaymentDate, PD.PaymentRemarks AS Remarks
                             FROM dbo.tblCustPayDetail AS PD
                             INNER JOIN dbo.tblCustomerPay AS CP ON PD.CustPayId = CP.CustPayId
                             INNER JOIN dbo.tblInvoice AS INV ON PD.InvoiceId = INV.InvoiceId
                             WHERE PD.InvoiceId = '" + invoiceId + @"'
                             ORDER BY CP.PaymentDate";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetPaymentDetailCountByCustPayId(string custPayId)
        {
            string query = @"SELECT COUNT(*) AS Cnt FROM dbo.tblCustPayDetail WHERE CustPayId = '" + custPayId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
