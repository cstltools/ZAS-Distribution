using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class DestroyProductDal
    {

        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        DB_Manager aDbManager = new DB_Manager();
        ClsApprovalAction approvalAction = new ClsApprovalAction();


        //Destroy Report
        ClsCommonInternalDAL aCommonInternalDAL = new ClsCommonInternalDAL();

        public void LoadGroupInfo(DropDownList groupDropDownList)
        {
            string query = @"SELECT GroupId, GroupName FROM tblGroupInformation";
            aCommonInternalDAL.LoadDropDownValue(groupDropDownList, "GroupName", "GroupId", query, "SSIDB");
        }

        public void LoadCompanyInfo(DropDownList ddl, string groupId)
        {
            string query = @"SELECT CompanyId, CompanyName FROM tblCompanyInfo WHERE GroupId = " + groupId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "CompanyName", "CompanyId", query, "SSIDB");
        }

        public void LoadDcInfoByUser(DropDownList ddl, string companyId, string userId)
        {
            string queryStr = @"SELECT DC.ComUnitId, DC.ComUnitCode + ' : ' + DC.ComUnitName AS DCName  FROM tblCompanyUnit AS DC WHERE DC.CompanyId = " + companyId + " AND ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId = " + userId + ")";
            aCommonInternalDAL.LoadDropDownValue(ddl, "DCName", "ComUnitId", queryStr, "SSIDB");
        }

        public void LoadDcInfo(DropDownList ddl, string companyId)
        {
            string query = @"SELECT CMU.ComUnitId,CMU.ComUnitCode + ':' + CMU.ComUnitName AS UnitName FROM dbo.tblCompanyUnit AS CMU WHERE CMU.CompanyId = " + companyId;
            aCommonInternalDAL.LoadDropDownValue(ddl, "UnitName", "ComUnitId", query, "SSIDB");
        }

        public DataTable LoadWingsTransitInfo(string parameter)
        {
            string query = @"SELECT CI.CompanyName,UNT.ComUnitName,ISNULL(SUM(INV.TpGrandTotal),0) AS TotalSales,(0.00)AS  Collection,(0.00)AS  Transit FROM dbo.tblInvoice AS INV 
                            INNER JOIN dbo.tblInvoiceDetail AS INVD ON INVD.InvoiceId = INV.InvoiceId
                            LEFT JOIN dbo.tblOrder AS ODR ON ODR.OrderId = INV.OrderId
                            INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = ODR.ManufacId
                            INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = INV.ComUnitId
                            WHERE INV.DeliveryInvoiceStatus IS NULL " + parameter + " GROUP BY CI.CompanyName,CUNT.ComUnitCode,CUNT.ComUnitName";

            return aCommonInternalDAL.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetDestroyedList(string pram)
        {
            string query = @"SELECT DP.DestroyId,CI.CompanyName,IUNT.ComUnitName,F.ProductCode,F.ProductName,F.BatchNo,F.ExpDate,DP.StockOutQty AS StockQty,DP.StockOutAmount AS Amount,
                             DP.Reason,DP.StockOutDate,DP.ApproveBy FROM dbo.tblDestroyProduct AS DP 
                             INNER JOIN dbo.tblDCStoreFreeze AS F ON DP.DcFreezeId = F.DCStoreFreezeId
                             INNER JOIN dbo.tblUnitPrice U ON F.ProductCode = U.ProductCode
                             INNER JOIN dbo.tblProduct P ON F.ProductCode = P.ProductCode
                             INNER JOIN dbo.tblCompanyUnit AS IUNT ON IUNT.ComUnitId = F.ComUnitId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON IUNT.CompanyId = CI.CompanyId
                             WHERE  U.IsActive = 1 " + pram + " ORDER BY F.ExpDate ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        //Destroy Report



        public void LoadManufac(DropDownList ddl)
        {
            ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void LoadStockCondition(DropDownList aDownList)
        {
            string StockCondition = "select * from tblStockCondition WHERE StockCondition<>'Available'";
            //string StockCondition = "select * from tblStockCondition WHERE StockCondition<>'Available' AND StockConId IN (SELECT StockConId FROM tblStockConditionPermission where Permission=1 AND UserId='" + userid + "')";
            aCommonInternalDal.LoadDropDownValue(aDownList, "StockCondition", "StockConId", StockCondition, "SSIDB");
        }

        public DataTable GetFreezeStockDataByCondition(string pram)
        {
            string query = @"SELECT F.DCStoreFreezeId,F.DCStoreId,F.ProductCode,F.ProductName,F.BatchNo,F.ExpDate,F.StockQty,
                           (U.UnitPrice*F.StockQty) AS Amount,F.StockCondition,F.Remarks
                           FROM dbo.tblDCStoreFreeze F
                           INNER JOIN dbo.tblUnitPrice U ON F.ProductCode = U.ProductCode
                           INNER JOIN dbo.tblProduct P ON F.ProductCode = P.ProductCode
                           INNER JOIN dbo.tblCompanyUnit AS IUNT ON IUNT.ComUnitId = F.ComUnitId
                           WHERE StockQty > 0 AND U.IsActive = 1 " + pram + " ORDER BY F.ExpDate ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public void UserDdl(DropDownList userDropDownList)
        {
            string query = "SELECT USR.UserId,USR.UserName AS LoginName from tblUser AS USR WHERE USR.UserId != 1 ORDER BY LoginName ";
            aCommonInternalDal.LoadDropDownValue(userDropDownList, "LoginName", "UserId", query, "SSIDB");
        }

        public bool SaveDestroyProductInfo(DestroyProductDao aInformationDao)
        {
            string insertQuery = @"INSERT INTO tblDestroyProduct (DcFreezeId,StockOutBy,StockOutQty,StockOutAmount,StockOutDate,ActionStatus,Reason,IsApproved,EntryBy,EntryDate) VALUES ('" + aInformationDao.DcFreezeId + "','" + aInformationDao.StockOutBy
                                     + "','" + aInformationDao.StockOutQty + "','" + aInformationDao.StockOutAmount + "','" + aInformationDao.StockOutDate + "','" + "Posted" + "','" + aInformationDao.Reason + "','" + aInformationDao.IsApproved + "','" + aInformationDao.EntryBy + "','" + aInformationDao.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public DataTable CheckFreezeIdExistOrNot(string DCStoreFreezeId)
        {
            string query = @"SELECT * FROM tblDestroyProduct WHERE DcFreezeId = " + DCStoreFreezeId ;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }

        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }

        public DataTable GetSpecialApproveOrder(string actionstatus)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT DSP.DcFreezeId,DSP.DestroyId,CI.CompanyName, UNT.ComUnitName,DCS.ProductCode,DCS.ProductName,DCS.BatchNo,DCS.MfgDate,DCS.ExpDate,DSP.StockOutQty,
            DSP.StockOutAmount,USR.UserName AS StockOutBy,DSP.Reason,DSP.StockOutDate FROM dbo.tblDestroyProduct AS DSP 
            INNER JOIN dbo.tblDCStoreFreeze AS DFZS ON DFZS.DCStoreFreezeId = DSP.DcFreezeId
            INNER JOIN dbo.tblDCStore AS DCS ON DCS.DCStoreId = DFZS.DCStoreId
            INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = DFZS.ComUnitId
            INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
            INNER JOIN dbo.tblUser AS USR ON USR.UserId = DSP.StockOutBy WHERE ActionStatus='" + actionstatus + "' AND DSP.IsApproved = 0 ORDER BY DSP.StockOutDate";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }

        public bool ApprovalUpdateDAL(DestroyProductDao aOrderInfoMaster)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@OrderId", aOrderInfoMaster.DestroyId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aOrderInfoMaster.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aOrderInfoMaster.ApproveBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aOrderInfoMaster.ApproveDate));

            string query = @"UPDATE tblDestroyProduct SET ActionStatus = @ActionStatus,ApproveBy = @ApprovedBy,ApproveDate = @ApprovedDate WHERE DestroyId = @OrderId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }

        public DataTable FetchDcFreezeQty(int dcFreezeId)
        {
            string query = @"SELECT StockQty FROM dbo.tblDCStoreFreeze WHERE DCStoreFreezeId = " + dcFreezeId;
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public bool UpdateFreezeStockQty(int dcFreezeId, decimal qty)
        {
            var aSqlParameterlist = new List<SqlParameter>
            {
                new SqlParameter("@DcFreezeId", dcFreezeId),
                new SqlParameter("@Quantity", qty)
            };

            const string query = @"UPDATE tblDCStoreFreeze SET StockQty = @Quantity WHERE DCStoreFreezeId = @DcFreezeId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }

        public DataTable LoadDestroyProductInfo(string parameter)
        {

            string query = @"SELECT DP.DestroyId,CI.CompanyName,IUNT.ComUnitName,F.ProductCode,F.ProductName,F.BatchNo,USR.UserName,
                             F.ExpDate,DP.StockOutQty AS StockQty,DP.StockOutAmount AS Amount,PKS.PackSizeName,
                             DP.Reason,DP.StockOutDate,DP.ApproveBy,DP.ApproveDate FROM dbo.tblDestroyProduct AS DP 
                             INNER JOIN dbo.tblDCStoreFreeze AS F ON DP.DcFreezeId = F.DCStoreFreezeId
                             INNER JOIN dbo.tblUnitPrice U ON F.ProductCode = U.ProductCode
                             INNER JOIN dbo.tblProduct P ON F.ProductCode = P.ProductCode
                             INNER JOIN dbo.tblPackSize AS PKS ON PKS.PackSizeId = P.PackSizeId
                             INNER JOIN dbo.tblCompanyUnit AS IUNT ON IUNT.ComUnitId = F.ComUnitId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON IUNT.CompanyId = CI.CompanyId
                             INNER JOIN dbo.tblUser AS USR ON USR.UserId = DP.StockOutBy
                             WHERE  U.IsActive = 1 AND DP.DestroyId = " + parameter + " ORDER BY F.ExpDate ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetNonTranscationalInvoice(string actionStatus)
        {
            DataTable aDataTable = new DataTable();
            string query = @"SELECT NTV.InvReturnMasterID,NTV.ReturnCode,INV.InvoiceNo,CI.CompanyName,NTV.ReturnDate,NTV.SalesDate,UNT.ComUnitName,CSTMR.CustomerCode,CSTMR.CustomerName,NTV.TotalReturn FROM dbo.tblNonTranscationalInvoiceMaster AS NTV
                             INNER JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = NTV.CustomerMasterId
                             INNER JOIN dbo.tblCompanyUnit AS UNT ON UNT.ComUnitId = NTV.ComUnitId
                             INNER JOIN dbo.tblCompanyInfo AS CI ON CI.CompanyId = UNT.CompanyId
                             INNER JOIN dbo.tblInvoice AS INV ON INV.InvoiceId = NTV.InvoiceId WHERE ActionStatus='" + actionStatus + "' ORDER BY UNT.EntryDate";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }

        public int NonTranscationalInvoiceDAL(tblNonTranscationalInvoiceMasterDao aOrderInfoMaster)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvReturnMasterID", aOrderInfoMaster.InvReturnMasterID));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aOrderInfoMaster.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aOrderInfoMaster.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aOrderInfoMaster.ApprovedDate));

            return aCommonInternalDal.RunStoreProcedure("sp_NonTranscationalInvoiceApproval", aSqlParameterlist, "SSIDB");
        }

        public bool PostJournal(int destroyId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@DestroyId", destroyId));

            return aCommonInternalDal.UpdateAction("sp_ProductDestroyPosting", aSqlParameterList);
        }
    }
}
