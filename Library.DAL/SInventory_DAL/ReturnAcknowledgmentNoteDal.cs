using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ReturnAcknowledgmentNoteDal
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();

        public void GetDdlCompany(DropDownList ddl)
        {
            const string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void GetDdlRegion(DropDownList ddl)
        {
            const string queryStr = "SELECT * FROM dbo.tblRegion";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "RegionName", "RegionId", queryStr);
        }



        public int SaveReturnAckwnoledgementNote(RAN_MasterDao ranMaster, List<RAN_DetailsDao> ranDetails)
        {
            int status = 0;
            List<int> savedDetailIds = new List<int>();

            // Save the master record once
            int ranMasterId = SaveRANMaster(ranMaster);

            if (ranMasterId <= 0)
            {
                // Master save failed, return 0
                return 0;
            }

            bool allDetailsSaved = true;

            foreach (var detail in ranDetails)
            {
                // Assign master ID to detail
                detail.RanMasterID = ranMasterId;

                int detailResult = SaveRANDetail(detail);

                if (detailResult <= 0)
                {
                    // Rollback the master if any detail fails

                    DeleteRANMaster(ranMasterId);  // You need to implement this method to delete master record

                    allDetailsSaved = false;
                    break;
                }

                // Optional: PaymentJournalPosting or other post-processing here if needed
                // PaymentJournalPosting(ranMasterId, detail.ProductId);

                savedDetailIds.Add(detailResult);
            }

            status = allDetailsSaved ? ranMasterId : 0;

            return status;
        }

        
        private bool DeleteRANMaster(int ranMasterId)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@RanMasterId", ranMasterId));
            return aInternalDal.DeleteAction("sp_RAN_DeleteById", aSqlParameters);

        }



        public int SaveRANMaster(RAN_MasterDao ranMaster)
        {
            List<SqlParameter> aSqlParameters = new List<SqlParameter>();


            aSqlParameters.Add(new SqlParameter("@InvoiceId", ranMaster.InvoiceId.HasValue ? (object)ranMaster.InvoiceId.Value : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@ReturnDate", ranMaster.ReturnDate.HasValue ? (object)ranMaster.ReturnDate.Value : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@CustomerMasterId", ranMaster.CustomerMasterId.HasValue ? (object)ranMaster.CustomerMasterId.Value : 0));
            aSqlParameters.Add(new SqlParameter("@ComUnitId", ranMaster.ComUnitId.HasValue ? (object)ranMaster.ComUnitId.Value : 0));
            aSqlParameters.Add(new SqlParameter("@TotalReturn", ranMaster.TotalReturn.HasValue ? (object)ranMaster.TotalReturn.Value : 0m));
            aSqlParameters.Add(new SqlParameter("@CreateBy", !string.IsNullOrEmpty(ranMaster.CreateBy) ? (object)ranMaster.CreateBy : DBNull.Value));
            aSqlParameters.Add(new SqlParameter("@TotalValue", ranMaster.TotalValue.HasValue ? (object)ranMaster.TotalValue.Value : 0m));

            return aInternalDal.SaveAction("sp_RAN_MasterInsert", aSqlParameters, "@RanMasterID");
        }


        public int SaveRANDetail(RAN_DetailsDao ranDetail)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();


            aSqlParameterList.Add(new SqlParameter("@RanMasterID", ranDetail.RanMasterID ?? (object)DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@ProductId", ranDetail.ProductId ?? (object)DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@Batch", !string.IsNullOrEmpty(ranDetail.Batch) ? (object)ranDetail.Batch : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@ExpDate", ranDetail.ExpDate.HasValue ? (object)ranDetail.ExpDate.Value : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@MfgDate", ranDetail.MfgDate.HasValue ? (object)ranDetail.MfgDate.Value : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@Qty", ranDetail.Qty.HasValue ? (object)ranDetail.Qty.Value : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@Price", ranDetail.Price.HasValue ? (object)ranDetail.Price.Value : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@VAT", ranDetail.VAT.HasValue ? (object)ranDetail.VAT.Value : DBNull.Value));
            aSqlParameterList.Add(new SqlParameter("@TotalAmount", ranDetail.TotalAmount.HasValue ? (object)ranDetail.TotalAmount.Value : DBNull.Value));

            return aInternalDal.SaveAction("sp_RAN_DetailInsert", aSqlParameterList, "@RanDetailID");
        }


        public int SaveNonTranscationalMaster(tblNonTranscationalInvoiceMasterDao aStockInMasterDao)
        {

            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

            aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aStockInMasterDao.InvoiceId));
            aSqlParameterlist.Add(new SqlParameter("@ReturnDate", aStockInMasterDao.ReturnDate));
            aSqlParameterlist.Add(new SqlParameter("@SalesDate", aStockInMasterDao.SalesDate));
            aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aStockInMasterDao.CustomerMasterId));
            aSqlParameterlist.Add(new SqlParameter("@ReturnCode", aStockInMasterDao.ReturnCode));
            aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aStockInMasterDao.ComUnitId));
            aSqlParameterlist.Add(new SqlParameter("@ActionStatus", aStockInMasterDao.ActionStatus));
            aSqlParameterlist.Add(new SqlParameter("@TotalReturn", aStockInMasterDao.TotalReturn));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", aStockInMasterDao.EntryBy));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", aStockInMasterDao.EntryDate));
            aSqlParameterlist.Add(new SqlParameter("@TotalValue", aStockInMasterDao.TotalValue));

            string insertQuery = @"INSERT INTO dbo.tblNonTranscationalInvoiceMaster ( InvoiceId,
          ReturnDate,
          ReturnCode,
          SalesDate,
          CustomerMasterId ,
          ComUnitId ,
          CreateBy ,
          CreateDate,
          ActionStatus,
          TotalReturn,TotalValue)
          VALUES  (@InvoiceId,@ReturnDate,@ReturnCode,@SalesDate,@CustomerMasterId,@ComUnitId,@EntryBy,@EntryDate,@ActionStatus,@TotalReturn,@TotalValue)";

            return aInternalDal.SaveDataByInsertCommandById(insertQuery, aSqlParameterlist, "SSIDB");
        }


        public bool SaveNonTranscationalMasterDetail(List<NonTranscationalInvoiceDetailDao> aDetailList)
        {

            bool status = false;

            foreach (NonTranscationalInvoiceDetailDao aDetailDao in aDetailList)
            {
                string insertQuery = @"INSERT INTO dbo.tblNonTranscationalInvoiceDetail
                ( 
                    InvReturnMasterID,ProductId,Batch, ExpDate, MfgDate, Qty, Price, VAT, TotalAmount 
                )
                VALUES  ('" + aDetailDao.InvReturnMasterID + "'," +
                 "'" + aDetailDao.ProductId + "'," +
                 "'" + aDetailDao.Batch + "'," +
                 "'" + aDetailDao.ExpDate + "'," +
                 "'" + aDetailDao.MfgDate + "'," +
                 "'" + aDetailDao.Qty + "'," +
                 "'" + aDetailDao.Price + "'," +
                 "'" + aDetailDao.VAT + "'," +
                 "'" + aDetailDao.TotalAmount + "' " + ")";

                status = aInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
            }

            return status;

        }
    }
}
