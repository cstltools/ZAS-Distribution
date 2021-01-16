using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using CrystalDecisions.Shared.Json;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

/// <summary>
/// Summary description for SInventoryWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SInventoryWebService : System.Web.Services.WebService
{

    public SInventoryWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    private string strConnectionString = ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ToString();
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetPerticularaction(string prefixText)
    {
        //string param = "";
        //if (Session["MOParam"] != null)
        //{
        //    param = Session["MOParam"].ToString();
        //}
        string sql = "";

        sql = @"SELECT COA.AccountName + '~'+ CAST(COA.AccountId AS NVARCHAR) AS Name FROM ZAS_ACCDB..tblChartOfAccounts AS COA 
                WHERE COA.BankAccountId IS NULL AND COA.CashAccountId IS NULL AND COA.AccountName + '~'+ CAST(COA.AccountId AS NVARCHAR) LIKE @prefixText";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Name"].ToString(), i);
            i++;
        }
        return items;
    }

    //web service for product discount page 

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] loadCustomerInfoforProductdiscount(string prefixText)
    {
        string param = "";
        if (Session["MOParam"] != null)
        {
            param = Session["MOParam"].ToString();
        }
        string sql = "";

        sql = @"SELECT  CAST(tblCustMaster.CustomerMasterId AS nvarchar(50))+':'+tblCustMaster.CustomerName As Name FROM dbo.tblCustMaster
             WHERE (CAST(tblCustMaster.CustomerMasterId AS nvarchar(50))+' '+tblCustMaster.CustomerName) like @prefixText " + param + "";


        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Name"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] loadCustomernew(string prefixText)
    {
        string param = "";
        if (Session["MOParam"] != null)
        {
            param = Session["MOParam"].ToString();
        }
        string sql = "";

        sql = @"SELECT  CAST(tblCustMaster.CustomerMasterId AS nvarchar(50))+':'+tblCustMaster.CustomerName As Cust FROM dbo.tblCustMaster
             WHERE (CAST(tblCustMaster.CustomerMasterId AS nvarchar(50))+' '+tblCustMaster.CustomerName) like @prefixText " + param + "";


        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Cust"].ToString(), i);
            i++;
        }
        return items;
    }

    WebServiceDal aServiceDal = new WebServiceDal();

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomerInfo(string prefixText)
    {
        DataTable customers = aServiceDal.GetCustomerInfo(prefixText);

        var customer = new string[customers.Rows.Count];

        int i = 0;

        foreach (DataRow dr in customers.Rows)
        {
            customer.SetValue(dr["CustomerName"].ToString(), i);
            i++;
        }

        return customer;
    }



    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomerInfoByCode(string prefixText)
    {
        DataTable customers = aServiceDal.GetCustomerInfoByCode(prefixText);

        var customer = new string[customers.Rows.Count];

        int i = 0;

        foreach (DataRow dr in customers.Rows)
        {
            customer.SetValue(dr["CustomerName"].ToString(), i);
            i++;
        }

        return customer;
    }



    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetApprovedProduct(string prefixText)
    {

        Int32 companyId = 0;

        if (Session["CompanyId"].ToString() != "")
        {
            companyId = Convert.ToInt32(Session["CompanyId"].ToString());
        }

        string sql = @"SELECT PD.ProductCode+':'+PD.ProductName AS Product FROM tblProduct AS PD
                            INNER JOIN tblUnitPrice AS UP ON PD.ProductId = UP.ProductId
                           WHERE  UP.ActionStatus = 'Accepted' AND UP.IsActive = 'True' AND CompanyId = '" + companyId + "' AND PD.ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    


    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProduct(string prefixText)
    {

        //int manufac = 0;
        //if (contextKey != "")
        //{
        //    string[] St = new string[] { };
        //    DataTable dT = new DataTable();
        //    string[] mainstrnin = contextKey.Split(',');
        //    manufac = Convert.ToInt32(mainstrnin[0]);
        //}

        Int32 companyId = 0;

        if (Session["CompanyId"].ToString() != "")
        {
            companyId = Convert.ToInt32(Session["CompanyId"].ToString());
        }

        string sql = "SELECT ProductCode+':'+ProductName AS Product FROM tblProduct WHERE CompanyId = '" + companyId + "' AND ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomer(string prefixText)
    {
        string sql = "";

        if (Session["UserType"].ToString() == "Admin")
        {
            sql = "select CustomerCode+':'+CustomerName as Customer from tblCustMaster where CustomerName like @prefixText";
        }
        else
        {
            string comUnit = Session["ComUnitId"].ToString();
            sql = "select CustomerCode+':'+CustomerName as Customer from tblCustMaster where ComUnitId='" + comUnit.Trim() + "'  and  CustomerName like @prefixText";
        }

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Customer"].ToString(), i);
            i++;
        }
        return items;
    }
    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomernew(string prefixText)
    {
        string param = "";
        if (Session["MOParam"] != null)
        {
            param = Session["MOParam"].ToString();
        }
        string sql = "";

        sql = @"SELECT DISTINCT tblCustMaster.CustomerCode+':'+tblCustMaster.CustomerName + ':( Region- ' + tblRegion.RegionName + '; Area - ' + tblArea.AreaName + '; Territory - ' + tblTerritory.TerritoryName + '; Market - ' +tblMarket.MarketName +' )'  +':'+CONVERT(NVARCHAR(MAX),tblCustMaster.CustomerMasterId)Cust FROM dbo.tblCustMaster
            INNER JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
            INNER JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
            INNER JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId 
			INNER JOIN dbo.tblArea ON tblArea.AreaId = tblTerritory.AreaId
            INNER JOIN tblRegion ON tblArea.RegionId = tblRegion.RegionId
			INNER JOIN dbo.tblDcWiseAreaInfo ON tblDcWiseAreaInfo.AreaId = tblArea.AreaId
			INNER JOIN dbo.tblCompanyUnit  ON dbo.tblDcWiseAreaInfo.DCId=dbo.tblCompanyUnit.ComUnitId
            INNER JOIN dbo.tblEmpGeneralInfo ON dbo.tblMIOInfo.EmployeeId=dbo.tblEmpGeneralInfo.EmpInfoId WHERE tblCustMaster.IsActive = 1 AND tblCustMaster.CustomerCode+':'+tblCustMaster.CustomerName+':'+CONVERT(NVARCHAR(MAX),tblCustMaster.CustomerMasterId) 
			LIKE  @prefixText " + param + "";



        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Cust"].ToString(), i);
            i++;
        }
        return items;
    }
    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetDelivaryInfo(string prefixText)
    {
        string sql = "";

        sql = "SELECT (DeliveryManCode+':'+DeliveryManName+':'+PhoneNo+':'+CONVERT(NVARCHAR(MAX),DeliveryManId))Del FROM dbo.tblDeliveryManInfo WHERE (DeliveryManCode+':'+DeliveryManName+':'+PhoneNo+':'+CONVERT(NVARCHAR(MAX),DeliveryManId)) LIKE  @prefixText";


        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Del"].ToString(), i);
            i++;
        }
        return items;
    }
    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetCustomerWithMIO(string prefixText)
    {
        string sql = "";

        sql = @"SELECT DISTINCT CustomerCode+':'+CustomerName+':'+CONVERT(NVARCHAR(MAX),CustomerMasterId)Cust FROM dbo.tblCustMaster
LEFT JOIN dbo.tblMarket ON tblMarket.MarketId = tblCustMaster.MarketId
LEFT JOIN dbo.tblTerritory ON tblTerritory.TerritoryId = tblMarket.TerritoryId
LEFT JOIN dbo.tblMIOInfo ON tblMIOInfo.TerritoryId = tblTerritory.TerritoryId WHERE MIOId='" + Session["MIOId"].ToString() + "' AND  (CustomerCode+':'+CustomerName+':'+CONVERT(NVARCHAR(MAX),CustomerMasterId)) LIKE @prefixText";


        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Cust"].ToString(), i);
            i++;
        }
        return items;
    }
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProduct2(string prefixText)
    {
        string sql = "SELECT ProductName+':'+ProductCode AS Product FROM tblProduct WHERE ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProduct3(string prefixText)
    {
        string sql = "SELECT ProductCode+':'+ProductName+':'+PackSize AS Product FROM tblProduct WHERE ProductName like @prefixText";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }

    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetEmpInfo(string prefixText)
    {
        string sql = "";

        if (Session["UserType"].ToString() == "Admin")
        {
            sql = "select EmpMasterCode+':'+EmpName as EmpInfo from tblEmpGeneralInfo where EmpName like @prefixText";
        }
        else
        {
            string comUnit = Session["ComUnitId"].ToString();
            sql = "select EmpMasterCode+':'+EmpName as EmpInfo from tblEmpGeneralInfo where ComUnitId='" + comUnit.Trim() + "'  and  EmpName like @prefixText";
        }

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["EmpInfo"].ToString(), i);
            i++;
        }
        return items;
    }
    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetConsumerCustomer(string prefixText)
    {
        string sql = "";

        //if (Session["UserType"].ToString() == "Admin")
        //{
        //    sql = "select EmpMasterCode+':'+EmpName as EmpInfo from tblEmpGeneralInfo where EmpName like @prefixText";
        //}
        //else
        {
            //string comUnit = Session["ComUnitId"].ToString();
            sql = "SELECT (CustomerPhoneNo+':'+CustomerName+':'+CONVERT(NVARCHAR(MAX),ConsumerCustId))Cust FROM dbo.tblConsumerCustomer WHERE   (CustomerPhoneNo+':'+CustomerName+':'+CONVERT(NVARCHAR(MAX),ConsumerCustId)) LIKE @prefixText";
        }

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Cust"].ToString(), i);
            i++;
        }
        return items;
    }



    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetProductByMenufracturer(string prefixText, string contextKey)
    {
        int manufac = 0;
        if (contextKey != "")
        {
            string[] St = new string[] { };
            DataTable dT = new DataTable();
            string[] mainstrnin = contextKey.Split(',');
            manufac = Convert.ToInt32(mainstrnin[0]);
        }
        string sql =
            "SELECT tblProduct.ProductCode+':'+tblProduct.ProductName AS Product FROM tblProduct left join tblUnitPrice on tblProduct.ProductCode=tblUnitPrice.ProductCode WHERE  (tblProduct.ProductCode+':'+tblProduct.ProductName) like @prefixText  and tblProduct.CompanyId ='" +
            manufac + "' AND IsActive=1 AND ActionStatus='Accepted'";
        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Product"].ToString(), i);
            i++;
        }
        return items;
    }



    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod]
    public string[] GetInvoiceCustomer(string prefixText)
    {
        //string param = "";
        //if (Session["MOParam"] != null)
        //{
        //    param = Session["MOParam"].ToString();
        //}

        string sql = "";

        sql = @"SELECT DISTINCT CSTMR.CustomerCode + ':' + CSTMR.CustomerName + ':( Region- ' + RGN.RegionName + '; Area - ' + ARA.AreaName + '; Territory - ' + TTR.TerritoryName + '; Market - ' + MKT.MarketName +' )'  + ':' +CONVERT(NVARCHAR(MAX),CSTMR.CustomerMasterId)Cust FROM tblInvoice AS INV 
                LEFT JOIN dbo.tblCustMaster AS CSTMR ON CSTMR.CustomerMasterId = INV.CustomerMasterId
                LEFT JOIN tblMarket AS MKT ON MKT.MarketId = CSTMR.MarketId
                LEFT JOIN dbo.tblTerritory AS TTR ON MKT.TerritoryId = TTR.TerritoryId
                LEFT JOIN dbo.tblArea AS ARA ON ARA.AreaId = TTR.AreaId
                LEFT JOIN dbo.tblRegion AS RGN ON RGN.RegionId = ARA.RegionId
                WHERE CSTMR.IsActive = 1 AND CSTMR.CustomerCode + ':' + CSTMR.CustomerName+':'+CONVERT(NVARCHAR(MAX),CSTMR.CustomerMasterId)
		        LIKE  @prefixText ";

        SqlDataAdapter da = new SqlDataAdapter(sql, strConnectionString);
        da.SelectCommand.Parameters.Add("@prefixText", System.Data.SqlDbType.VarChar, 50).Value = "%" + prefixText + "%";
        DataTable DTLocal = new DataTable();
        da.Fill(DTLocal);
        string[] items = new string[DTLocal.Rows.Count];
        int i = 0;
        foreach (DataRow dr in DTLocal.Rows)
        {
            items.SetValue(dr["Cust"].ToString(), i);
            i++;
        }
        return items;
    }


    // Dashboard

    static SalesDashboardDal  aDashboardDal = new SalesDashboardDal();

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static CompanyWiseSalesDao[] GetCompanyWiseSales(string companyId, string year, string month)
    {
        DataTable aTable = aDashboardDal.GetCompanyWiseSales(companyId, year, month);

        List<CompanyWiseSalesDao> aList = new List<CompanyWiseSalesDao>();

        if (aTable.Rows.Count > 0)
        {
            var aData = new CompanyWiseSalesDao();

            foreach (DataRow DR in aTable.Rows)
            {
                aData = new CompanyWiseSalesDao();

                aData.InvoiceDate = DR["InvoiceDate"].ToString();
                aData.SalesValue = Convert.ToDecimal(DR["SalesValue"].ToString());

                aList.Add(aData);

            }

            return aList.ToArray();
        }

        return aList.ToArray();
    }
}



