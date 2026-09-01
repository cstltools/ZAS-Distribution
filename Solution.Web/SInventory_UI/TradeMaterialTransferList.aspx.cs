using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_TradeMaterialTransferList : System.Web.UI.Page
{
    private readonly TradeMaterialTransferDal aTradeMaterialTransferDal = new TradeMaterialTransferDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBusinessUnits();
            LoadGrid();
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
    }

    private void LoadGrid()
    {
        DataTable aDataTable = aTradeMaterialTransferDal.GetTradeMaterialTransferList(
            fromDateTextBox.Text.Trim(),
            toDateTextBox.Text.Trim(),
            fromBusinessUnitDropDownList.SelectedValue,
            toBusinessUnitDropDownList.SelectedValue,
            rmNameTextBox.Text.Trim());

        tradeMaterialTransferGridView.DataSource = aDataTable;
        tradeMaterialTransferGridView.DataBind();
    }

    private bool Validation()
    {
        DateTime dateTime;

        if (fromDateTextBox.Text.Trim() != string.Empty &&
            !DateTime.TryParseExact(fromDateTextBox.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) &&
            !DateTime.TryParse(fromDateTextBox.Text.Trim(), out dateTime))
        {
            showMessageBox("Please insert valid from date.");
            fromDateTextBox.Focus();
            return false;
        }

        if (toDateTextBox.Text.Trim() != string.Empty &&
            !DateTime.TryParseExact(toDateTextBox.Text.Trim(), "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) &&
            !DateTime.TryParse(toDateTextBox.Text.Trim(), out dateTime))
        {
            showMessageBox("Please insert valid to date.");
            toDateTextBox.Focus();
            return false;
        }

        return true;
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        if (!Validation())
        {
            return;
        }

        LoadGrid();
    }

    protected void clearButton_Click(object sender, EventArgs e)
    {
        fromDateTextBox.Text = string.Empty;
        toDateTextBox.Text = string.Empty;
        rmNameTextBox.Text = string.Empty;

        if (fromBusinessUnitDropDownList.Items.Count > 0)
        {
            fromBusinessUnitDropDownList.SelectedIndex = 0;
        }

        if (toBusinessUnitDropDownList.Items.Count > 0)
        {
            toBusinessUnitDropDownList.SelectedIndex = 0;
        }

        LoadGrid();
    }

    protected void newLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("TradeMaterialTransfer.aspx");
    }

    protected void tradeMaterialTransferGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            string tradeMaterialTransferId = tradeMaterialTransferGridView.DataKeys[rowIndex][0].ToString();
            Response.Redirect("TradeMaterialTransfer.aspx?ID=" + tradeMaterialTransferId);
        }

        if (e.CommandName == "DeleteData")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int tradeMaterialTransferId = Convert.ToInt32(tradeMaterialTransferGridView.DataKeys[rowIndex][0].ToString());

            DataTable checkDt = aTradeMaterialTransferDal.GetTradeMaterialTransferById(tradeMaterialTransferId);
            if (checkDt.Rows.Count > 0 && checkDt.Rows[0]["ApprovalStatus"].ToString() == "Approved")
            {
                showMessageBox("Approved trade material transfer cannot be deleted.");
                return;
            }

            if (aTradeMaterialTransferDal.DeleteTradeMaterialTransfer(tradeMaterialTransferId))
            {
                showMessageBox("Trade material transfer deleted successfully.");
                LoadGrid();
            }
            else
            {
                showMessageBox("Trade material transfer delete failed.");
            }
        }

        if (e.CommandName == "ViewDetails")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int tradeMaterialTransferId = Convert.ToInt32(tradeMaterialTransferGridView.DataKeys[rowIndex][0].ToString());
            ShowDetails(tradeMaterialTransferId);
        }
    }

    private void ShowDetails(int id)
    {
        selectedTmtIdHiddenField.Value = id.ToString();
        DataTable dt = aTradeMaterialTransferDal.GetTradeMaterialTransferById(id);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            lblDetailId.Text = row["TransferCode"].ToString();
            lblDetailFromBU.Text = row["FromCompanyName"].ToString();
            lblDetailFromLoc.Text = row["FromLocationName"].ToString();
            lblDetailFGName.Text = row["FromProductName"].ToString() + " (" + row["FromProductCode"].ToString() + ")";
            lblDetailToBU.Text = row["ToCompanyName"].ToString();
            lblDetailToLoc.Text = row["ToLocationName"].ToString();
            lblDetailRMName.Text = row["ToItemDescription"].ToString() + " (" + row["ToItemDescriptionCode"].ToString() + ")";
            lblDetailQty.Text = Convert.ToDecimal(row["TransferQty"]).ToString("0.00");
            lblDetailRemarks.Text = string.IsNullOrEmpty(row["Remarks"].ToString()) ? "No remarks" : row["Remarks"].ToString();
            lblDetailEntryBy.Text = row["EntryBy"].ToString();
            lblDetailEntryDate.Text = Convert.ToDateTime(row["EntryDate"]).ToString("dd-MMM-yyyy hh:mm tt");
                        string status = row["ApprovalStatus"].ToString();
            if (status == "Approved")
            {
                lblDetailStatus.Text = "Approved";
                lblDetailStatus.CssClass = "badge badge-approved";
                approvedByRow.Visible = true;
                lblDetailApprovedBy.Text = row["ApprovedBy"].ToString();
                lblDetailApprovedDate.Text = row["ApprovedDate"] != DBNull.Value ? Convert.ToDateTime(row["ApprovedDate"]).ToString("dd-MMM-yyyy hh:mm tt") : "";
            }
            else
            {
                lblDetailStatus.Text = "Pending";
                lblDetailStatus.CssClass = "badge badge-pending";
                approvedByRow.Visible = false;
            }
            
            ModalPopupExtender1.Show();
        }
    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
}
