using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_OrderRequisitionCreation : System.Web.UI.Page
{
    RequisitionBLL aRequisitionBll = new RequisitionBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitialGrid();
            Todate();
            ReqNo();
            DropDownLoad();

            SetGridStatus();
        }
    }

    private void SetGridStatus()
    {
        productGrid.Visible = companyNameDropDownList.SelectedValue != "";
        submitButton.Enabled = companyNameDropDownList.SelectedValue != "";
    }

    private void DropDownLoad()
    {
        aRequisitionBll.LoadCompanyDropdownList(companyNameDropDownList);
        //aRequisitionBll.WareHouseLoad(warehouseDropDownList);
        //aRequisitionBll.DCLoad(dcDropDownList);
        //wareHouseDropDownList.SelectedIndex = 1;
        //manufacturerDropDownList.SelectedIndex = 1;
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        SetGridStatus();

        InitialGrid();
        
        if (companyNameDropDownList.SelectedValue != "")
        {
            aRequisitionBll.LoadWhInformation(warehouseDropDownList, companyNameDropDownList.SelectedValue);
            aRequisitionBll.DCLoad(dcDropDownList, companyNameDropDownList.SelectedValue);

            Session["CompanyId"] = "";
            Session["CompanyId"] = companyNameDropDownList.SelectedValue;
        }
        else
        {
            Session["CompanyId"] = "";
            warehouseDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();
        }
    }

    private void ReqNo()
    {
        reqNoTextBox.Text = aRequisitionBll.ReqNo();
    }
    private void Todate()
    {
        reqDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
    }
    private void InitialGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");

        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["PackSize"] = "";
        dataRow["Quantity"] = "";

        aDataTable.Rows.Add(dataRow);

        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();

        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            //modal.ContextKey = companyNameDropDownList.SelectedValue;
        }
       
    }

    private void AddRowInGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");

        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                dataRow = aDataTable.NewRow();
                dataRow["SL"] = Convert.ToString(i + 1);
                TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                dataRow["ProductCode"] = productCodeTextBox.Text.Trim();
                TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                dataRow["ProductName"] = productNameTextBox.Text.Trim();
                TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                dataRow["PackSize"] = packSizeTextBox.Text;
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                dataRow["Quantity"] = quantityTextBox.Text.Trim();
                aDataTable.Rows.Add(dataRow);
            }
        }
        int sl = aDataTable.Rows.Count;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["PackSize"] = "";
        dataRow["Quantity"] = "";
        aDataTable.Rows.Add(dataRow);


        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = companyNameDropDownList.SelectedValue;
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        AddRowInGrid();
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton productCodeTextBox = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)productCodeTextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            int sl1 = 1;
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (i != rowindex)
                {
                    dataRow = aDataTable.NewRow();

                    dataRow["SL"] = Convert.ToString(sl1);
                    TextBox productCodeTextBox2 = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                    dataRow["ProductCode"] = productCodeTextBox2.Text.Trim();
                    TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                    dataRow["ProductName"] = productNameTextBox.Text.Trim();
                    TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                    dataRow["PackSize"] = packSizeTextBox.Text;
                    TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                    dataRow["Quantity"] = quantityTextBox.Text.Trim();
                    aDataTable.Rows.Add(dataRow);
                    sl1 += 1;
                }
            }
        }
        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        if (productGridView.Rows.Count < 1)
        {
            InitialGrid();
        }
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = companyNameDropDownList.SelectedValue;
        }
    }
    private void GetProductInGrid(int rowindex, string productCode)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {
            if (companyNameDropDownList.SelectedValue != "")
            {
                if (ProductExistOrNot(productCode, rowindex))
                {
                    aDataTable = _aDcStockReceiveBll.ProductInfo(productCode, companyNameDropDownList.SelectedValue);
                    if (aDataTable.Rows.Count > 0)
                    {
                        TextBox productNameTextBox =
                            (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                        productNameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();
                        TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                        packSizeTextBox.Text = aDataTable.Rows[0]["PackSize"].ToString();
                    }
                    else
                    {
                        TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
                        productCodeTextBox.Text = "";

                        TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                        packSizeTextBox.Text = "";

                        TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                        productNameTextBox.Text = "";

                        //HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
                        //productidHiddenField.Value = "";
                        ShowMessageBox("Input Correct Data!!");
                    }
                }

                else
                {
                    TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
                    productCodeTextBox.Text = "";

                    TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                    packSizeTextBox.Text = "";

                    TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                    productNameTextBox.Text = "";

                    //HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
                    //productidHiddenField.Value = "";

                    ShowMessageBox("Product already exist !!!");
                }
                
            }
            else
            {
                TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
                productCodeTextBox.Text = "";

                TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                packSizeTextBox.Text = "";

                TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                productNameTextBox.Text = "";

                //HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
               // productidHiddenField.Value = "";
                ShowMessageBox("You have to select a company before adding product to grid!!!");
            }

            
        }
    }

    private bool ProductExistOrNot(string productCode, int rowindex)
    {
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            var productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");

            if (productCodeTextBox.Text.Trim() == productCode && i != rowindex)
            {
                return false;
            }
        }

        return true;
    }

    protected void productCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();

        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productCodeTextBox =
            (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");

        string productCode = productCodeTextBox.Text.Trim();
        GetProductInGrid(rowindex, productCode);

    }


    private void SaveAllData()
    {
        string[] companyInfo = dcDropDownList.SelectedItem.Text.Split(':');
        int maxReqId;
        var aRequesition = new Requesition()
                                       {
                                           
                                           ReqNo = reqNoTextBox.Text.Trim(),
                                           ReqDate = Convert.ToDateTime(reqDateTextBox.Text.Trim()),
                                           WarehouseId = Convert.ToInt32(warehouseDropDownList.SelectedValue),
                                           WearhouseName = warehouseDropDownList.SelectedItem.Text.Trim(),
                                           ComUnitId = Convert.ToInt32(dcDropDownList.SelectedValue),
                                           ComUnitCode = companyInfo[0],
                                           ComUnitName = companyInfo[1],
                                           EntryBy = Session["LoginName"].ToString(),
                                           EntryDate = DateTime.Now,
                                           CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue),
                                       };


        bool requsitionSave = aRequisitionBll.SaveRequsition(aRequesition, out maxReqId);

        List<RequsitionChild> aRequsitionChildrenList = new List<RequsitionChild>();

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {

                TextBox productCodeTextBox = (TextBox) productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                RequsitionChild aRequsitionChild = new RequsitionChild();
                aRequsitionChild.ProductCode = productCodeTextBox.Text.Trim().ToUpper();
                aRequsitionChild.ProductName = productNameTextBox.Text.Trim();
                aRequsitionChild.PackSize = packSizeTextBox.Text.Trim();
                aRequsitionChild.ReqQty = Convert.ToDecimal(quantityTextBox.Text.Trim());
                aRequsitionChild.ReqId = maxReqId;

            aRequsitionChildrenList.Add(aRequsitionChild);
        }

        string msg = aRequisitionBll.SaveRequsitionChild(aRequsitionChildrenList);
        Clear();
        ShowMessageBox(msg);
    }

    public bool Validation()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select a company !!!");
            companyNameDropDownList.Focus();
            companyNameDropDownList.BackColor = Color.GhostWhite;
            return false;
        }
        if (warehouseDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select WareHouse !!!");
            warehouseDropDownList.Focus();
            warehouseDropDownList.BackColor = Color.GhostWhite;
            return false;
        }
        if (dcDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select Distribution Center  !!!");
            dcDropDownList.Focus();
            dcDropDownList.BackColor = Color.GhostWhite;
            return false;
        }
        if (productGridView.Rows.Count > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (((TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox")).Text == "")
                {
                    ((TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox")).Focus();
                    ShowMessageBox("Please fill out Req.Qty!!");
                    return false;
                }
            }
        }
        if (productGridView.Rows.Count > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text == "")
                {
                    ((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Focus();
                    ShowMessageBox("Please fill out productCode!!");
                    return false;
                }
            }
        }
        if (productGridView.Rows.Count > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (((TextBox)productGridView.Rows[i].FindControl("productNameTextBox")).Text == "")
                {
                    ((TextBox)productGridView.Rows[i].FindControl("productNameTextBox")).Focus();
                    ShowMessageBox("Please fill out productName!!");
                    return false;
                }
            }
        }
        return true;
    }
    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            SaveAllData();
        }
        else
        {
            ShowMessageBox("Select Manufacturer Name!!");
        }
       
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    private void Clear()
    {
       // manufacturerDropDownList.SelectedValue = "";
        InitialGrid();
        Todate();
        ReqNo();
        DropDownLoad();
        companyNameDropDownList.SelectedValue = "";

        if (companyNameDropDownList.SelectedValue == "")
        {
            warehouseDropDownList.Items.Clear();
            dcDropDownList.Items.Clear();
        }

        
        SetGridStatus();

        
    }
    protected void productNameTextBox_TextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");

        string productName = productNameTextBox.Text.Trim();
        if (productName.Contains(':'))
        {
            string[] productInfo = productName.Split(':');

            TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");

            productCodeTextBox.Text = productInfo[0];
            //productNameTextBox.Text = productInfo[1];
            string productCode = productCodeTextBox.Text.Trim();
            GetProductInGrid(rowindex, productCode);
        }
        else
        {
            TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
            productCodeTextBox.Text = "";

            TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
            packSizeTextBox.Text = "";

            productNameTextBox.Text = "";

            HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
            //productidHiddenField.Value = "";
            ShowMessageBox("Input Correct Data!!");
            
        }
    }
    protected void miaImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("OrderRequisitionView.aspx");
    }
    protected void manufacturerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitialGrid();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        ExcelToGrid();
    }
    private void ExcelToGrid()
    {
        string FileName = Path.GetFileName(id_fu.PostedFile.FileName);
        string Extension = Path.GetExtension(id_fu.PostedFile.FileName);
        string FilePath = "~/ExcelFiles/" + id_fu.FileName;
        id_fu.SaveAs(MapPath(FilePath));

        string path = System.IO.Path.GetFullPath(Server.MapPath(FilePath));
        OleDbConnection oledbConn = null;

        if (Path.GetExtension(path) == ".xls")
        {
            oledbConn =
                new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path +
                                    ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
        }
        else if (Path.GetExtension(path) == ".xlsx")
        {
            oledbConn =
                new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path +
                                    ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
        }

        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = oledbConn;

        oledbConn.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        oledbConn.Close();

        oledbConn.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        oledbConn.Close();

        DataTable destinationTable = new DataTable();
        destinationTable = dt.Clone();

        foreach (DataRow row in dt.Rows)
        {
            if (!string.IsNullOrEmpty(row[0].ToString()))
            {
                destinationTable.ImportRow(row);
            }
        }
        string fileName = Path.GetFileName(FilePath);
        //txtSheetName.Text = fileName;
        productGridView.Caption = fileName;
        productGridView.DataSource = destinationTable;
        productGridView.DataBind();
        lbl_up_status.Text = productGridView.Rows.Count.ToString() + " record Found!";
        IsFileUploaded.Value = "true";
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }
}