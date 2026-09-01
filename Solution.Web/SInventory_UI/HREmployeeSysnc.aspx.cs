using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_MarketInfoView : System.Web.UI.Page
{

    DataTable aDataTable = new DataTable();
    HREmployeeDataSyncDal aDataSyncDal = new HREmployeeDataSyncDal();
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            LoadHREmployee();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(ddlCompany, groupDropDownList.SelectedValue);
        }
        else
        {
            ddlCompany.Items.Clear();

        }


        if (Session["IsHead"].ToString() == "True")
        {
            DataTable aTable = aReportDal.GetUserCompanyById(Session["UserId"].ToString());

            if (aTable.Rows.Count > 0)
            {
                ddlCompany.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
            }
        }



        //if (Session["UserType"].ToString() != "Admin")
        //{
        //    ddlCompany.Enabled = false;
        //}
        //else
        //{
        //    ddlCompany.Enabled = true;
        //}

    }

    protected void gotoinvoiceButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow currentRow = (GridViewRow)button.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        if (aDataSyncDal.SyncEmployeById(loadGridView.DataKeys[rowindex]["EmpMasterCode"].ToString()))
        {
            ShowMessageBox("Synced Succesfully!");
            LoadHREmployee();
        }

    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void LoadHREmployee()
    {
        aDataTable = aDataSyncDal.GetEmployeeDataFromHr(GeneratePram());
        loadGridView.DataSource = aDataTable;
        loadGridView.DataBind();

        for (int i = 0; i < aDataTable.Rows.Count; i++)
        {
            var gotoinvoiceButton = (Button)loadGridView.Rows[i].Cells[1].FindControl("gotoinvoiceButton");
            var wingsSwapButton = (Button)loadGridView.Rows[i].Cells[1].FindControl("wingsSwapButton");

            if (loadGridView.DataKeys[i]["SyncStatus"].ToString() == "Not Synced")
            {
                gotoinvoiceButton.Enabled = true;
                gotoinvoiceButton.Text = "Proced to Sync ";
                gotoinvoiceButton.ToolTip = "";

                gotoinvoiceButton.Enabled = true;
                gotoinvoiceButton.Text = "Proced to Sync ";
                gotoinvoiceButton.ToolTip = "";
            }
            else
            {
                gotoinvoiceButton.Visible = false;
                
            }

            if (loadGridView.DataKeys[i]["CompanySwap"].ToString() == "Yes")
            {
                wingsSwapButton.Enabled = true;
                wingsSwapButton.Text = "Proced Change >> ";
                wingsSwapButton.ToolTip = "";

               
            }
            else
            {
                wingsSwapButton.Visible = false;

            }
        }
    }

    private string GeneratePram()
    {
        string pram = "";


        if (ddlCompany.SelectedValue != "")
        {
            pram = pram + " AND UNT.UnitId = " + ddlCompany.SelectedValue;
        }
        
        if (empCodeTextBox.Text.Trim() != "")
        {
            pram = pram + " AND EGI.EmpMasterCode = '" + empCodeTextBox.Text.Trim() + "'";
        }  

        return pram;
    }

    

   

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string marketId = loadGridView.DataKeys[rowindex][0].ToString();
            PopUp(marketId);
        }
    }

    private void PopUp(string Id)
    {
        string url = "MarketInfoEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    protected void marketNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MarketInfoEntry.aspx");
    }
    protected void marketReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        LoadHREmployee();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        LoadHREmployee();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("HREmployeeSysnc.aspx");

    }

    protected void wingsSwapButton_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        GridViewRow currentRow = (GridViewRow)button.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        if (aDataSyncDal.SyncCompanyEmployeById(loadGridView.DataKeys[rowindex]["EmpMasterCode"].ToString()))
        {
            ShowMessageBox("Synced Succesfully!");
            LoadHREmployee();
        }
    }
}