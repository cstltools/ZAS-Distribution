using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_OtherStockAction : System.Web.UI.Page
{
    private DataTable aDataTable = new DataTable();
    private OtherStockActionBLL aOtherStockActionBLL = new OtherStockActionBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
        }
    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;
            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }
    }

    public void LoadDropDown()
    {
        //aOtherStockActionBLL.LoadmanufacturerName(manufacturerDropDownList);
        //aOtherStockActionBLL.DCLoad(dcDropDownList, Session["UserId"].ToString());

        aOtherStockActionBLL.LoadCompanyDropdownList(companyNameDropDownList);
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            if (Session["UserType"].ToString() != "")
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    aOtherStockActionBLL.LoadDcInformationByUser(dcDropDownList, companyNameDropDownList.SelectedValue, Session["UserId"].ToString());
                }
                else
                {
                    aOtherStockActionBLL.LoadWhInformation(dcDropDownList, companyNameDropDownList.SelectedValue);
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
    private void Load()
    {
        aDataTable = aOtherStockActionBLL.LoadData(Convert.ToInt32(dcDropDownList.SelectedValue),Convert.ToInt32(companyNameDropDownList.SelectedValue));
        if (aDataTable.Rows.Count>0)
        {
            loadGridView.DataSource = aDataTable;
            loadGridView.DataBind();
        }
        else
        {
            showMessageBox("No Data Found!!");
        }
       
    }
    private void Load2()
    {
        aDataTable = aOtherStockActionBLL.LoadData(Convert.ToInt32(dcDropDownList.SelectedValue), Convert.ToInt32(companyNameDropDownList.SelectedValue));
        {
            loadGridView.DataSource = aDataTable;
            loadGridView.DataBind();
        }
       

    }
    protected void submitButton_Click(object sender, EventArgs e)
    {
        Load();
    }
    protected void refreshButton_Click(object sender, EventArgs e)
    {

    }
    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewData")
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            int rowindex = Convert.ToInt32(e.CommandArgument);
            int RowIndex2 = row.RowIndex;
            int bigStore = Convert.ToInt32(loadGridView.Rows[rowindex].Cells[5].Text);

            if (loadGridView.Rows[rowindex].Cells[7].Text != "Restricted" && Session["LoginName"].ToString() != "21900")
            {
            if ((((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) != "" && (((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) != "0")
            {
                if (bigStore < Convert.ToDecimal(((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text))
                {
                    showMessageBox("Return Quantity must be Less then Stock Qty");
                }
                if (bigStore >= Convert.ToDecimal(((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text))
                {
                    int DCStoreFreezeId = Convert.ToInt32(loadGridView.DataKeys[rowindex][0].ToString());
                    int DCStoreId = Convert.ToInt32(loadGridView.DataKeys[rowindex][1].ToString());

                    //Store Freeze  Update
                    DataTable dt = new DataTable();
                    dt = aOtherStockActionBLL.LoadStockData(DCStoreFreezeId);
                    decimal dtstock = Convert.ToDecimal(dt.Rows[0]["StockQty"].ToString());
                    string a = ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text;

                    decimal StockQty = dtstock - Convert.ToDecimal(a.Trim());
                    aOtherStockActionBLL.UpdateDCStoreFreezeStock(StockQty, DCStoreFreezeId);

                    //DC Store Update
                    DataTable dt2 = new DataTable();
                    dt2 = aOtherStockActionBLL.LoadStockDCStockData(DCStoreId);
                    decimal dtdcstock = Convert.ToDecimal(dt2.Rows[0]["StockQty"].ToString());
                    string B = ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text;
                    decimal DCStockQty = dtdcstock + Convert.ToDecimal(B.Trim());
                    aOtherStockActionBLL.UpdateDCStock(DCStockQty, DCStoreId);
                    Load2();
                    showMessageBox("Stock Return Successfully !!");
                }
            }
            else
            {
                showMessageBox("Insert Return Quantity !!");
            }
            }
            if (Session["LoginName"].ToString() == "21900")
            {
                if ((((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) != "" && (((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) != "0")
                {
                    if (bigStore < Convert.ToDecimal(((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text))
                    {
                        showMessageBox("Return Quantity must be Less then Stock Qty");
                    }
                    if (bigStore >= Convert.ToDecimal(((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text))
                    {
                        int DCStoreFreezeId = Convert.ToInt32(loadGridView.DataKeys[rowindex][0].ToString());
                        int DCStoreId = Convert.ToInt32(loadGridView.DataKeys[rowindex][1].ToString());

                        //Store Freeze  Update
                        DataTable dt = new DataTable();
                        dt = aOtherStockActionBLL.LoadStockData(DCStoreFreezeId);
                        decimal dtstock = Convert.ToDecimal(dt.Rows[0]["StockQty"].ToString());
                        string a = ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text;

                        decimal StockQty = dtstock - Convert.ToDecimal(a.Trim());
                        aOtherStockActionBLL.UpdateDCStoreFreezeStock(StockQty, DCStoreFreezeId);

                        //DC Store Update
                        DataTable dt2 = new DataTable();
                        dt2 = aOtherStockActionBLL.LoadStockDCStockData(DCStoreId);
                        decimal dtdcstock = Convert.ToDecimal(dt2.Rows[0]["StockQty"].ToString());
                        string B = ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text;
                        decimal DCStockQty = dtdcstock + Convert.ToDecimal(B.Trim());
                        aOtherStockActionBLL.UpdateDCStock(DCStockQty, DCStoreId);
                        Load2();
                        showMessageBox("Stock Return Successfully !!");
                    }
                }
                else
                {
                    showMessageBox("Insert Return Quantity !!");
                }
            }
        }
    }
    protected void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();

    }
    protected void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }

    protected void submitButton0_OnClick(object sender, EventArgs e)
    {

        if (Validation())
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                int rowindex = Convert.ToInt32(i);
                int RowIndex2 = i;
                int bigStore = Convert.ToInt32(loadGridView.Rows[rowindex].Cells[5].Text);

                CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

                if (ChkBoxRows.Checked)
                {
                    if (loadGridView.Rows[rowindex].Cells[7].Text != "Blocked")
                    {
                        DataTable aTable = aOtherStockActionBLL.CheckPermission(loadGridView.Rows[rowindex].Cells[7].Text,
                            Session["UserId"].ToString());
                        if (aTable.Rows.Count > 0 || loadGridView.Rows[rowindex].Cells[7].Text != "")
                        {
                            if ((((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) !=
                                "" &&
                                (((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) !=
                                "0")
                            {
                                if (bigStore <
                                    Convert.ToDecimal(
                                        ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox"))
                                            .Text))
                                {
                                    showMessageBox("Return Quantity must be Less then Stock Qty");
                                }
                                if (bigStore >=
                                    Convert.ToDecimal(
                                        ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox"))
                                            .Text))
                                {
                                    int DCStoreFreezeId = Convert.ToInt32(loadGridView.DataKeys[rowindex][0].ToString());
                                    int DCStoreId = Convert.ToInt32(loadGridView.DataKeys[rowindex][1].ToString());

                                    //Store Freeze  Update
                                    DataTable dt = new DataTable();
                                    dt = aOtherStockActionBLL.LoadStockData(DCStoreFreezeId);
                                    decimal dtstock = Convert.ToDecimal(dt.Rows[0]["StockQty"].ToString());
                                    string a =
                                        ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox"))
                                            .Text;

                                    decimal StockQty = dtstock - Convert.ToDecimal(a.Trim());
                                    aOtherStockActionBLL.UpdateDCStoreFreezeStock(StockQty, DCStoreFreezeId);

                                    //DC Store Update
                                    DataTable dt2 = new DataTable();
                                    dt2 = aOtherStockActionBLL.LoadStockDCStockData(DCStoreId);
                                    decimal dtdcstock = Convert.ToDecimal(dt2.Rows[0]["StockQty"].ToString());
                                    string B =
                                        ((TextBox)loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox"))
                                            .Text;
                                    decimal DCStockQty = dtdcstock + Convert.ToDecimal(B.Trim());
                                    aOtherStockActionBLL.UpdateDCStock(DCStockQty, DCStoreId);

                                    // Posting Journal

                                    if (aOtherStockActionBLL.FreezeStockReleasePosting(DCStoreFreezeId, Convert.ToDecimal(a.Trim())))
                                    {
                                        showMessageBox("Stock Return Successfully !!");
                                    }

                                }
                            }
                            else
                            {
                                showMessageBox("Insert Return Quantity !!");
                            }
                        }

                    }
                    //else
                    //{
                    //    showMessageBox("Blocked stock can't return !!");
                    //}
                    //if (Session["LoginName"].ToString() == "21900")
                    //{
                    //    if ((((TextBox) loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) != "" &&
                    //        (((TextBox) loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text) != "0")
                    //    {
                    //        if (bigStore <
                    //            Convert.ToDecimal(
                    //                ((TextBox) loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text))
                    //        {
                    //            showMessageBox("Return Quantity must be Less then Stock Qty");
                    //        }
                    //        if (bigStore >=
                    //            Convert.ToDecimal(
                    //                ((TextBox) loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text))
                    //        {
                    //            int DCStoreFreezeId = Convert.ToInt32(loadGridView.DataKeys[rowindex][0].ToString());
                    //            int DCStoreId = Convert.ToInt32(loadGridView.DataKeys[rowindex][1].ToString());

                    //            //Store Freeze  Update
                    //            DataTable dt = new DataTable();
                    //            dt = aOtherStockActionBLL.LoadStockData(DCStoreFreezeId);
                    //            decimal dtstock = Convert.ToDecimal(dt.Rows[0]["StockQty"].ToString());
                    //            string a =
                    //                ((TextBox) loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text;

                    //            decimal StockQty = dtstock - Convert.ToDecimal(a.Trim());
                    //            aOtherStockActionBLL.UpdateDCStoreFreezeStock(StockQty, DCStoreFreezeId);

                    //            //DC Store Update
                    //            DataTable dt2 = new DataTable();
                    //            dt2 = aOtherStockActionBLL.LoadStockDCStockData(DCStoreId);
                    //            decimal dtdcstock = Convert.ToDecimal(dt2.Rows[0]["StockQty"].ToString());
                    //            string B =
                    //                ((TextBox) loadGridView.Rows[RowIndex2].Cells[5].FindControl("returnQtyTextBox")).Text;
                    //            decimal DCStockQty = dtdcstock + Convert.ToDecimal(B.Trim());
                    //            aOtherStockActionBLL.UpdateDCStock(DCStockQty, DCStoreId);

                    //            showMessageBox("Stock Return Successfully !!");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        showMessageBox("Insert Return Quantity !!");
                    //    }
                    //}
                }
            }
        }

        
        Load2();
    }

    private bool Validation()
    {
        InvoiceBLL aInvoiceBll = new InvoiceBLL();
        DataTable aTable = aInvoiceBll.GetFinancialYearInfo(DateTime.Now.Date.ToString(), dcDropDownList.SelectedValue);

        if (aTable.Rows.Count == 0)
        {
            showMessageBox("You should declare financial year !!");
            return false;
        }

        return true;
    }
}