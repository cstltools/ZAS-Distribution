using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DirectStockOut : System.Web.UI.Page
{
    WingsWiseSalesReportDal aReportDal = new WingsWiseSalesReportDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);

        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);


        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();

        }

    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {

                if (Session["UserId"].ToString() == 2.ToString(CultureInfo.InvariantCulture))
                {
                    aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
                }


                //if (Session["UserType"].ToString() != "Admin")
                //{
                //    aReportDal.LoadDcInfoByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                //}
                //else
                //{
                //    aReportDal.LoadDcInfo(dcDropDownList, companyNameDropDownList.SelectedValue);
                //}
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

    protected void searchButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {

            DataTable aTable = aReportDal.GetDcStockInfo(dcDropDownList.SelectedValue);

            if (aTable.Rows.Count > 0)
            {
                orderGridView.DataSource = aTable;
                orderGridView.DataBind();
            }
            else
            {
                orderGridView.DataSource = null;
                orderGridView.DataBind();
            }
        }
    }

    private bool Validation()
    {

        if (groupDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select group Name !!");
            return false;
        }

        if (Session["UserType"].ToString() != "")
        {
            if (Session["UserId"].ToString() == 2.ToString(CultureInfo.InvariantCulture))
            {
                if (dcDropDownList.SelectedValue == "")
                {
                    ShowMessageBox("Please Select DC !!");
                    return false;
                }
            }
        }


        return true;
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        LoadDropdownList();
        dcDropDownList.Items.Clear();
        orderGridView.DataSource = null;
        orderGridView.DataBind();

    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var chkBoxHeader = (CheckBox)orderGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");

            chkBoxRows.Checked = chkBoxHeader.Checked;
        }
    }

    protected void submitButton_OnClick(object sender, EventArgs e)
    {
        if (SaveValidation())
        {
            DirectStockOutDao aDirectStockOutDao;
            bool id = false;
            string code = "";
           

            DataTable aTable = aReportDal.GenerateCode();

            if (aTable.Rows.Count > 0)
            {
                code = (Convert.ToInt32(aTable.Rows[0].Field<string>("StockOutCode")) + 1).ToString(CultureInfo.InvariantCulture);
                
            }
            else
            {
                code = "10001";
            }

            for (int i = 0; i < orderGridView.Rows.Count; i++)
            {
                aDirectStockOutDao = new DirectStockOutDao();

                var chkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");
                var qtyBoxRows = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("quantityTextBox");
                var reasonBoxRows = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("reasonTextBox");

                if (chkBoxRows.Checked)
                {
                    aDirectStockOutDao.Code = code;
                    aDirectStockOutDao.DCId = Convert.ToInt32(dcDropDownList.SelectedValue);
                    aDirectStockOutDao.ProductCode = orderGridView.Rows[i].Cells[1].Text.Trim();
                    aDirectStockOutDao.ProductName = orderGridView.Rows[i].Cells[2].Text.Trim();
                    aDirectStockOutDao.StockOutDate = DateTime.Now;
                    aDirectStockOutDao.DCStoreId = Convert.ToInt32(orderGridView.DataKeys[i]["DCStoreId"]);
                    aDirectStockOutDao.StockOutQty = Convert.ToDecimal(qtyBoxRows.Text);
                    aDirectStockOutDao.StockOutReason = reasonBoxRows.Text;
                    aDirectStockOutDao.EntryBy = Session["LoginName"].ToString();
                    aDirectStockOutDao.EntryDate = DateTime.Now;

                    id = aReportDal.SaveDirectStockOutInfo(aDirectStockOutDao);

                }
                
            }

            if (id)
            {
                ShowMessageBox("Stock out successfully !!!");
                clearButton_OnClick(null,null);
            }
        }
    }

    private bool SaveValidation()
    {

        int count = 0;

        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (chkBoxRows.Checked)
            {
                count ++;
            }

            if (count > 0)
            {
                break;
            }
        }

        if (count == 0)
        {
            ShowMessageBox("You should select at least one product !!!");
            return false;
        }


        for (int i = 0; i < orderGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)orderGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (chkBoxRows.Checked)
            {
                var qtyBoxRows = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("quantityTextBox");
                var reasonBoxRows = (TextBox)orderGridView.Rows[i].Cells[0].FindControl("reasonTextBox");

                if (qtyBoxRows.Text == "")
                {
                    ShowMessageBox("You should enter stock out quantity !!!");
                    qtyBoxRows.Focus();
                    qtyBoxRows.BackColor = Color.Lavender;
                    return false;
                }

                if (reasonBoxRows.Text == "")
                {
                    ShowMessageBox("You should enter stock out reason !!!");
                    reasonBoxRows.Focus();
                    reasonBoxRows.BackColor = Color.Lavender;
                    return false;
                }
            }
        }

        return true;
    }

    protected void quantityTextBox_OnTextChanged(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow)(((TextBox)sender).Parent.Parent)).RowIndex;
        int currentStock = Convert.ToInt32(orderGridView.Rows[rowIndex].Cells[7].Text);

        if (((TextBox)orderGridView.Rows[rowIndex].Cells[5].FindControl("quantityTextBox")).Text != "")
        {
            if (IsInteger(((TextBox)orderGridView.Rows[rowIndex].Cells[8].FindControl("quantityTextBox")).Text))
            {

                int returnQty = Convert.ToInt32(((TextBox)orderGridView.Rows[rowIndex].Cells[8].FindControl("quantityTextBox")).Text);

                if (returnQty != 0)
                {
                    if (returnQty > currentStock)
                    {
                        ShowMessageBox("Stock out quantity must be equal or less than  stock quantity!!");
                        ((TextBox)orderGridView.Rows[rowIndex].Cells[5].FindControl("quantityTextBox")).Text =
                            string.Empty;
                    }
                }
                else
                {
                    ShowMessageBox("Stock out quantity must be greater than zero(0) !!");
                    ((TextBox)orderGridView.Rows[rowIndex].Cells[5].FindControl("quantityTextBox")).Text =
                        string.Empty;
                }

            }

            else
            {
                ShowMessageBox("Please select a valid value for stock out quantity!!");
                ((TextBox)orderGridView.Rows[rowIndex].Cells[5].FindControl("quantityTextBox")).Text = string.Empty;
            }
        }
    }

    readonly Regex _regex = new Regex(@"[0-9]");

    private bool IsInteger(string str)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            if (!_regex.IsMatch(str))
            {
                return false;
            }

            return true;

        }
        catch (Exception ex)
        {
            ShowMessageBox(ex.Message);
        }

        return false;
    }

   
}