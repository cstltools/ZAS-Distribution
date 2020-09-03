using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management.Instrumentation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ManualOrderrCreation : System.Web.UI.Page
{
    ManualOrderCreationDal aOrderCreationDal = new ManualOrderCreationDal();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadDropDownList();
            InitialGrid();
            SetOrderDate();
            SetOderNo();
        }
    }

    private void SetOrderDate()
    {
        orderDateTextBox.Text = Convert.ToDateTime(DateTime.Today.ToShortDateString()).ToString("dd-MMM-yyyy");
    }

    private void InitialGrid()
    {
        var aDataTable = new DataTable();

        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("GrossValue");

        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["PackSize"] = "";
        dataRow["Quantity"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["ProductId"] = "";
        dataRow["VAT"] = "";

        aDataTable.Rows.Add(dataRow);

        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();

        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = companyDropDownList.SelectedValue;
        }

    }

    private void LoadDropDownList()
    {
        aOrderCreationDal.GetDdlCompany(companyDropDownList);
        aOrderCreationDal.GetDdlRegion(regionDropDownList);
    }

    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        InitialGrid();
        companyDropDownList.SelectedValue = "";

        if (custCodeTextBox.Text.Contains(':'))
        {
            custIdHiddenField.Value = "";
            string[] data = custCodeTextBox.Text.Split(':');
            custCodeTextBox.Text = data[0];
            custIdHiddenField.Value = data[3];

        }
        else
        {
            if (custCodeTextBox.Text != "")
            {
                custIdHiddenField.Value = "";
                DataTable aTable = aOrderCreationDal.GetCustomerInfoByCode(custCodeTextBox.Text.Trim());

                if (aTable.Rows.Count > 0)
                {
                    custIdHiddenField.Value =
                        aTable.Rows[0].Field<int>("CustomerMasterId").ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    custIdHiddenField.Value = "";
                    custCodeTextBox.Text = "";
                    Clear();
                    ShowMessageBox("Customer does not exist or not contain enough information !!!!");
                }
            }
        }

        if (custIdHiddenField.Value != "")
        {
            isVatApplicable.Value = "";
            DataTable checkedTable = aOrderCreationDal.GetIsVatApplicable(custIdHiddenField.Value);

            if (checkedTable.Rows.Count > 0)
            {
                isVatApplicable.Value = checkedTable.Rows[0].Field<bool>("IsVatApplicable").ToString(CultureInfo.InvariantCulture);
            }
        }


        if (custCodeTextBox.Text != "" && custIdHiddenField.Value != "")
        {
            SetCusromerDetailInfo();

            DataTable dtcrlimit = aOrderCreationDal.GetCustomerCreditLimit(custIdHiddenField.Value);
            DataTable dueAmmount = aOrderCreationDal.GetCustomerDueInfo(custIdHiddenField.Value);

            if (dueAmmount.Rows.Count > 0)
            {
                if (dueAmmount.Rows[0].Field<Decimal>("Due") > 0)
                {
                    previousDueLabel.Text =
                        dueAmmount.Rows[0].Field<Decimal>("Due").ToString(CultureInfo.InvariantCulture);

                    previousDueLabel.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    previousDueLabel.Text = "0.00";
                    previousDueLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#9900CC");
                }
            }

            SetDueStatus();

           

            if (dtcrlimit.Rows.Count > 0)
            {
                creditLimitNameLabel.Text = dtcrlimit.Rows[0]["LimitAmount"].ToString();
            }

            DataTable dtusedcr = aOrderCreationDal.GetCustomerUsedCreditLimit(custIdHiddenField.Value);

            if (dtusedcr.Rows.Count > 0)
            {
                crtakenNameLabel.Text = dtusedcr.Rows[0][0].ToString();
            }
            remainingcrLabel.Text = ((string.IsNullOrEmpty(creditLimitNameLabel.Text)
                ? 0
                : Convert.ToDecimal(creditLimitNameLabel.Text))
                                     - (string.IsNullOrEmpty(crtakenNameLabel.Text) ? 0 : Convert.ToDecimal(crtakenNameLabel.Text))).ToString();
            decimal creditlim = 0;
            creditlim = string.IsNullOrEmpty(creditLimitNameLabel.Text) ? 0 : Convert.ToDecimal(creditLimitNameLabel.Text);
            if (creditlim > 0)
            {
                percentOfUsedCrLabel.Text = (((string.IsNullOrEmpty(crtakenNameLabel.Text)
                ? 0
                : Convert.ToDecimal(crtakenNameLabel.Text)) * 100) / (string.IsNullOrEmpty(creditLimitNameLabel.Text)
                    ? 0
                    : Convert.ToDecimal(creditLimitNameLabel.Text))).ToString("F") + "%";
            }
            grossLabel.Text = string.Empty;
            CalculateRowGross();
            CalculateGross();
            IncreasetakenLimit();

        }

        if ((Convert.ToDecimal(remainingcrLabel.Text) < 0 && paymentHiddenField.Value != "Cash") || dueHiddenField.Value == "DueExist")
        {
            spord.Visible = true;
            isSpecialAppCheckBox.Visible = true;
        }
        else
        {
            spord.Visible = false;
            isSpecialAppCheckBox.Visible = false;
            isSpecialAppCheckBox.Checked = false;
        }
    }

    private void SetDueStatus()
    {

        if (Convert.ToDecimal(previousDueLabel.Text.Trim()) > 0)
        {
            dueHiddenField.Value = "";
            dueHiddenField.Value = "DueExist";
        }
        else
        {
            dueHiddenField.Value = "";
        }

        
    }

    private void SetCusromerDetailInfo()
    {
        DataTable cstmrInfo = aOrderCreationDal.GetCustomerInfo(custCodeTextBox.Text.Trim());

        if (cstmrInfo.Rows.Count > 0)
        {
            custNameLabel.Text = cstmrInfo.Rows[0]["CustomerName"].ToString();
            marketNameLabel.Text = cstmrInfo.Rows[0]["MarketName"].ToString();
            
            paymentHiddenField.Value = "";
            paymentHiddenField.Value = cstmrInfo.Rows[0].Field<String>("TermOfPayment");

            regionDropDownList.SelectedValue = cstmrInfo.Rows[0]["RegionId"].ToString();

            if (regionDropDownList.SelectedValue != "")
            {
                aOrderCreationDal.GetDdlArea(areaDropDownList, regionDropDownList.SelectedValue);
                areaDropDownList.SelectedValue = cstmrInfo.Rows[0]["AreaId"].ToString();
            }
            else
            {
                areaDropDownList.Items.Clear();
                territoryDropDownList.Items.Clear();
                marketDropDownList.Items.Clear();
                dcDropDownList.Items.Clear();
            }

            if (areaDropDownList.SelectedValue != "")
            {
                aOrderCreationDal.GetDdlTeritory(territoryDropDownList, areaDropDownList.SelectedValue);
                territoryDropDownList.SelectedValue = cstmrInfo.Rows[0]["TerritoryId"].ToString();
               
                //Load DC

                if (Session["UserType"].ToString() != "")
                {
                    if (Session["UserType"].ToString() != "Admin")
                    {
                        aOrderCreationDal.GetDdlDcByUser(dcDropDownList, areaDropDownList.SelectedValue, Session["UserId"].ToString());
                    }
                    else
                    {
                        aOrderCreationDal.GetDdlDc(dcDropDownList, areaDropDownList.SelectedValue);
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                } 


                
            }
            else
            {
                territoryDropDownList.Items.Clear();
                marketDropDownList.Items.Clear();
                dcDropDownList.Items.Clear();
            }

            if (territoryDropDownList.SelectedValue != "")
            {
                aOrderCreationDal.GetDdlMarket(marketDropDownList, territoryDropDownList.SelectedValue);
                marketDropDownList.SelectedValue = cstmrInfo.Rows[0]["MarketId"].ToString();

                if (marketDropDownList.SelectedValue != "")
                {
                    string[] marketName = marketDropDownList.SelectedItem.Text.Split(':');

                    marketNameLabel.Text = marketName[0];
                   
                }

                //Load MIO

                aOrderCreationDal.GetDdlMio(mioDropDownList, territoryDropDownList.SelectedValue);
            }
            else
            {
                marketDropDownList.Items.Clear();
                mioDropDownList.Items.Clear();
            }

            regionDropDownList.Enabled = false;
            areaDropDownList.Enabled = false;
            territoryDropDownList.Enabled = false;
            marketDropDownList.Enabled = false;
            mioDropDownList.Enabled = false;
            companyDropDownList.Enabled = false;
        }

        else
        {
            ShowMessageBox("Customer does not exist or not contain enough information !!!!");
        }
    }


    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void Clear()
    {

        InitialGrid();
        SetOrderDate();
        SetOderNo();
        LoadDropDownList();
        custNameLabel.Text = string.Empty;
        mioCodeLabel.Text = string.Empty;
        mioNameLabel.Text = string.Empty;
        marketNameLabel.Text = string.Empty;
        orderIdHiddenField.Value = string.Empty;
        delIdHiddenField.Value = string.Empty;
        mioDropDownList.SelectedValue = "";
        custIdHiddenField.Value = string.Empty;
        //regionDropDownList.Items.Clear();
        regionDropDownList.SelectedValue = "";
        companyDropDownList.SelectedValue = "";

        custCodeTextBox.Text = "";
        dcDropDownList.Items.Clear();
        areaDropDownList.Items.Clear();
        creditLimitNameLabel.Text = string.Empty;
        remainingcrLabel.Text = string.Empty;
        mioDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
        marketDropDownList.Items.Clear();
        crtakenNameLabel.Text = "";
        remainingcrLabel.Text = "";
        grossLabel.Text = string.Empty;

        spord.Visible = false;
        isSpecialAppCheckBox.Visible = false;

        percentOfUsedCrLabel.Text = "";

        if (isSpecialAppCheckBox.Checked)
        {
            isSpecialAppCheckBox.Checked = false;
        }

    }

    private void SetOderNo()
    {
        orderNoTextBox.Text = "0001";
        //OrdNo();
    }

    private string OrdNo()
    {
        string ordNo = string.Empty;

        ordNo = OrdnNoGenerator(aOrderCreationDal.OrderManualId());

        return ordNo;
    }

    private string OrdnNoGenerator(int id)
    {
        string code = string.Empty;
        string Id = id.ToString();
        if (Id.Length == 1)
        {
            Id = "000000" + Id;
        }
        if (Id.Length == 2)
        {
            Id = "00000" + Id;
        }
        if (Id.Length == 3)
        {
            Id = "0000" + Id;
        }
        if (Id.Length == 4)
        {
            Id = "000" + Id;
        }
        if (Id.Length == 5)
        {
            Id = "00" + Id;
        }
        if (Id.Length == 6)
        {
            Id = "0" + Id;
        }
        code = "SMP-" + Id;
        return code;
    }

    protected void dcDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        mioCodeLabel.Text = "";
        mioNameLabel.Text = "";

        InitialGrid();

        if (dcDropDownList.SelectedValue != "")
        {
           DataTable aTable = aOrderCreationDal.GetCompanyByDc(dcDropDownList.SelectedValue);

           if (aTable.Rows.Count > 0)
            {
                companyDropDownList.SelectedValue = aTable.Rows[0]["CompanyId"].ToString();

                foreach (GridViewRow row in productGridView.Rows)
                {
                    TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
                    AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
                    modal.ContextKey = companyDropDownList.SelectedValue;
                }
            }
        }
        else
        {
            companyDropDownList.SelectedValue = "";
        }

        if (companyDropDownList.SelectedValue != "" && territoryDropDownList.SelectedValue != "")
        {
            DataTable aTable = aOrderCreationDal.GetMioByCompany(companyDropDownList.SelectedValue, territoryDropDownList.SelectedValue);

            if (aTable.Rows.Count > 0)
            {
                mioDropDownList.SelectedValue = aTable.Rows[0]["MIOId"].ToString();

                if (mioDropDownList.SelectedValue != "")
                {
                    string[] mioName = mioDropDownList.SelectedItem.Text.Split(':');

                    mioCodeLabel.Text = mioName[1];
                    mioNameLabel.Text = mioName[0];
                }
            }
            else
            {
                mioDropDownList.SelectedValue = "";
                ShowMessageBox("Mio doesn't exist on this territory !!!");
            }
        }
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
            ShowMessageBox("Input Correct Data!!");
            ClearRow(rowindex);
        }
    }

    

    private void GetProductInGrid(int rowindex, string productCode)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {

            if (ProductExistOrNot(productCode, rowindex))
            {
                aDataTable = _aDcStockReceiveBll.ProductInfo(productCode, companyDropDownList.SelectedValue);

                if (aDataTable.Rows.Count > 0)
                {
                    HiddenField productidHiddenField =
                        (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
                    TextBox productNameTextBox =
                        (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                    productNameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();
                    TextBox packSizeTextBox =
                        (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                    packSizeTextBox.Text = aDataTable.Rows[0]["PackSize"].ToString();
                    HiddenField unitpriceHiddenField =
                        (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("unitpriceHiddenField");
                    unitpriceHiddenField.Value = aDataTable.Rows[0]["UnitPrice"].ToString();
                    productidHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();
                    TextBox vatTextBox =
                        (TextBox)productGridView.Rows[rowindex].Cells[5].FindControl("vatTextBox");
                    TextBox tpTextBox =
                        (TextBox)productGridView.Rows[rowindex].Cells[6].FindControl("tpTextBox");
                    tpTextBox.Text = aDataTable.Rows[0]["UnitPrice"].ToString();

                    vatTextBox.Text = isVatApplicable.Value == "False" ? 0.ToString(CultureInfo.InvariantCulture) : aDataTable.Rows[0]["VATAmountPerUnit"].ToString();
                    

                }
                else
                {
                    ShowMessageBox("Product Not Found");
                    ClearRow(rowindex);

                }
                if (rowindex != 0)
                {
                    if (
                        ((HiddenField)productGridView.Rows[rowindex - 1].Cells[0].FindControl("productidHiddenField"))
                            .Value ==
                        string.Empty)
                    {
                        ShowMessageBox("Fill Out Previous Field");
                        ClearRow(rowindex);
                    }
                }
            }
            else
            {
                var productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
                TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                TextBox vatTextBox = (TextBox)productGridView.Rows[rowindex].Cells[5].FindControl("vatTextBox");
                TextBox tpTextBox = (TextBox)productGridView.Rows[rowindex].Cells[6].FindControl("tpTextBox");
                TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
                HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("unitpriceHiddenField");

                productCodeTextBox.Text = "";
                productNameTextBox.Text = "";
                packSizeTextBox.Text = "";
                unitpriceHiddenField.Value = "";
                productidHiddenField.Value = "";
                tpTextBox.Text = "";
                vatTextBox.Text = "";


                ShowMessageBox("Product Already Exist !!!");
            }


        }
    }

    public void ClearRow(int rowindex)
    {
        TextBox productNameTextBox =
                    (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
        productNameTextBox.Text = "";
        TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
        productCodeTextBox.Text = "";
        TextBox packSizeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[3].FindControl("packSizeTextBox");
        packSizeTextBox.Text = "";
        ((TextBox)productGridView.Rows[rowindex].FindControl("grossTextBox")).Text = string.Empty;
        HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
        HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("unitpriceHiddenField");
        TextBox vatTextBox =
            (TextBox)productGridView.Rows[rowindex].Cells[5].FindControl("vatTextBox");
        TextBox tpTextBox =
            (TextBox)productGridView.Rows[rowindex].Cells[6].FindControl("tpTextBox");
        TextBox grossTextBox =
            (TextBox)productGridView.Rows[rowindex].Cells[7].FindControl("grossTextBox");
        TextBox quantityTextBox = (TextBox)productGridView.Rows[rowindex].Cells[4].FindControl("reqQtyTextBox");
        quantityTextBox.Text = string.Empty;
        productidHiddenField.Value = string.Empty;
        unitpriceHiddenField.Value = string.Empty;
        vatTextBox.Text = string.Empty;
        tpTextBox.Text = string.Empty;
        grossTextBox.Text = string.Empty;

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

        TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");

        string productCode = productCodeTextBox.Text.Trim();
        GetProductInGrid(rowindex, productCode);
    }

    protected void reqQtyTextBox_OnTextChanged(object sender, EventArgs e)
    {

        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;
        HiddenField productidHiddenField =
                    (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
        if (productidHiddenField.Value == string.Empty)
        {
            ClearRow(rowindex);
            ShowMessageBox("Give Product Information");
        }
        else
        {
            CalculateRowGross();
            IncreasetakenLimit();
            CalculateGross();
        }

         if (dueHiddenField.Value == "DueExist")
         {
            spord.Visible = true;
            isSpecialAppCheckBox.Visible = true;
        }
        else
        {
            if (Convert.ToDecimal(remainingcrLabel.Text) < 0 && paymentHiddenField.Value != "Cash")
            {
                spord.Visible = true;
                isSpecialAppCheckBox.Visible = true;
            }
            else
            {
                spord.Visible = false;
                isSpecialAppCheckBox.Visible = false;
                isSpecialAppCheckBox.Checked = false;
            }
        }
        
    }

    public void CalculateGross()
    {
        decimal gross = 0;

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            decimal amount = string.IsNullOrEmpty(((TextBox)productGridView.Rows[i].FindControl("grossTextBox")).Text) ? 0 : Convert.ToDecimal(((TextBox)productGridView.Rows[i].FindControl("grossTextBox")).Text);
            gross += amount;
        }
        grossLabel.Text = gross.ToString("F");
    }

    public void IncreasetakenLimit()
    {
        remainingcrLabel.Text = ((string.IsNullOrEmpty(creditLimitNameLabel.Text)
            ? 0
            : Convert.ToDecimal(creditLimitNameLabel.Text))
                                 - (string.IsNullOrEmpty(crtakenNameLabel.Text) ? 0 : Convert.ToDecimal(crtakenNameLabel.Text))).ToString();
        decimal totaltaken = 0;
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            totaltaken += string.IsNullOrEmpty(((TextBox)productGridView.Rows[i].FindControl("grossTextBox")).Text) ? 0 : Convert.ToDecimal(((TextBox)productGridView.Rows[i].FindControl("grossTextBox")).Text);
        }
        remainingcrLabel.Text = (Convert.ToDecimal(remainingcrLabel.Text) - totaltaken).ToString();

        if (Convert.ToDecimal(remainingcrLabel.Text) < 0)
        {

            remainingcrLabel.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            remainingcrLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#9900CC");
        }
    }

    public void CalculateRowGross()
    {
        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            decimal tp = 0;
            decimal qty = 0;
            tp =
                string.IsNullOrEmpty(((TextBox)productGridView.Rows[i].FindControl("tpTextBox")).Text)
                    ? 0
                    : Convert.ToDecimal(((TextBox)productGridView.Rows[i].FindControl("tpTextBox")).Text);
            qty = string.IsNullOrEmpty(((TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox")).Text)
                    ? 0
                    : Convert.ToDecimal(((TextBox)productGridView.Rows[i].FindControl("reqQtyTextBox")).Text);
            ((TextBox)productGridView.Rows[i].FindControl("grossTextBox")).Text = (tp * qty).ToString();
        }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        AddRowInGrid();
        IncreasetakenLimit();
    }

    private void AddRowInGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("GrossValue");

        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {

                dataRow = aDataTable.NewRow();
                TextBox vatTextBox =
                    (TextBox)productGridView.Rows[i].Cells[5].FindControl("vatTextBox");
                TextBox tpTextBox =
                    (TextBox)productGridView.Rows[i].Cells[6].FindControl("tpTextBox");

                dataRow["SL"] = Convert.ToString(i + 1);
                TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                dataRow["ProductCode"] = productCodeTextBox.Text.Trim();
                TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                dataRow["ProductName"] = productNameTextBox.Text.Trim();
                TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                TextBox grossTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("grossTextBox");
                dataRow["PackSize"] = packSizeTextBox.Text;
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("unitpriceHiddenField");
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
                dataRow["Quantity"] = quantityTextBox.Text.Trim();
                dataRow["ProductId"] = productidHiddenField.Value.Trim();
                dataRow["UnitPrice"] = unitpriceHiddenField.Value;
                dataRow["VAT"] = vatTextBox.Text;
                dataRow["GrossValue"] = grossTextBox.Text;


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
        dataRow["ProductId"] = "";
        dataRow["UnitPrice"] = "";
        dataRow["VAT"] = "";
        dataRow["GrossValue"] = "";
        aDataTable.Rows.Add(dataRow);


        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = companyDropDownList.SelectedValue;
        }

        if (dueHiddenField.Value == "DueExist")
        {
            spord.Visible = true;
            isSpecialAppCheckBox.Visible = true;
        }
        else
        {
            if (Convert.ToDecimal(remainingcrLabel.Text) < 0 && paymentHiddenField.Value != "Cash")
            {
                spord.Visible = true;
                isSpecialAppCheckBox.Visible = true;
            }
            else
            {
                spord.Visible = false;
                isSpecialAppCheckBox.Visible = false;
                isSpecialAppCheckBox.Checked = false;
            }
        }
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
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("PackSize");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("UnitPrice");
        aDataTable.Columns.Add("VAT");
        aDataTable.Columns.Add("GrossValue");
        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            int sl1 = 1;
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (i != rowindex)
                {
                    dataRow = aDataTable.NewRow();
                    TextBox vatTextBox =
                    (TextBox)productGridView.Rows[i].Cells[5].FindControl("vatTextBox");
                    dataRow["SL"] = Convert.ToString(sl1);
                    TextBox productCodeTextBox2 = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                    dataRow["ProductCode"] = productCodeTextBox2.Text.Trim();
                    TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                    dataRow["ProductName"] = productNameTextBox.Text.Trim();
                    TextBox packSizeTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("packSizeTextBox");
                    dataRow["PackSize"] = packSizeTextBox.Text;
                    TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
                    HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("unitpriceHiddenField");
                    HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
                    TextBox grossTextBox = (TextBox)productGridView.Rows[i].Cells[3].FindControl("grossTextBox");
                    dataRow["Quantity"] = quantityTextBox.Text.Trim();
                    dataRow["UnitPrice"] = unitpriceHiddenField.Value.Trim();
                    dataRow["VAT"] = vatTextBox.Text;
                    dataRow["GrossValue"] = grossTextBox.Text;
                    dataRow["ProductId"] = productidHiddenField.Value.Trim();
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
            modal.ContextKey = companyDropDownList.SelectedValue;
        }
        CalculateRowGross();
        CalculateGross();
        IncreasetakenLimit();

        if (dueHiddenField.Value == "DueExist")
        {
            spord.Visible = true;
            isSpecialAppCheckBox.Visible = true;
        }
        else
        {
            if (Convert.ToDecimal(remainingcrLabel.Text) < 0 && paymentHiddenField.Value != "Cash")
            {
                spord.Visible = true;
                isSpecialAppCheckBox.Visible = true;
            }
            else
            {
                spord.Visible = false;
                isSpecialAppCheckBox.Visible = false;
                isSpecialAppCheckBox.Checked = false;
            }
        }
    
}
    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            Int32 masterId = aOrderCreationDal.SaveOrderInformation(PrepareOrderData());
            Int32 id = 0;

            if (masterId > 0)
            {
                id = SaveDetailInformation(PrepareOrderDetailData(masterId));

                if (CheckDataSavedOrNot(masterId))
                {
                    ShowMessageBox("Order Generated Successfully !!");
                    Clear();
                }
                else
                {
                    aOrderCreationDal.DeleteOrderMasterInfo(masterId);
                    ShowMessageBox("Error! Check Your Internet Conection !!");
                }
            }

            
        }
    }

    private bool CheckDataSavedOrNot(int masterId)
    {
        DataTable aTable = aOrderCreationDal.GetDataSaveStatus(masterId);

        if (aTable.Rows.Count > 0)
        {
            return true;
        }

        return false;
    }

    private int SaveDetailInformation(List<OrderInfoDetail> aOrderInfoDetails)
    {
        Int32 id = 0;

        foreach (var aOrderInfoDetail in aOrderInfoDetails)
        {
            id = aOrderCreationDal.SaveOrderDetailInfo(aOrderInfoDetail);
        }

        return id;
    }

    private List<OrderInfoDetail> PrepareOrderDetailData(int masterId)
    {
        List<OrderInfoDetail> aOrderInfoDetailList = new List<OrderInfoDetail>();
        OrderInfoDetail aOrderInfoDetail;

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {

            TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
            TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
            TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
            HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
            HiddenField unitpriceHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("unitpriceHiddenField");

            aOrderInfoDetail = new OrderInfoDetail();

            aOrderInfoDetail.ProductCode = productCodeTextBox.Text.Trim();
            aOrderInfoDetail.ProductName = productNameTextBox.Text.Trim();
            aOrderInfoDetail.ProductId = Convert.ToInt32(productidHiddenField.Value);
            aOrderInfoDetail.Quantity = Convert.ToDecimal(quantityTextBox.Text.Trim());
            aOrderInfoDetail.OrderId = masterId;
            aOrderInfoDetail.TradePrice = Convert.ToDecimal(unitpriceHiddenField.Value);
            aOrderInfoDetail.TotalTradePrice = aOrderInfoDetail.Quantity * aOrderInfoDetail.TradePrice;

            aOrderInfoDetailList.Add(aOrderInfoDetail);
        }

        return aOrderInfoDetailList;
    }

    private OrderInfoMaster PrepareOrderData()
    {
        var aListMasterDao = new OrderInfoMaster();

        aListMasterDao.OrderCode = orderNoTextBox.Text.Trim();
        aListMasterDao.ComUnitId = Convert.ToInt32(dcDropDownList.SelectedValue);
        aListMasterDao.ComUnitName = dcDropDownList.SelectedItem.Text.Split(':')[1].Trim();
        aListMasterDao.ComUnitCode = dcDropDownList.SelectedItem.Text.Split(':')[0].Trim();
        aListMasterDao.MIOCode = mioCodeLabel.Text.Trim();
        aListMasterDao.MIOName = mioNameLabel.Text.Trim();
        aListMasterDao.ManufacId = Convert.ToInt32(companyDropDownList.SelectedValue);
        aListMasterDao.CustomerCode = custCodeTextBox.Text.Trim();
        aListMasterDao.SubmissionDate = Convert.ToDateTime(orderDateTextBox.Text.Trim());
        aListMasterDao.CustomerName = custNameLabel.Text.Trim();
        aListMasterDao.CustomerMasterId = Convert.ToInt32(custIdHiddenField.Value);
        aListMasterDao.IsManual = true;
        aListMasterDao.IsSpecialApproval = isSpecialAppCheckBox.Checked;
        aListMasterDao.EntryBy = Session["LoginName"].ToString();
        aListMasterDao.EntryDate = DateTime.Now;
        aListMasterDao.MIOId = Convert.ToInt32(mioDropDownList.SelectedValue);
        aListMasterDao.MIOId = Convert.ToInt32(mioDropDownList.SelectedValue);
        aListMasterDao.RegionId = Convert.ToInt32(regionDropDownList.SelectedValue);
        aListMasterDao.AreaId = Convert.ToInt32(areaDropDownList.SelectedValue);
        aListMasterDao.TerritoryId = Convert.ToInt32(territoryDropDownList.SelectedValue);
        aListMasterDao.MarketId = Convert.ToInt32(marketDropDownList.SelectedValue);

        DataTable asmInfo = aOrderCreationDal.GetAsmInfo(areaDropDownList.SelectedValue, companyDropDownList.SelectedValue);

        if (asmInfo.Rows.Count > 0)
        {
            aListMasterDao.ASMId = string.IsNullOrEmpty(asmInfo.Rows[0]["ASMId"].ToString()) ? 0 : Convert.ToInt32(asmInfo.Rows[0]["ASMId"].ToString());
        }
        
        DataTable rsmInfo = aOrderCreationDal.GetRsmInfo(regionDropDownList.SelectedValue, companyDropDownList.SelectedValue);

        if (rsmInfo.Rows.Count > 0)
        {
            aListMasterDao.RSMId = string.IsNullOrEmpty(rsmInfo.Rows[0]["RSMId"].ToString()) ? 0 : Convert.ToInt32(rsmInfo.Rows[0]["RSMId"].ToString());
        }
        
        decimal totalprice = 0;

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
            var quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("reqQtyTextBox");
            var unitpriceHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("unitpriceHiddenField");

            totalprice += Convert.ToDecimal(unitpriceHiddenField.Value) * Convert.ToDecimal(quantityTextBox.Text);
        }

        aListMasterDao.GrossValue = totalprice;

        if (isSpecialAppCheckBox.Checked == false)
        {
            aListMasterDao.ActionStatus = "Accepted";
            aListMasterDao.ApprovedBy = Session["LoginName"].ToString();
        }
        else
        {
            aListMasterDao.ActionStatus = "Posted"; 
        }

        aListMasterDao.ApprovedDate = DateTime.Now;

        return aListMasterDao;
    }

    private bool Validation()
    {
        if (companyDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select Manufacturer !!!");
            return false;
        }

        if (dcDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select Distribution Center  !!!");
            dcDropDownList.Focus();
            dcDropDownList.BackColor = Color.GhostWhite;
            return false;
        }



        if (orderDateTextBox.Text == "")
        {
            ShowMessageBox("Order date is required !!!");
            orderDateTextBox.Focus();
            orderDateTextBox.BackColor = Color.GhostWhite;
            return false;
        }

        if (mioDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Mio is Required !!!");
            return false;
        }

        if (regionDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Region is Required !!!");
            return false;
        }

        if (areaDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Area is Required !!!");
            return false;
        }

        //Check ASM Setings

        if (areaDropDownList.SelectedValue != "")
        {
            DataTable aTable = aOrderCreationDal.GetAsmInfo(areaDropDownList.SelectedValue,companyDropDownList.SelectedValue);

            if (aTable.Rows.Count <= 0)
            {
                ShowMessageBox("Please set ASM for this area (" + areaDropDownList.SelectedItem.Text + ") !!!");
                return false;
            }

        }

        //Check ASM Setings

        if (areaDropDownList.SelectedValue != "")
        {
            DataTable aTable = aOrderCreationDal.GetRsmInfo(regionDropDownList.SelectedValue, companyDropDownList.SelectedValue);

            if (aTable.Rows.Count <= 0)
            {
                ShowMessageBox("Please set RSM for this region (" + regionDropDownList.SelectedItem.Text + ") !!!");
                return false;
            }

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

        if (Convert.ToDecimal(previousDueLabel.Text.Trim()) > 0 && isSpecialAppCheckBox.Checked == false)
        {
            ShowMessageBox("Customer has previous due. Special Approval required !!!");
            return false;
        }

        if (Convert.ToDecimal(remainingcrLabel.Text) < 0 && paymentHiddenField.Value != "Cash" && isSpecialAppCheckBox.Checked == false)
        {
            ShowMessageBox("Credit Limit Crossed !!!");
            return false;
        }

        return true;
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Clear();
    }
}