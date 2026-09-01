using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class SInventory_UI_DBBackup : System.Web.UI.Page
{//Metioned here your database name
    string dbname = "test1";
    SqlConnection sqlcon = new SqlConnection();
    SqlCommand sqlcmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //Mentioned Connection string make sure that user id and password sufficient previlages
        sqlcon.ConnectionString = @"Server=182.163.127.238\MSSQLSERVER2014;database=SalesrollDB_ZAS;uid=sa;pwd=sa1234#;";

        //Enter destination directory where backup file stored
        string destdir = "D:\\Backupdb";

        //Check that directory already there otherwise create 
        if (!System.IO.Directory.Exists(destdir))
        {
            System.IO.Directory.CreateDirectory("D:\\Backupdb");
        }
        try
        {
            //Open connection
            sqlcon.Open();
            //query to take backup database
            sqlcmd = new SqlCommand("backup database SalesRollDB_ZAS to disk='" + destdir + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", sqlcon);
            sqlcmd.ExecuteNonQuery();
            //Close connection
            sqlcon.Close();
            Response.Write("Backup database successfully");
        }
        catch (Exception ex)
        {
            Response.Write("Error During backup database!");
        }
    }
}