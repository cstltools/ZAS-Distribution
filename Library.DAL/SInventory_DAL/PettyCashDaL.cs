using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.PettyCashDao;

namespace Library.DAL.PettyCash_DaL
{
  public  class PettyCashDaL
    {
        ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
        PettyCashDetailsDao aPettyCashDetailsDao = new PettyCashDetailsDao();
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

        public void LoadCompanydropdown(DropDownList ddl)
        {
           // ClsCommonInternalDAL aInternalDal = new ClsCommonInternalDAL();
            string queryStr = "select * from tblCompanyInfo";
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }



        public void LoadCashAccDal(DropDownList ddl, string companyId)
        {
            string queryStr = @"SELECT AccountId,COA.AccountName FROM ZAS_ACCDB..tblChartOfAccounts AS COA
                LEFT JOIN ZAS_ACCDB..tblMultipleCompanySameCashAccount AS MC ON MC.CashAccountId = COA.CashAccountId
            WHERE COA.CashAccountId IS NOT NULL AND MC.CompanyId =" + companyId;
            aInternalDal.LoadDropDownValueWithoutDataBase(ddl, "AccountName", "AccountId", queryStr);

        }

      //delete pettycash master dal 

      public bool MasterDeleteDataDal(string Id)
      {
          List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
          aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", Id));


          return aInternalDal.DeleteAction("sp_DEL_PettyCashMaster", aSqlParameterlist);
      }


      //delete pettycash Details dal 

      public bool DetailsDeleteDataDal(string Id)
      {
          List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
          aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", Id));
          return aInternalDal.DeleteAction("sp_DEL_PettyCashDetails", aSqlParameterlist);
      }




//        public Int32 SavePrettyCashMasterid(PettyCashMasterDao aCashMasterDao)
//        {
//            //List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
//            //aSqlParameterlist.Add(new SqlParameter("@ConpanyId", aCashMasterDao.ConpanyId));
//            //aSqlParameterlist.Add(new SqlParameter("@PostingDate", aCashMasterDao.PostingDate));
//            //aSqlParameterlist.Add(new SqlParameter("@CashAccId", aCashMasterDao.CashAccId));
//            //aSqlParameterlist.Add(new SqlParameter("@Remark", aCashMasterDao.Remark));
//            //aSqlParameterlist.Add(new SqlParameter("@EntryBy", aCashMasterDao.EntryBy));
//            //aSqlParameterlist.Add(new SqlParameter("@EntryDate", aCashMasterDao.EntryDate));

//            string insertQuery = @"insert into tblPettycashMaster (ConpanyId,PostingDate,CashAccId,Remark,EntryBy,EntryDate) 
//            values ('" + aCashMasterDao.ConpanyId + "','"+ aCashMasterDao.PostingDate+ "','" + aCashMasterDao.CashAccId + "','" + aCashMasterDao.Remark + "'," +
//                                 " '" + aCashMasterDao.EntryBy + "','" + aCashMasterDao.EntryDate + "')";
//            return aInternalDal.SaveDataByInsertCommandpp(insertQuery);
//        }

     

        //public int Update(PettyCashDetailsDao aCashDetailsDao)
        //{
        //    string insertQuery = @"update tblPettyCashDetails Set   PerticulerId = '" + aCashDetailsDao.PerticulerId + "', CashReason = '" + aCashDetailsDao.CashReason + "', Amount= '"+aCashDetailsDao.Amount+"' where PettyCashmasterId = '"+aCashDetailsDao.PettyCashmasterId+"'";
        //    return aCommonInternalDal.UpdateDataByCommand(insertQuery, "SSIDB");
        //}






        public Int32 SaveDataForDetail(PettyCashDetailsDao aCashDetailsDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", aCashDetailsDao.PettyCashmasterId));
            aSqlParameterlist.Add(new SqlParameter("@PerticulerId", aCashDetailsDao.PerticulerId));
            aSqlParameterlist.Add(new SqlParameter("@CashReason", aCashDetailsDao.CashReason));
            aSqlParameterlist.Add(new SqlParameter("@Amount", aCashDetailsDao.Amount));


            return aInternalDal.SaveAction("sp_I_PrettyCashDetails", aSqlParameterlist, "@PettyCashDetailsId");


        }

        


        public Int32 SavePettyCashMaster(PettyCashMasterDao aCashMasterDao)
        {
           
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@ConpanyId", aCashMasterDao.ConpanyId));
            aSqlParameterlist.Add(new SqlParameter("@PostingDate", aCashMasterDao.PostingDate));
            aSqlParameterlist.Add(new SqlParameter("@CashAccId", aCashMasterDao.CashAccId));
            aSqlParameterlist.Add(new SqlParameter("@Remark", aCashMasterDao.Remark));
            aSqlParameterlist.Add(new SqlParameter("@EntryBy", aCashMasterDao.EntryBy));
            aSqlParameterlist.Add(new SqlParameter("@EntryDate", aCashMasterDao.EntryDate));
            aSqlParameterlist.Add(new SqlParameter("@ActiveStatus", aCashMasterDao.ActiveStatus));

            
            return aInternalDal.SaveAction("sp_I_PrettyCashMaster", aSqlParameterlist, "@PettyCashmasterId");

        }



        public DataTable GetAllAvailablePrettyCash()
        {
            DataTable aDataTable = new DataTable();
            string query = @"
            Select tblPettyCashMaster.PettyCashmasterId,TbCom.CompanyName, tblPettyCashMaster.PostingDate As PostingDate, CHA.AccountName As CashAcc, tblPettyCashMaster.Remark,
            CCHA.AccountName As PerticularName, tblPettyCashDetails.CashReason, tblPettyCashDetails.Amount,tblPettyCashMaster.ActiveStatus
            from tblPettyCashMaster
            LEFT JOIN tblPettyCashDetails On tblPettyCashDetails.PettyCashmasterId = tblPettyCashMaster.PettyCashmasterId
			Left JOIn tblCompanyInfo AS TBCom On tblPettyCashMaster.ConpanyId = TBCom.CompanyId
			LEFT JOIN ZAS_ACCDB..tblChartOfAccounts As CHA on tblPettyCashMaster.CashAccId = CHA.AccountId
			LEFT JOIN ZAS_ACCDB..tblChartOfAccounts AS CCHA on tblPettyCashDetails.PerticulerId = CCHA.AccountId";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }

      //product discount check 

      public DataTable CheckProductDisCountdal()
      {
          DataTable aDataTable = new DataTable();
          string query = @"Select ActionStatus, IsActive from tblProductDiscount";
          aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

          return aDataTable;
      }

       


        public DataTable GetMasterInfoByIdDal(int id)
        {

            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", id));
            return aInternalDal.GetDataTableAction("sp_GET_PretyCashMaster_ById", aSqlParameterlist, "SSIDB");

        }


        //
        public DataTable GetDetailsInfoByIdDal(int id)
        {

            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", id));
            return aInternalDal.GetDataTableAction("sp_GET_PretyCashDetails_ById", aSqlParameterlist, "SSIDB");

        }



        public bool UpdateMasterDataDal(PettyCashMasterDao aCashMasterDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", aCashMasterDao.PettyCashmasterId));
            aSqlParameterlist.Add(new SqlParameter("@ConpanyId", aCashMasterDao.ConpanyId));
            aSqlParameterlist.Add(new SqlParameter("@PostingDate", aCashMasterDao.PostingDate));
            aSqlParameterlist.Add(new SqlParameter("@CashAccId", aCashMasterDao.CashAccId));
            aSqlParameterlist.Add(new SqlParameter("@Remark", aCashMasterDao.Remark));
            aSqlParameterlist.Add(new SqlParameter("@UpdateBy", aCashMasterDao.UpdateBy));
            aSqlParameterlist.Add(new SqlParameter("@UpdateDate", aCashMasterDao.UpdateDate));

            return aInternalDal.UpdateAction("sp_UD_prettyCashMaster", aSqlParameterlist);
        }

        public bool UpdateDetailsDataDal(PettyCashDetailsDao aCashDetailsDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", aCashDetailsDao.PettyCashmasterId));
            aSqlParameterlist.Add(new SqlParameter("@PerticulerId", aCashDetailsDao.PerticulerId));
            aSqlParameterlist.Add(new SqlParameter("@CashReason", aCashDetailsDao.CashReason));
            aSqlParameterlist.Add(new SqlParameter("@Amount", aCashDetailsDao.Amount));


            return aInternalDal.UpdateAction("sp_UD_prettyCashDetails", aSqlParameterlist);
        }

        public bool ApprovalUpdateDAL(PettyCashMasterDao aCashMasterDao)
        {
            List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
            aSqlParameterlist.Add(new SqlParameter("@PettyCashmasterId", aCashMasterDao.PettyCashmasterId));
            aSqlParameterlist.Add(new SqlParameter("@ActiveStatus", aCashMasterDao.ActiveStatus));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedBy", aCashMasterDao.ApprovedBy));
            aSqlParameterlist.Add(new SqlParameter("@ApprovedDate", aCashMasterDao.ApprovedDate));
            string query = @"UPDATE tblPettyCashMaster SET ActiveStatus=@ActiveStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE PettyCashmasterId=@PettyCashmasterId";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, aSqlParameterlist, "SSIDB");
        }


        ClsApprovalAction approvalAction = new ClsApprovalAction();

        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }


        public DataTable GetSpecialApproveOrder(string actionstatus)
        {

            DataTable aDataTable = new DataTable();
            string query = @"Select tblPettyCashMaster.PettyCashmasterId, tblPettyCashMaster.ConpanyId As CompanyId, tblPettyCashMaster.PostingDate As PostingDate,tblPettyCashMaster.CashAccId As CashAccId, tblPettyCashMaster.Remark ,
   tblPettyCashDetails.PerticulerId , tblPettyCashDetails.CashReason, tblPettyCashDetails.Amount,tblPettyCashMaster.ActiveStatus, tblPettyCashMaster.EntryBy,  tblPettyCashMaster.ApprovedBy,  tblPettyCashMaster.UpdateBy
   from tblPettyCashMaster
   LEFT JOIN tblPettyCashDetails On tblPettyCashDetails.PettyCashmasterId = tblPettyCashMaster.PettyCashmasterId 
   where tblPettyCashMaster.PettyCashmasterId is NOT NUll AND Activestatus= '" + actionstatus + "'";
            aDataTable = aCommonInternalDal.DataContainerDataTable(query, "SSIDB");

            return aDataTable;
        }


        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }




      //pettycash report view
        public DataTable GetAllEmployeeInformation(string parameter)
        {
//            string queryStr = @"Select  tblCompanyInfo.CompanyName As CompanyName,CHA.AccountName As CashAccount, tblPettyCashMaster.PostingDate As PostingDate, tblPettyCashMaster.Remark , BCA.AccountName As AccountName, 
//            tblPettyCashDetails.CashReason As CashReason, tblPettyCashDetails.Amount As Amount
//            
//			from tblPettyCashMaster
//	
//            LEFT JOIN tblPettyCashDetails On tblPettyCashDetails.PettyCashmasterId = tblPettyCashMaster.PettyCashmasterId
//			LEFT JOiN tblCompanyInfo On tblPettyCashMaster.ConpanyId = tblCompanyInfo.CompanyId
//			LEFT JOIN ZAS_ACCDB..tblChartOfAccounts As CHA On CHA.AccountId = tblPettyCashMaster.CashAccId
//			LEFT JOIN ZAS_ACCDB..tblChartOfAccounts BCA On BCA.AccountId = tblPettyCashDetails.PerticulerId
//	      	where tblPettyCashMaster.PettyCashmasterId Is NOT NUll " + parameter;


            string queryStr =
                @"Select Com.CompanyName As CompanyName,CHA.AccountName As CashAccount, Mas.PostingDate As PostingDate, Mas.Remark , BCA.AccountName As AccountName, 
            pcd.CashReason As CashReason, pcd.Amount, TotalAmount       
			from tblPettyCashMaster As Mas
            LEFT JOIN tblPettyCashDetails As pcd On pcd.PettyCashmasterId = Mas.PettyCashmasterId
			LEFT JOIN ZAS_ACCDB..tblChartOfAccounts As CHA On CHA.AccountId = Mas.CashAccId
			LEFT JOIN ZAS_ACCDB..tblChartOfAccounts BCA On BCA.AccountId = pcd.PerticulerId
			LEFT JOiN tblCompanyInfo As Com On Com.CompanyId = mas.ConpanyId
            LEFT JOIN (
			SELECT PettyCashmasterId,SUM(Amount) AS TotalAmount FROM tblPettyCashDetails GROUP By PettyCashmasterId
			) AS TT ON MAS.PettyCashmasterId = TT.PettyCashmasterId
	      	where Mas.PettyCashmasterId Is NOT NUll'"+parameter+"'GROUP By Mas.PettyCashmasterId, Com.CompanyName,CHA.AccountName , Mas.PostingDate, Mas.Remark , BCA.AccountName, pcd.CashReason,pcd.Amount,TotalAmount";
           
      


            return aInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }




        public DataTable LoadCreditCustomerReport(string parameter)
        {
            string queryStr = @"Select cust.CustomerCode As CustomerID, cust.CustomerName , cust.Address, com.CompanyName,  DATEDIFF(day, StartDate, EndDate) AS DayLimit, LimitAmount As ValueLimit from tblCustomerCreditLimit 
              Left join tblCustMaster As cust On cust.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
              Left Join tblCompanyInfo As com On com.CompanyId = tblCustomerCreditLimit.CompanyId 
              Where tblCustomerCreditLimit.CreditLimitId Is NOT NULL " + parameter;
            return aInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        //Load Product discount report All
        public DataTable LoadProductDiscountReportAlldal( )
        {
            string queryStr = @"select com.CompanyName , Cust.CustomerCode As CustomerId, CustomerCode + ' - ' + Cust.CustomerName AS CustomerName, Cust.Address,Pro.ProductCode,Pro.ProductName, tblProductDiscount.DiscountPercentage from tblProductDiscount
		Left join tblCustMaster As Cust On Cust.CustomerMasterId= tblProductDiscount.CustomerMasterId
		Left JOin tblProduct AS Pro On Pro.ProductId = tblProductDiscount.ProductId
		Left JOIN tblCompanyInfo As Com on Com.companyId= Pro.companyId

		Where tblProductDiscount.DiscountId Is Not NUll And tblProductDiscount.ActionStatus='Accepted' And tblProductDiscount.IsActive=1 ";
            return aInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        //Load Product Discount report
        public DataTable LoadProductDiscountReportdal(string parameter)
        {
            string queryStr = @"select com.CompanyName , Cust.CustomerCode As CustomerId, CustomerCode + ' - ' + Cust.CustomerName AS CustomerName, Cust.Address,Pro.ProductCode,Pro.ProductName, tblProductDiscount.DiscountPercentage from tblProductDiscount
		Left join tblCustMaster As Cust On Cust.CustomerMasterId= tblProductDiscount.CustomerMasterId
		Left JOin tblProduct AS Pro On Pro.ProductId = tblProductDiscount.ProductId
		Left JOIN tblCompanyInfo As Com on Com.companyId= Pro.companyId
		Where tblProductDiscount.DiscountId Is Not NUll And tblProductDiscount.ActionStatus='Accepted' And tblProductDiscount.IsActive=1 " + parameter;
            return aInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }


        public DataTable LoadCreditCustomerReportAll()
        {
            string queryStr = @"Select cust.CustomerCode As CustomerID, cust.CustomerName , cust.Address, com.CompanyName, DayLimit, LimitAmount As ValueLimit from tblCustomerCreditLimit 

              Left join tblCustMaster As cust On cust.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
              Left Join tblCompanyInfo As com On com.CompanyId = tblCustomerCreditLimit.CompanyId 
              Where tblCustomerCreditLimit.CreditLimitId Is NOT NULL And tblCustomerCreditLimit.ActionStatus='Accepted' And tblCustomerCreditLimit.IsActive=1";
            return aInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }


        public DataTable Load()
        {
            string queryStr = @"Select cust.CustomerCode As CustomerID, cust.CustomerName , cust.Address, com.CompanyName, DayLimit, LimitAmount As ValueLimit from tblCustomerCreditLimit 

          Left join tblCustMaster As cust On cust.CustomerMasterId = tblCustomerCreditLimit.CustomerMasterId
          Left Join tblCompanyInfo As com On com.CompanyId = tblCustomerCreditLimit.CompanyId 
          Where tblCustomerCreditLimit.CreditLimitId Is NOT NULL" ;
            return aInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

    }
}
