using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class RollBackDAL
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public DataTable GetStockInTransfer(string reqId)
        {
            string query = @"SELECT * FROM dbo.tblStockInTransfar WHERE ReqId='" + reqId + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool UpdatePickingInformationOnRequisitionDAL(string id)
        {
            string query = @"UPDATE tblRequisition SET  CreatePicking=NULL,PickingDate=NULL,PickingNo=NULL, " +
                              " TruckNo=NULL,DriverName=NULL,TotalPrice=NULL,TotalVAT=NULL ," +
                              " GrandTotalPrice=NULL WHERE ReqId='" + id + "'";


            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateIssueInformationOnRequisitionChildDAL(string id)
        {
            string query = @" UPDATE tblRequsitionChild SET IssueQty=NULL,UnitPrice=NULL,PriceAmount=NULL, " +
                           " VATAmount=NULL,TotalPrice=NULL,IsPicking=NULL,CaseQty=NULL,MusakVATAmount=NULL,MusakTotalPrice=NULL   WHERE ReqChildId='" + id + "'";

            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateCentralStockStockOut(decimal quantity, string receiveId)
        {
            string query = @"UPDATE dbo.tblCentralStore SET Quantity=Quantity+" + quantity + " WHERE ReceiveId='" + receiveId + "'";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool DeleteStockInTransfer(string id)
        {
            string query = @" DELETE FROM dbo.tblStockInTransfar WHERE ReqId='"+id+"'";

            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public DataTable GetAllStockRcvByDcDAL()
        {
            string query = @"SELECT * FROM dbo.tblRequisition WHERE Submit='OK' AND (ReceiveIssue IS NULL OR ReceiveIssue ='') ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool UpdateReqDetailIssueStatusDAL(string Reqid)
        {
            string query = @"UPDATE dbo.tblRequsitionChild SET IsIssue=NULL WHERE ReqId='" + Reqid + "' ";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
        public bool UpdateIssueInformationOnRequisitionDAL(string Reqid)
        {
            string query = @"UPDATE tblRequisition SET  Submit=NULL, IssueChalanNo=NULL WHERE ReqId='" + Reqid + "'";

            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }
    }
}
