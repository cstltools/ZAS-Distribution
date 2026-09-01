using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_TradeMaterialTransfer : System.Web.UI.Page
{
    private readonly TradeMaterialTransferDal aTradeMaterialTransferDal = new TradeMaterialTransferDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBusinessUnits();
            ClearTransactionFields();
            LoadTradeMaterialTransferForEdit();
        }
    }

    private void LoadBusinessUnits()
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        int userId = Convert.ToInt32(Session["UserId"].ToString());
        int moduleId = 0;

        aTradeMaterialTransferDal.LoadBusinessUnit(fromBusinessUnitDropDownList, userId, moduleId);
        aTradeMaterialTransferDal.LoadBusinessUnit(toBusinessUnitDropDownList, userId, moduleId);

        if (fromBusinessUnitDropDownList.Items.Count == 2)
        {
            fromBusinessUnitDropDownList.SelectedIndex = 1;
            LoadFromDependentData();
        }

        if (toBusinessUnitDropDownList.Items.Count == 2)
        {
            toBusinessUnitDropDownList.SelectedIndex = 1;
            LoadToDependentData();
        }
    }

    private void LoadFromDependentData()
    {
        fromLocationDropDownList.Items.Clear();
        fgDropDownList.Items.Clear();
        fromCurrentStockQtyTextBox.Text = string.Empty;

        if (fromBusinessUnitDropDownList.SelectedValue != "")
        {
            aTradeMaterialTransferDal.LoadLocation(fromLocationDropDownList, fromBusinessUnitDropDownList.SelectedValue);
            aTradeMaterialTransferDal.LoadFgByCompany(fgDropDownList, Convert.ToInt32(fromBusinessUnitDropDownList.SelectedValue));
        }
    }

    private void LoadToDependentData()
    {
        toLocationDropDownList.Items.Clear();
        aTradeMaterialTransferDal.LoadToLocation(toLocationDropDownList, toBusinessUnitDropDownList.SelectedValue);
    }

    private void LoadFgCurrentStock()
    {
        fromCurrentStockQtyTextBox.Text = string.Empty;

        if (fgDropDownList.SelectedValue != "" && fromLocationDropDownList.SelectedValue != "")
        {
            DataTable aDataTable = aTradeMaterialTransferDal.GetFgCurrentStock(Convert.ToInt32(fgDropDownList.SelectedValue), Convert.ToInt32(fromLocationDropDownList.SelectedValue));

            if (aDataTable.Rows.Count > 0)
            {
                fromCurrentStockQtyTextBox.Text = aDataTable.Rows[0]["CurrentStock"].ToString();
            }
            else
            {
                fromCurrentStockQtyTextBox.Text = "0";
            }
        }
    }

    private void LoadRmCurrentStock()
    {
        if (rmIdHiddenField.Value != "" && toBusinessUnitDropDownList.SelectedValue != "" && toLocationDropDownList.SelectedValue != "")
        {
            aTradeMaterialTransferDal.GetRawMaterialCurrentStock(
                Convert.ToInt32(toBusinessUnitDropDownList.SelectedValue),
                Convert.ToInt32(toLocationDropDownList.SelectedValue),
                Convert.ToInt32(rmIdHiddenField.Value));
        }
    }

    private void ResolveRawMaterial()
    {
        rmIdHiddenField.Value = string.Empty;

        if (rmNameTextBox.Text.Trim() == "")
        {
            return;
        }

        string rawMaterialText = rmNameTextBox.Text.Trim();

        if (rawMaterialText.Contains(":"))
        {
            string[] data = rawMaterialText.Split(new[] { ':' }, 2);
            rawMaterialText = data[0] + ":" + data[1];
        }

        DataTable aDataTable = aTradeMaterialTransferDal.GetRawMaterialInfo(rawMaterialText);

        if (aDataTable.Rows.Count > 0)
        {
            rmIdHiddenField.Value = aDataTable.Rows[0]["ItemDescriptionId"].ToString();
            rmNameTextBox.Text = aDataTable.Rows[0]["ItemDescriptionCode"] + ":" + aDataTable.Rows[0]["ItemDescription"];
            LoadRmCurrentStock();
        }
        else
        {
            rmNameTextBox.Text = string.Empty;
            showMessageBox("Raw material does not exist.");
        }
    }

    private bool Validation()
    {
        if (fromBusinessUnitDropDownList.SelectedValue == "")
        {
            ShowValidationMessage(fromBusinessUnitDropDownList, "Please select from business unit.");
            return false;
        }

        if (fromLocationDropDownList.SelectedValue == "")
        {
            ShowValidationMessage(fromLocationDropDownList, "Please select from location.");
            return false;
        }

        if (fgDropDownList.SelectedValue == "")
        {
            ShowValidationMessage(fgDropDownList, "Please select FG.");
            return false;
        }

        if (toBusinessUnitDropDownList.SelectedValue == "")
        {
            ShowValidationMessage(toBusinessUnitDropDownList, "Please select to business unit.");
            return false;
        }

        if (toLocationDropDownList.SelectedValue == "")
        {
            ShowValidationMessage(toLocationDropDownList, "Please select to location.");
            return false;
        }

        if (rmIdHiddenField.Value == "")
        {
            ShowValidationMessage(rmNameTextBox, "Please select RM.");
            return false;
        }

        decimal transferQty;
        if (!decimal.TryParse(transferQtyTextBox.Text.Trim(), out transferQty) || transferQty <= 0)
        {
            ShowValidationMessage(transferQtyTextBox, "Please insert valid transfer quantity.");
            return false;
        }

        decimal currentStockQty;
        decimal.TryParse(fromCurrentStockQtyTextBox.Text.Trim(), out currentStockQty);
        if (transferQty > currentStockQty)
        {
            ShowValidationMessage(transferQtyTextBox, "Transfer quantity cannot be greater than current FG stock.");
            return false;
        }

        return true;
    }

    private void ShowValidationMessage(WebControl control, string message)
    {
        control.Focus();
        control.BackColor = Color.GhostWhite;
        showMessageBox(message);
    }

    private void ClearTransactionFields()
    {
        tradeMaterialTransferIdHiddenField.Value = string.Empty;
        fromCurrentStockQtyTextBox.Text = string.Empty;
        transferQtyTextBox.Text = string.Empty;
        remarksTextBox.Text = string.Empty;
        rmNameTextBox.Text = string.Empty;
        rmIdHiddenField.Value = string.Empty;
    }

    private void ClearAll()
    {
        tradeMaterialTransferIdHiddenField.Value = string.Empty;

        if (fromBusinessUnitDropDownList.Items.Count > 0)
        {
            fromBusinessUnitDropDownList.SelectedIndex = 0;
        }

        if (toBusinessUnitDropDownList.Items.Count > 0)
        {
            toBusinessUnitDropDownList.SelectedIndex = 0;
        }

        fromLocationDropDownList.Items.Clear();
        toLocationDropDownList.Items.Clear();
        fgDropDownList.Items.Clear();
        ClearTransactionFields();
        LoadBusinessUnits();
        submitButton.Text = "Submit";
        submitButton.BackColor = Color.Empty;
    }

    private static void SetSelectedValueIfExists(ListControl listControl, string selectedValue)
    {
        if (listControl.Items.FindByValue(selectedValue) != null)
        {
            listControl.SelectedValue = selectedValue;
        }
    }

    private void LoadTradeMaterialTransferForEdit()
    {
        if (string.IsNullOrEmpty(Request.QueryString["ID"]))
        {
            return;
        }

        int tradeMaterialTransferId;
        if (!int.TryParse(Request.QueryString["ID"], out tradeMaterialTransferId))
        {
            return;
        }

        DataTable aDataTable = aTradeMaterialTransferDal.GetTradeMaterialTransferById(tradeMaterialTransferId);

        if (aDataTable.Rows.Count == 0)
        {
            showMessageBox("Trade material transfer information not found.");
            return;
        }

        DataRow dataRow = aDataTable.Rows[0];

        tradeMaterialTransferIdHiddenField.Value = dataRow["TradeMaterialTransferId"].ToString();

        SetSelectedValueIfExists(fromBusinessUnitDropDownList, dataRow["FromCompanyId"].ToString());
        LoadFromDependentData();
        SetSelectedValueIfExists(fromLocationDropDownList, dataRow["FromLocationId"].ToString());
        SetSelectedValueIfExists(fgDropDownList, dataRow["FromProductId"].ToString());
        LoadFgCurrentStock();

        SetSelectedValueIfExists(toBusinessUnitDropDownList, dataRow["ToCompanyId"].ToString());
        LoadToDependentData();
        SetSelectedValueIfExists(toLocationDropDownList, dataRow["ToLocationId"].ToString());

        rmIdHiddenField.Value = dataRow["ToItemDescriptionId"].ToString();
        rmNameTextBox.Text = dataRow["ToItemDescriptionCode"] + ":" + dataRow["ToItemDescription"];
        transferQtyTextBox.Text = dataRow["TransferQty"].ToString();
        remarksTextBox.Text = dataRow["Remarks"].ToString();

        submitButton.Text = "Update";
        submitButton.BackColor = Color.DodgerBlue;

        if (dataRow["ApprovalStatus"].ToString() == "Approved")
        {
            submitButton.Visible = false;
            showMessageBox("This trade material transfer has already been approved and cannot be modified.");
            fromBusinessUnitDropDownList.Enabled = false;
            fromLocationDropDownList.Enabled = false;
            fgDropDownList.Enabled = false;
            transferQtyTextBox.ReadOnly = true;
            remarksTextBox.ReadOnly = true;
            toBusinessUnitDropDownList.Enabled = false;
            toLocationDropDownList.Enabled = false;
            rmNameTextBox.ReadOnly = true;
        }
    }

    protected void fromBusinessUnitDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFromDependentData();
    }

    protected void toBusinessUnitDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadToDependentData();
        LoadRmCurrentStock();
    }

    protected void fromLocationDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFgCurrentStock();
    }

    protected void toLocationDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRmCurrentStock();
    }

    protected void fgDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFgCurrentStock();
    }

    protected void rmNameTextBox_TextChanged(object sender, EventArgs e)
    {
        ResolveRawMaterial();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (!Validation())
        {
            return;
        }

        TradeMaterialTransferDao aTradeMaterialTransferDao = new TradeMaterialTransferDao();

        DataTable fgInfoTable = aTradeMaterialTransferDal.GetFgInfo(Convert.ToInt32(fgDropDownList.SelectedValue));
        if (fgInfoTable.Rows.Count == 0)
        {
            showMessageBox("FG information not found.");
            return;
        }

        DataTable rmInfoTable = aTradeMaterialTransferDal.GetRawMaterialInfo(rmNameTextBox.Text.Trim());
        if (rmInfoTable.Rows.Count == 0)
        {
            showMessageBox("RM information not found.");
            return;
        }

        aTradeMaterialTransferDao.TradeMaterialTransferId =
            string.IsNullOrEmpty(tradeMaterialTransferIdHiddenField.Value)
                ? 0
                : Convert.ToInt32(tradeMaterialTransferIdHiddenField.Value);

        aTradeMaterialTransferDao.FromCompanyId = Convert.ToInt32(fromBusinessUnitDropDownList.SelectedValue);
        aTradeMaterialTransferDao.FromLocationId = Convert.ToInt32(fromLocationDropDownList.SelectedValue);
        aTradeMaterialTransferDao.FromProductId = Convert.ToInt32(fgDropDownList.SelectedValue);
        aTradeMaterialTransferDao.FromProductCode = fgInfoTable.Rows[0]["ProductCode"].ToString();
        aTradeMaterialTransferDao.FromProductName = fgInfoTable.Rows[0]["ProductName"].ToString();
        aTradeMaterialTransferDao.FromCurrentStockQty = Convert.ToDecimal(fromCurrentStockQtyTextBox.Text == "" ? "0" : fromCurrentStockQtyTextBox.Text, CultureInfo.InvariantCulture);

        aTradeMaterialTransferDao.ToCompanyId = Convert.ToInt32(toBusinessUnitDropDownList.SelectedValue);
        aTradeMaterialTransferDao.ToLocationId = Convert.ToInt32(toLocationDropDownList.SelectedValue);
        aTradeMaterialTransferDao.ToItemDescriptionId = Convert.ToInt32(rmInfoTable.Rows[0]["ItemDescriptionId"]);
        aTradeMaterialTransferDao.ToItemDescriptionCode = rmInfoTable.Rows[0]["ItemDescriptionCode"].ToString();
        aTradeMaterialTransferDao.ToItemDescription = rmInfoTable.Rows[0]["ItemDescription"].ToString();
        aTradeMaterialTransferDao.ToCurrentStockQty = 0;

        aTradeMaterialTransferDao.TransferQty = Convert.ToDecimal(transferQtyTextBox.Text.Trim(), CultureInfo.InvariantCulture);
        aTradeMaterialTransferDao.FgIssueTransactionType = "Trade Material Transfer FG Issue";
        aTradeMaterialTransferDao.RmReceiveTransactionType = "Trade Material Transfer RM Receive";
        aTradeMaterialTransferDao.Remarks = remarksTextBox.Text.Trim();
        if (aTradeMaterialTransferDao.TradeMaterialTransferId > 0)
        {
            DataTable checkDt = aTradeMaterialTransferDal.GetTradeMaterialTransferById(aTradeMaterialTransferDao.TradeMaterialTransferId);
            if (checkDt.Rows.Count > 0 && checkDt.Rows[0]["ApprovalStatus"].ToString() == "Approved")
            {
                showMessageBox("Approved trade material transfer cannot be updated.");
                return;
            }
            aTradeMaterialTransferDao.UpdateBy = Session["LoginName"] == null ? string.Empty : Session["LoginName"].ToString();
            aTradeMaterialTransferDao.UpdateDate = DateTime.Now;
        }
        else
        {
            aTradeMaterialTransferDao.EntryBy = Session["LoginName"] == null ? string.Empty : Session["LoginName"].ToString();
            aTradeMaterialTransferDao.EntryDate = DateTime.Now;
        }

        string msg = aTradeMaterialTransferDal.SaveTradeMaterialTransfer(aTradeMaterialTransferDao);

        if (msg.Contains("successfully"))
        {
           

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "alert",
                "alert('" + msg.Replace("'", "\\'") + "');window.location='TradeMaterialTransferList.aspx';",
                true);
            return;
        }

        showMessageBox(msg);
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    protected void listButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("TradeMaterialTransferList.aspx");
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    [WebMethod]
    [ScriptMethod]
    public static string[] GetRawMaterial(string prefixText)
    {
        TradeMaterialTransferDal aTradeMaterialTransferDal = new TradeMaterialTransferDal();
        return aTradeMaterialTransferDal.SearchRawMaterial(prefixText);
    }
}
