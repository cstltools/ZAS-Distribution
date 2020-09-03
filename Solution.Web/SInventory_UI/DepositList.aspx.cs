using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DepositList : System.Web.UI.Page
{
    CompanywisebranchDal aDal = new CompanywisebranchDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Loaddropdownlist();
        }
    }

    private void Loaddropdownlist()
    {
        aDal.LoadCompany(companyNameDropDownList);
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Loaddropdownlist();
        fromDateTextBox.Text = "";
        toDateTextBox.Text = "";

        invoiceGridView.DataSource = null;
        invoiceGridView.DataBind();
    }

    protected void Button_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            DataTable aTable = aDal.LoadDepositList(GenerateParameter());

            if (aTable.Rows.Count > 0)
            {
                invoiceGridView.DataSource = aTable;
                invoiceGridView.DataBind();
            }
            else
            {
                invoiceGridView.DataSource = null;
                invoiceGridView.DataBind();
            }
        }
    }

    private bool Validation()
    {
       
        
        if (fromDateTextBox.Text == "")
        {
            ShowMessageBox("Please select from date !!");
            return false;
        }

        if (toDateTextBox.Text == "")
        {
            ShowMessageBox("Please select to date !!");
            return false;
        }

        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please select company !!");
            return false;
        }

        return true;
    }

    private string GenerateParameter()
    {
        string param = "";

        if (fromDateTextBox.Text.Trim() != "")
        {
            if (toDateTextBox.Text.Trim() != "")
            {
                param = param + " AND DP.DepositDate BETWEEN '" +
                        fromDateTextBox.Text.Trim() + "' AND '" + toDateTextBox.Text.Trim() + "'";
            }
        }

        if (companyNameDropDownList.SelectedValue == "")
        {
            param = param + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        return param;
    }

    protected void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string depositId = invoiceGridView.DataKeys[rowindex][0].ToString();

            if (depositId != null)
            {
                CompanyWiseDepositDao aDao = new CompanyWiseDepositDao();

                aDao.IsDelete = true;
                aDao.DeleteBy = Session["LoginName"].ToString();
                aDao.DeleteDate = DateTime.Now;
                aDao.DepositId = Convert.ToInt32(depositId);

                aDal.UpdateDepositInfo(aDao);
            }

            Button_Click(null,null);

        }
    }
}