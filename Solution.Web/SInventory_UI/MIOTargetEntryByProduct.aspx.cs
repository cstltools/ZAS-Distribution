using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MIOTargetEntryByProduct : System.Web.UI.Page
{
    // footer totals (recalculated per postback; not stored in ViewState)
    private decimal TotalQty = 0m;
    private decimal TotalAmount = 0m;

    MiaTargetDAL aReportDal = new MiaTargetDAL();

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
            mioDropDownList.Items.Clear();
        }
    }

    private void Clear()
    {
        companyNameDropDownList.SelectedValue = "";
        periodDropDownList.SelectedIndex = 0;
        mioDropDownList.Items.Clear();

        loadGridView.DataSource = null;
        loadGridView.DataBind();
    }

    protected void showMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Company!!");
            return false;
        }

        if (mioDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select MIA!!");
            return false;
        }

        // at least one selected
        int count = 0;
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            if (chkBoxRows != null && chkBoxRows.Checked)
            {
                count++;
                break;
            }
        }

        if (count == 0)
        {
            showMessageBox("You should select at least one product !!!");
            return false;
        }

        // qty must exist for checked rows
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            if (chkBoxRows != null && chkBoxRows.Checked)
            {
                TextBox tbQty = (TextBox)loadGridView.Rows[i].FindControl("returnQtyTextBox");
                if (tbQty == null || tbQty.Text.Trim() == "")
                {
                    if (tbQty != null) tbQty.Focus();
                    showMessageBox("You should select target quantity !!!");
                    return false;
                }
            }
        }

        return true;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (!Validation())
        {
            showMessageBox("Please input data in all Textbox !!!");
            return;
        }

        aReportDal.DeleteExistingData(DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
            periodDropDownList.SelectedValue, mioDropDownList.SelectedValue);

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            if (chkBoxRows == null || !chkBoxRows.Checked) continue;

            MIOTargetProductWise aMiaTarget = new MIOTargetProductWise
            {
                MiaId = Convert.ToInt32(mioDropDownList.SelectedValue),
                CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                Period = periodDropDownList.SelectedItem.Text.Trim(),
                Year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
                ProductId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                TargetQty = Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("returnQtyTextBox")).Text),
                TargetAmount = Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("tbxTargetAmount")).Text)
            };

            aReportDal.SaveMiaTargetProductWise(aMiaTarget);
        }

        Clear();
    }

    protected void miaViewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MiaTargetView.aspx");
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            mioDropDownList.Items.Clear();
            loadGridView.DataSource = null;
            loadGridView.DataBind();
            return;
        }

        aReportDal.LoadMIOInformation(mioDropDownList, companyNameDropDownList.SelectedValue);

        DataTable aTable = aReportDal.GetActiveProduct(companyNameDropDownList.SelectedValue);

        loadGridView.DataSource = (aTable != null && aTable.Rows.Count > 0) ? aTable : null;
        loadGridView.DataBind();

        // footer totals reset
        RecalcFooterTotals();
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void periodDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (mioDropDownList.SelectedValue == "")
        {
            periodDropDownList.SelectedIndex = 0;
            showMessageBox("Please select MIO !!!");
            return;
        }

        // because first item is "-----Select-----"
        if (periodDropDownList.SelectedIndex == 0)
        {
            // clear selections if needed
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                var chk = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
                var tbQty = (TextBox)loadGridView.Rows[i].FindControl("returnQtyTextBox");

                chk.Checked = false;
                tbQty.Text = "";
                ClearTargetAmountRow(loadGridView.Rows[i]);
            }

            SyncHeaderSelectAll();
            RecalcFooterTotals();
            return;
        }

        DataTable aTable = aReportDal.GetExistingData(
            DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
            periodDropDownList.SelectedValue,
            mioDropDownList.SelectedValue
        );

        // always reset first
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            var chk = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            var tbQty = (TextBox)loadGridView.Rows[i].FindControl("returnQtyTextBox");

            chk.Checked = false;
            tbQty.Text = "";
            ClearTargetAmountRow(loadGridView.Rows[i]);
        }

        if (aTable != null && aTable.Rows.Count > 0)
        {
            // faster lookup: ProductId -> TargetQty
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                int productId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());

                DataRow[] found = aTable.Select("ProductId=" + productId);
                if (found.Length == 0) continue;

                var chk = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
                var tbQty = (TextBox)loadGridView.Rows[i].FindControl("returnQtyTextBox");

                chk.Checked = true;
                tbQty.Text = Convert.ToString(found[0]["TargetQty"]);

                // amount will be calculated from AvgUnitPrice (correct)
                CalculateTargetAmountRow(loadGridView.Rows[i]);
            }
        }

        SyncHeaderSelectAll();
        RecalcFooterTotals();
    }


    // ===================== CHECKBOX + CALC =====================

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var header = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        foreach (GridViewRow row in loadGridView.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;

            var chk = (CheckBox)row.FindControl("chkSelect");
            chk.Checked = header.Checked;

            if (chk.Checked) CalculateTargetAmountRow(row);
            else ClearTargetAmountRow(row);
        }

        RecalcFooterTotals();
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        var chk = (CheckBox)sender;
        var row = (GridViewRow)chk.NamingContainer;

        if (chk.Checked) CalculateTargetAmountRow(row);
        else ClearTargetAmountRow(row);

        SyncHeaderSelectAll();
        RecalcFooterTotals();
    }

    protected void returnQtyTextBox_TextChanged(object sender, EventArgs e)
    {
        var tbQty = (TextBox)sender;
        var row = (GridViewRow)tbQty.NamingContainer;

        var chk = (CheckBox)row.FindControl("chkSelect");
        if (chk != null && chk.Checked)
        {
            CalculateTargetAmountRow(row);
        }
        else
        {
            // qty changed but not checked => keep amount blank
            ClearTargetAmountRow(row);
        }

        RecalcFooterTotals();
    }

    private void CalculateTargetAmountRow(GridViewRow row)
    {
        var tbQty = (TextBox)row.FindControl("returnQtyTextBox");
        var tbAmt = (TextBox)row.FindControl("tbxTargetAmount");
        if (tbQty == null || tbAmt == null) return;

        decimal qty = ToDecimal(tbQty.Text);
        if (qty <= 0)
        {
            tbAmt.Text = "";
            return;
        }

        object dkVal = loadGridView.DataKeys[row.RowIndex].Values["AvgUnitPrice"];
        decimal avgUp = ToDecimal(dkVal);

        tbAmt.Text = (qty * avgUp).ToString("0");
    }

    private void ClearTargetAmountRow(GridViewRow row)
    {
        var tbAmt = (TextBox)row.FindControl("tbxTargetAmount");
        if (tbAmt != null) tbAmt.Text = "";
    }

    // ===================== FOOTER UI =====================

    protected void loadGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Footer TOTAL row: merge first 4 columns (SL + Code + Name + Pack)
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].ColumnSpan = 4;
            e.Row.Cells[0].Text = "TOTAL";
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center; // Qty
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center; // Amount
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center; // checkbox
        }
    }

    private void RecalcFooterTotals()
    {
        decimal totalQty = 0m;
        decimal totalAmt = 0m;

        foreach (GridViewRow row in loadGridView.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;

            var chk = (CheckBox)row.FindControl("chkSelect");
            if (chk == null || !chk.Checked) continue;

            var tbQty = (TextBox)row.FindControl("returnQtyTextBox");
            var tbAmt = (TextBox)row.FindControl("tbxTargetAmount");

            totalQty += ToDecimal(tbQty != null ? tbQty.Text : "0");
            totalAmt += ToDecimal(tbAmt != null ? tbAmt.Text : "0");
        }

        if (loadGridView.FooterRow != null)
        {
            var lblTotalQty = (Label)loadGridView.FooterRow.FindControl("lblTotalQty");
            var lblTotalAmount = (Label)loadGridView.FooterRow.FindControl("lblTotalAmount");

            if (lblTotalQty != null) lblTotalQty.Text = totalQty.ToString("0");
            if (lblTotalAmount != null) lblTotalAmount.Text = totalAmt.ToString("0");
        }
    }

    private void SyncHeaderSelectAll()
    {
        if (loadGridView.HeaderRow == null) return;

        var header = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");
        if (header == null) return;

        bool allChecked = true;
        foreach (GridViewRow row in loadGridView.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;

            var chk = (CheckBox)row.FindControl("chkSelect");
            if (chk == null || !chk.Checked)
            {
                allChecked = false;
                break;
            }
        }
        header.Checked = allChecked;
    }

    private decimal ToDecimal(object val)
    {
        if (val == null || val == DBNull.Value) return 0m;
        string s = Convert.ToString(val).Trim();
        if (s == "") return 0m;
        return Convert.ToDecimal(s);
    }
}
