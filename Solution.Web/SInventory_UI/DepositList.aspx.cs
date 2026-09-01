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
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DepositList : System.Web.UI.Page
{
    DepositSlipEntryDal aDal = new DepositSlipEntryDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Loaddropdownlist();
            SetNewEntryVisibility();
        }
    }

    private void SetNewEntryVisibility()
    {
        if (Session["UserId"] != null || Session["UserId"].ToString() != "")
        {
            if (Convert.ToInt32(Session["UserId"]) == 9 || Convert.ToInt32(Session["UserId"]) == 10)
            {
                entry.Visible = false;
            }
            else
            {
                entry.Visible = true;
            }
        }
    }

    private void Loaddropdownlist()
    {
        aDal.LoadManufac(companyNameDropDownList);
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aDal.GetDcInformationByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());

                }
                else
                {
                    aDal.GetDCInformation(dcDropDownList, companyNameDropDownList.SelectedValue);
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        else
        {
            dcDropDownList.Items.Clear();
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Loaddropdownlist();
        dcDropDownList.Items.Clear();
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

                for (int i = 0; i < invoiceGridView.Rows.Count; i++)
                {
                    if (Session["LoginName"].ToString() != "rezaul")
                    {
                        var gotoinvoiceButton = (ImageButton)invoiceGridView.Rows[i].Cells[1].FindControl("editImageButton");

                        gotoinvoiceButton.Enabled = false;
                        gotoinvoiceButton.BackColor = Color.DarkRed;
                        gotoinvoiceButton.ToolTip = "You're not authorised for this operation !!!";
                    }
                }
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
        //if (companyNameDropDownList.SelectedValue == "")
        //{
        //    ShowMessageBox("Please select company !!");
        //    return false;
        //}

        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserType"].ToString() != "Admin" && Session["IsHead"].ToString() != "True")
            {
                if (dcDropDownList.SelectedValue == "")
                {
                    ShowMessageBox("Please Select DC !!");
                    return false;
                }
            }
        }

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

        if (companyNameDropDownList.SelectedValue != "")
        {
            param = param + " AND CI.CompanyId = '" + companyNameDropDownList.SelectedValue + "'";
        }

        return param;
    }

    private void ShowMessageBox(string message)
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


    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DepositSlipEntry.aspx");
    }

    protected void reportButton_Click(object sender, EventArgs e)
    {
        Session["ComId"] = "";
        Session["UnitId"] = "";
        Session["FDate"] = "";
        Session["TDate"] = "";

        if (companyNameDropDownList.SelectedValue != "")
        {
            Session["ComId"] = companyNameDropDownList.SelectedValue;
        }

        if (dcDropDownList.SelectedValue != "")
        {
            Session["UnitId"] = dcDropDownList.SelectedValue;
        }

        if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
        {
            Session["FDate"] = fromDateTextBox.Text.Trim();
            Session["TDate"] = toDateTextBox.Text.Trim();
        }


        string url = "../SInventory_RPTVIEW/DepositReportViewer.aspx";
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
}