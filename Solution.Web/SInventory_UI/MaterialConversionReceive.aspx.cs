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

public partial class SInventory_UI_MaterialConversionReceive : System.Web.UI.Page
{

    MaterialConversionReceiveDal aReceiveDal = new MaterialConversionReceiveDal();
    WarehouseStockInBll aWarehouseStockInBll = new WarehouseStockInBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownlist();
           
        }
    }

    protected void stockInDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateTime value;

        if (!DateTime.TryParse(stockInDateTextBox.Text, out value))
        {
            stockInDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
    }



    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void challanDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateTime value;

        if (!DateTime.TryParse(challanDateTextBox.Text, out value))
        {
            challanDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
    }

    protected void referenceDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateTime value;

        if (!DateTime.TryParse(referenceDateTextBox.Text, out value))
        {
            referenceDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
    }

    private void LoadDropdownlist()
    {
        //aWarehouseStockInBll.LoadSupplierName(ddlSupplier);
        aReceiveDal.LoadCompany(ddlCompany);
       
    }

    private void LoadPendingMaterialConversionList()
    {
        loadGridView.DataSource = null;
        loadGridView.DataBind();

        if (ddlProductionUnit.SelectedValue != "")
        {
            DataTable aTable = new DataTable();

            aTable = aReceiveDal.LoadMaterialConversionList(GenerateParameter());

            if (aTable.Rows.Count > 0)
            {
                loadGridView.DataSource = aTable;
                loadGridView.DataBind();

                CalculateQuantityVatAndValue();
            }
        }

      

    }

    protected void mfgDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox mfgDateTextBox = (TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("mfgDateTextBox");

        DateTime value;

        if (!DateTime.TryParse(mfgDateTextBox.Text, out value))
        {
            mfgDateTextBox.Text = "";
            showMessageBox("Please insert valid date");
        }
        else
        {
            DateTime dd = Convert.ToDateTime(((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("mfgDateTextBox")).Text);
            ((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("expDateDateTextBox")).Text = (dd.AddYears(2).ToString("dd-MMM-yyyy"));
            //string dateStr = ((TextBox)productGridView.Rows[rowindex].Cells[5].FindControl("mfgDateTextBox")).ToString();
            //DateTime date = new DateTime();
            //if (DateTime.TryParseExact(dateStr, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            //{
            // successfully parsed the string into a DateTime instance =>
            // here we could add the desired number of months to it and construct
            // a new DateTime
            //DateTime newDate = date.AddYears(2);
            //((TextBox)productGridView.Rows[rowindex].Cells[5].FindControl("expDateDateTextBox")).Text = newDate.ToString("dd-MMM-yyyy");
            //}

        }


    }

    protected void expDateDateTextBox_OnTextChanged(object sender, EventArgs e)
    {

        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;


        if (((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("mfgDateTextBox")).Text != "")
        {
            TextBox expDateDateTextBox = (TextBox)loadGridView.Rows[rowindex].Cells[6].FindControl("expDateDateTextBox");
            DateTime value;

            if (!DateTime.TryParse(expDateDateTextBox.Text, out value))
            {
                expDateDateTextBox.Text = "";
                showMessageBox("Please insert valid date");
            }
            DateTime Mdate =
                Convert.ToDateTime(((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("mfgDateTextBox")).Text);
            DateTime ExDate =
              Convert.ToDateTime(((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("expDateDateTextBox")).Text);

            if (ExDate < Mdate)
            {
                ((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("expDateDateTextBox")).Text = string.Empty;
                showMessageBox("Date Error!!");
            }
        }
        else
        {
            showMessageBox("Please Insert Manufacturing Date!!");
            ((TextBox)loadGridView.Rows[rowindex].Cells[5].FindControl("expDateDateTextBox")).Text = string.Empty;
        }
    }


    protected void reqQtyTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        CalculateTotalValue(rowindex);
    }

    protected void costPriceTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        CalculateTotalValue(rowindex);
    }

    private void CalculateTotalValue(int rowindex)
    {
        TextBox quantityTextBox = (TextBox)loadGridView.Rows[rowindex].Cells[7].FindControl("reqQtyTextBox");
        TextBox costPriceTextBox = (TextBox)loadGridView.Rows[rowindex].Cells[8].FindControl("costPriceTextBox");
        TextBox vatTextBox = (TextBox)loadGridView.Rows[rowindex].Cells[9].FindControl("vatTextBox");
        TextBox totalValueTextBox = (TextBox)loadGridView.Rows[rowindex].Cells[10].FindControl("totalValueTextBox");

        decimal totalValue = 0;
        decimal totalVat = 0;
        string vat = vatTextBox.Text;
        string quantity = quantityTextBox.Text;
        string price = costPriceTextBox.Text;

        if (quantity != "" && price != "" && vat != "")
        {
            totalVat = totalVat + (Convert.ToDecimal(quantity) * Convert.ToDecimal(vat));
            totalValue = totalValue + (totalVat + (Convert.ToDecimal(quantity) * Convert.ToDecimal(price)));
            totalValueTextBox.Text = totalValue.ToString(CultureInfo.InvariantCulture);

            CalculateQuantityVatAndValue();
        }

        else
        {
            totalValueTextBox.Text = "";
            CalculateQuantityVatAndValue();
        }
    }

    private void CalculateQuantityVatAndValue()
    {
        decimal totalQuantity = 0; // Changed to decimal
        decimal value = 0;
        decimal totalVat = 0;

        if (loadGridView.Rows.Count > 0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                CheckBox cb = (CheckBox) loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

                if (cb.Checked)
                {

                    // Calculate total quantity

                    if (!string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("reqQtyTextBox")).Text))
                    {
                        Int32 qty = Convert.ToInt32(((TextBox)loadGridView.Rows[i].FindControl("reqQtyTextBox")).Text);
                        totalQuantity += qty;
                    }

                    // Calculate total VAT

                    if (!string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("vatTextBox")).Text) &&
                        !string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("reqQtyTextBox")).Text))
                    {
                        decimal qty = Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("reqQtyTextBox")).Text);
                        decimal vat = Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("vatTextBox")).Text);
                        totalVat += qty * vat;
                    }

                    // Calculate total value

                    if (!string.IsNullOrEmpty(((TextBox)loadGridView.Rows[i].FindControl("totalValueTextBox")).Text))
                    {
                        decimal costPrice = Convert.ToDecimal(((TextBox)loadGridView.Rows[i].FindControl("totalValueTextBox")).Text);
                        value += costPrice;
                    }

                }
                
            }

            // Assign calculated values to textboxes
            totalQtyTextBox.Text = totalQuantity.ToString(CultureInfo.InvariantCulture);
            totalVatTextBox.Text = totalVat.ToString(CultureInfo.InvariantCulture);
            grandTotalTextBox.Text = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (stockInIdHiddenField.Value == "")
            {
                SaveAllData();
            }

            else
            {
                UpdateAllData();
            }
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }


    private bool Validation()
    {
        if (warehouseDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select an Warehouse !!!");
            warehouseDropDownList.Focus();
            warehouseDropDownList.BackColor = Color.GhostWhite;
            return false;
        }

        //if (ddlSupplier.SelectedValue == "")
        //{
        //    showMessageBox("Please Select supplier !!!");
        //    ddlSupplier.Focus();
        //    ddlSupplier.BackColor = Color.GhostWhite;
        //    return false;
        //}

        if (stockInDateTextBox.Text == "")
        {
            showMessageBox("Please insert stock in date !!!");
            stockInDateTextBox.Focus();
            stockInDateTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        if (challanNoTextBox.Text == "")
        {
            showMessageBox("Please insert challan number !!!");
            challanNoTextBox.Focus();
            challanNoTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        if (challanDateTextBox.Text == "")
        {
            showMessageBox("Please insert challan date !!!");
            challanDateTextBox.Focus();
            challanDateTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        if (loadGridView.Rows.Count > 0)
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {

                CheckBox cb = (CheckBox) loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

                if (cb.Checked)
                {
                    if (((TextBox) loadGridView.Rows[i].FindControl("productCodeTextBox")).Text == "")
                    {
                        ((TextBox) loadGridView.Rows[i].FindControl("productCodeTextBox")).Focus();
                        showMessageBox("Please fill out product code!!");
                        return false;
                    }

                    if (((TextBox) loadGridView.Rows[i].FindControl("batchTextBox")).Text == "")
                    {
                        ((TextBox) loadGridView.Rows[i].FindControl("batchTextBox")).Focus();
                        showMessageBox("Please fill out batch!!");
                        return false;
                    }

                    if (((TextBox) loadGridView.Rows[i].FindControl("expDateDateTextBox")).Text == "")
                    {
                        ((TextBox) loadGridView.Rows[i].FindControl("expDateDateTextBox")).Focus();
                        showMessageBox("Please fill out exp. Date!!");
                        return false;
                    }

                    if (((TextBox) loadGridView.Rows[i].FindControl("mfgDateTextBox")).Text == "")
                    {
                        ((TextBox) loadGridView.Rows[i].FindControl("mfgDateTextBox")).Focus();
                        showMessageBox("Please fill out Mfg. Date!!");
                        return false;
                    }

                    if (((TextBox) loadGridView.Rows[i].FindControl("reqQtyTextBox")).Text == "")
                    {
                        ((TextBox) loadGridView.Rows[i].FindControl("reqQtyTextBox")).Focus();
                        showMessageBox("Please fill out Req.Qty!!");
                        return false;
                    }

                    //if (((TextBox)productGridView.Rows[i].FindControl("costPriceTextBox")).Text == "")
                    //{
                    //    ((TextBox)productGridView.Rows[i].FindControl("costPriceTextBox")).Focus();
                    //    showMessageBox("Please fill out Price!!");
                    //    return false;
                    //}

                    if (((TextBox) loadGridView.Rows[i].FindControl("vatTextBox")).Text == "")
                    {
                        ((TextBox) loadGridView.Rows[i].FindControl("vatTextBox")).Focus();
                        showMessageBox("Please fill out vat!!");
                        return false;
                    }
                }

            }
        }
        return true;
    }

    private void SaveAllData()
    {
        int maxRowId;

        WarehouseStockInMasterDao aStockInMasterDao = new WarehouseStockInMasterDao();

        aStockInMasterDao.WHStockInDate = Convert.ToDateTime(stockInDateTextBox.Text.Trim());
        aStockInMasterDao.WarehouseId = Convert.ToInt32(warehouseDropDownList.SelectedValue);

        aStockInMasterDao.SupplierId = string.IsNullOrEmpty(ddlSupplier.SelectedValue) || ddlSupplier.SelectedValue == "0"
    ? 0
    : Convert.ToInt32(ddlSupplier.SelectedValue);


        aStockInMasterDao.TotalVat = Convert.ToDecimal(totalVatTextBox.Text);
        aStockInMasterDao.TotalQuantity = Convert.ToDecimal(totalQtyTextBox.Text);
        aStockInMasterDao.TotalValue = Convert.ToDecimal((grandTotalTextBox.Text).Trim());
        aStockInMasterDao.ChallanNo = (challanNoTextBox.Text).Trim();
        aStockInMasterDao.ChallanDate = Convert.ToDateTime(challanDateTextBox.Text);
        aStockInMasterDao.ReferenceNo = (referenceNoTextBox.Text).Trim();

        if (referenceDateTextBox.Text != "")
        {
            aStockInMasterDao.ReferenceDate = ((referenceDateTextBox.Text).Trim());
        }
        else
        {
            aStockInMasterDao.ReferenceDate = "";
        }

        aStockInMasterDao.Remarks = (remarksTextBox.Text).Trim();
        aStockInMasterDao.Status = "Posted";
        aStockInMasterDao.EntryBy = Session["LoginName"].ToString();
        aStockInMasterDao.EntryDate = DateTime.Now;



        bool stockInSave = aWarehouseStockInBll.SaveStockInMasterInfo(aStockInMasterDao, out maxRowId);

        WharehouseStockInDetailsDao aWharehouseStockInDetailsDao;
        List<WharehouseStockInDetailsDao> aDetailList = new List<WharehouseStockInDetailsDao>();


        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox) loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

            if (cb.Checked)
            {
                HiddenField productidHiddenField =
                    (HiddenField) loadGridView.Rows[i].Cells[1].FindControl("productidHiddenField");
                HiddenField hdfMaterialConversionMasterId =
                    (HiddenField) loadGridView.Rows[i].Cells[1].FindControl("hdfMaterialConversionMasterId");
                TextBox batchTextBox = (TextBox) loadGridView.Rows[i].Cells[4].FindControl("batchTextBox");
                TextBox expDateTextBox = (TextBox) loadGridView.Rows[i].Cells[6].FindControl("expDateDateTextBox");
                TextBox mfgDateTextBox = (TextBox) loadGridView.Rows[i].Cells[5].FindControl("mfgDateTextBox");
                TextBox quantityTextBox = (TextBox) loadGridView.Rows[i].Cells[7].FindControl("reqQtyTextBox");
                TextBox costPriceTextBox = (TextBox) loadGridView.Rows[i].Cells[8].FindControl("costPriceTextBox");
                TextBox vatTextBox = (TextBox) loadGridView.Rows[i].Cells[9].FindControl("vatTextBox");
                TextBox totalValueTextBox = (TextBox) loadGridView.Rows[i].Cells[10].FindControl("totalValueTextBox");

                aWharehouseStockInDetailsDao = new WharehouseStockInDetailsDao();

                aWharehouseStockInDetailsDao.WHStockInMasterID = maxRowId;
                aWharehouseStockInDetailsDao.ProductId = Convert.ToInt32(productidHiddenField.Value);
                aWharehouseStockInDetailsDao.Batch = batchTextBox.Text;
                aWharehouseStockInDetailsDao.ExpDate = Convert.ToDateTime(expDateTextBox.Text);
                aWharehouseStockInDetailsDao.MfgDate = Convert.ToDateTime(mfgDateTextBox.Text);
                aWharehouseStockInDetailsDao.Qty = Convert.ToDecimal(quantityTextBox.Text);
                aWharehouseStockInDetailsDao.Price = Convert.ToDecimal(costPriceTextBox.Text);
                aWharehouseStockInDetailsDao.VAT = Convert.ToDecimal(vatTextBox.Text);
                aWharehouseStockInDetailsDao.TotalAmount = Convert.ToDecimal(totalValueTextBox.Text);
                aWharehouseStockInDetailsDao.MaterialConversionMasterId =
                    string.IsNullOrEmpty(hdfMaterialConversionMasterId.Value)
                        ? 0
                        : Convert.ToInt32(hdfMaterialConversionMasterId.Value);

                aDetailList.Add(aWharehouseStockInDetailsDao);
            }
        }

        string msg = aWarehouseStockInBll.SaveWarehouseStockInDetail(aDetailList);
        Clear();
        showMessageBox(msg);

        // Response.Redirect("WarehouseStockInView.aspx");
    }

    private void UpdateAllData()
    {
        WarehouseStockInMasterDao aStockInMasterDao = new WarehouseStockInMasterDao();

        aStockInMasterDao.WHStockInMasterID = Convert.ToInt32(stockInIdHiddenField.Value);
        aStockInMasterDao.WHStockInDate = Convert.ToDateTime(stockInDateTextBox.Text.Trim());
        aStockInMasterDao.WarehouseId = Convert.ToInt32(warehouseDropDownList.SelectedValue);
        aStockInMasterDao.TotalVat = Convert.ToDecimal(totalVatTextBox.Text);
        aStockInMasterDao.TotalQuantity = Convert.ToInt32(totalQtyTextBox.Text);
        aStockInMasterDao.TotalValue = Convert.ToDecimal(grandTotalTextBox.Text);
        aStockInMasterDao.ChallanNo = challanNoTextBox.Text;
        aStockInMasterDao.ChallanDate = Convert.ToDateTime(challanDateTextBox.Text);
        aStockInMasterDao.ReferenceNo = referenceNoTextBox.Text;

        aStockInMasterDao.SupplierId = string.IsNullOrEmpty(ddlSupplier.SelectedValue) || ddlSupplier.SelectedValue == "0"
    ? 0
    : Convert.ToInt32(ddlSupplier.SelectedValue);

        //if (referenceDateTextBox.Text != "")
        //{
        //    aStockInMasterDao.ReferenceDate = Convert.ToDateTime((referenceDateTextBox.Text).Trim());
        //}
        //else
        //{
        //    aStockInMasterDao.ReferenceDate = (DateTime)SqlDateTime.Null;
        //}
        if (referenceDateTextBox.Text != "")
        {
            aStockInMasterDao.ReferenceDate = ((referenceDateTextBox.Text).Trim());
        }
        else
        {
            aStockInMasterDao.ReferenceDate = "";
        }

        aStockInMasterDao.Remarks = remarksTextBox.Text;
        aStockInMasterDao.Status = "Posted";
        aStockInMasterDao.UpdateBy = Session["LoginName"].ToString();
        aStockInMasterDao.UpdateDate = DateTime.Now;

        bool stockInSave = aWarehouseStockInBll.UpdateStockInMasterInfo(aStockInMasterDao);

        aWarehouseStockInBll.DeleteWhStockInDetailById(Convert.ToInt32(stockInIdHiddenField.Value));

        WharehouseStockInDetailsDao aWharehouseStockInDetailsDao;
        List<WharehouseStockInDetailsDao> aDetailList = new List<WharehouseStockInDetailsDao>();

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox) loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

            if (cb.Checked)
            {

                HiddenField productidHiddenField =
                    (HiddenField) loadGridView.Rows[i].Cells[1].FindControl("productidHiddenField");
                TextBox batchTextBox = (TextBox) loadGridView.Rows[i].Cells[4].FindControl("batchTextBox");
                TextBox expDateTextBox = (TextBox) loadGridView.Rows[i].Cells[6].FindControl("expDateDateTextBox");
                TextBox mfgDateTextBox = (TextBox) loadGridView.Rows[i].Cells[5].FindControl("mfgDateTextBox");
                TextBox quantityTextBox = (TextBox) loadGridView.Rows[i].Cells[7].FindControl("reqQtyTextBox");
                TextBox costPriceTextBox = (TextBox) loadGridView.Rows[i].Cells[8].FindControl("costPriceTextBox");
                TextBox vatTextBox = (TextBox) loadGridView.Rows[i].Cells[9].FindControl("vatTextBox");
                TextBox totalValueTextBox = (TextBox) loadGridView.Rows[i].Cells[10].FindControl("totalValueTextBox");

                aWharehouseStockInDetailsDao = new WharehouseStockInDetailsDao();

                aWharehouseStockInDetailsDao.WHStockInMasterID = Convert.ToInt32(stockInIdHiddenField.Value);
                aWharehouseStockInDetailsDao.ProductId = Convert.ToInt32(productidHiddenField.Value);
                aWharehouseStockInDetailsDao.Batch = batchTextBox.Text;
                aWharehouseStockInDetailsDao.ExpDate = Convert.ToDateTime(expDateTextBox.Text);
                aWharehouseStockInDetailsDao.MfgDate = Convert.ToDateTime(mfgDateTextBox.Text);
                aWharehouseStockInDetailsDao.Qty = Convert.ToDecimal(quantityTextBox.Text);
                aWharehouseStockInDetailsDao.Price = Convert.ToDecimal(costPriceTextBox.Text);
                aWharehouseStockInDetailsDao.VAT = Convert.ToDecimal(vatTextBox.Text);
                aWharehouseStockInDetailsDao.TotalAmount = Convert.ToDecimal(totalValueTextBox.Text);

                aDetailList.Add(aWharehouseStockInDetailsDao);
            }
        }

        string msg = aWarehouseStockInBll.UpdateWarehouseStockInDetail(aDetailList);
        Clear();
        showMessageBox(msg);

        // Response.Redirect("WarehouseStockInView.aspx");
    }

    private void Clear()
    {

        LoadDropdownlist();
        stockInDateTextBox.Text = string.Empty;
        challanNoTextBox.Text = string.Empty;
        challanDateTextBox.Text = string.Empty;
        referenceNoTextBox.Text = string.Empty;
        referenceDateTextBox.Text = string.Empty;
        remarksTextBox.Text = string.Empty;
        totalQtyTextBox.Text = string.Empty;
        grandTotalTextBox.Text = string.Empty;
        totalVatTextBox.Text = string.Empty;

        loadGridView.DataSource = null;
        loadGridView.DataBind();

        submitButton.Text = "Save";
        stockInIdHiddenField.Value = "";
        warehouseDropDownList.Items.Clear();
        ddlProductionUnit.Items.Clear();

    }

    protected void totalValueTextBox_OnTextChanged(object sender, EventArgs e)
    {
        CalculateQuantityVatAndValue();
    }

    public string GenerateParameter()
    {
        string param = " AND CM.MaterialConversionMasterId IS NOT NULL ";

        if (ddlCompany.SelectedValue != "")
        {
            param = param + "  AND CM.BusinessUnitId = '" + ddlCompany.SelectedValue + "' ";
        }

        if (ddlProductionUnit.SelectedValue != "")
        {
            param = param + " AND CM.ProductionUnitId ='" + ddlProductionUnit.SelectedValue + "'";
        }


        return param;
    }

    protected void vatTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        CalculateTotalValue(rowindex);
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox ChkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[4].FindControl("chkSelect");

            if (ChkBoxHeader.Checked == true)
            {
                ChkBoxRows.Checked = true;

            }
            else
            {
                ChkBoxRows.Checked = false;
            }
        }

        CalculateQuantityVatAndValue();
    }

    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlProductionUnit.Items.Clear();

        if (ddlCompany.SelectedValue != "")
        {
            aReceiveDal.GetProductionUnit(ddlProductionUnit, ddlCompany.SelectedValue);
            aWarehouseStockInBll.LoadWhInformation(warehouseDropDownList, ddlCompany.SelectedValue);
        }

        //LoadPendingMaterialConversionList();
    }

    protected void ddlProductionUnit_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPendingMaterialConversionList();
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        CalculateQuantityVatAndValue();
    }

    protected void viewLinkButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("MaterialConversionReceiveView.aspx");
    }
}