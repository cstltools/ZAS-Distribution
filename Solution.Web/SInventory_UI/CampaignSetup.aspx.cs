using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CampaignSetup : System.Web.UI.Page
{

    CampaignDAL aCampaignDal=new CampaignDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitialGrid();
            LoadDropDownList();
            if (Session["MasterId"] !=null)
            {
                masterHiddenField.Value = Session["MasterId"].ToString();
                Session["MasterId"] = null;
                Button1.Text = "Update";
                LoadData(masterHiddenField.Value);
            }
        }
    }

    public void LoadData(string id)
    {
        DataTable dtdata = aCampaignDal.LoadingleData(id);
        if (dtdata.Rows.Count>0)
        {
            companyDropDownList.SelectedValue = dtdata.Rows[0]["CompanyId"].ToString();

            productGridView.DataSource = dtdata;
            productGridView.DataBind();
            for (int i = 0; i < dtdata.Rows.Count; i++)
            {
                CheckBox isActiveCheckBox = (CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox");
                isActiveCheckBox.Checked = (dtdata.Rows[i]["IsActive"].ToString() == "False") ? false : true;
            }
        }
    }

    private void InitialGrid()
    {
        var aDataTable = new DataTable();

        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("ProductId");

        aDataTable.Columns.Add("BonusProductCode");
        aDataTable.Columns.Add("BonusProductName");
        aDataTable.Columns.Add("BonusQuantity");
        aDataTable.Columns.Add("BonusProductId");
        
        aDataTable.Columns.Add("IsActive");

        aDataTable.Columns.Add("FromDate");
        aDataTable.Columns.Add("ToDate");
        aDataTable.Columns.Add("FromTime");
        aDataTable.Columns.Add("ToTime");
        

        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        dataRow["IsActive"] = "0";
        

        aDataTable.Rows.Add(dataRow);

        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();


        if (companyDropDownList.SelectedValue != "")
        {
            foreach (GridViewRow row in productGridView.Rows)
            {
                TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
                AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
                modal.ContextKey = companyDropDownList.SelectedValue;


                TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("bonusproductNameTextBox");
                AjaxControlToolkit.AutoCompleteExtender modala = (AjaxControlToolkit.AutoCompleteExtender)bonusproductNameTextBox.FindControl("productNameTextBox_AutoCompleteExtendera");
                modala.ContextKey = companyDropDownList.SelectedValue;
            }
        }
        

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        AddRowInGrid();
        //IncreasetakenLimit();
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
    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
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
            //ClearRow(rowindex);
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
                    
                    
                    productidHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();
                    

                    


                }
                else
                {
                    ShowMessageBox("Product Not Found");
                    //ClearRow(rowindex);

                }
                if (rowindex != 0)
                {
                    if (
                        ((HiddenField)productGridView.Rows[rowindex - 1].Cells[0].FindControl("productidHiddenField"))
                            .Value ==
                        string.Empty)
                    {
                        ShowMessageBox("Fill Out Previous Field");
                        //ClearRow(rowindex);
                    }
                }
            }
            else
            {
                var productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("productCodeTextBox");
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("productidHiddenField");
                TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("productNameTextBox");
                

                productCodeTextBox.Text = "";
                productNameTextBox.Text = "";
                
                productidHiddenField.Value = "";
                


                ShowMessageBox("Product Already Exist !!!");
            }


        }
    }

    private void GetProductInGridBonus(int rowindex, string productCode)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();
        DataTable aDataTable = new DataTable();
        if (!string.IsNullOrEmpty(productCode))
        {

            //if (ProductExistOrNot(productCode, rowindex))
            {
                aDataTable = _aDcStockReceiveBll.ProductInfo(productCode, companyDropDownList.SelectedValue);

                if (aDataTable.Rows.Count > 0)
                {
                    HiddenField productidHiddenField =
                        (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("bonusproductidHiddenField");
                    TextBox productNameTextBox =
                        (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("bonusproductNameTextBox");
                    productNameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();


                    productidHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();





                }
                else
                {
                    ShowMessageBox("Product Not Found");
                    //ClearRow(rowindex);

                }
                if (rowindex != 0)
                {
                    if (
                        ((HiddenField)productGridView.Rows[rowindex - 1].Cells[0].FindControl("bonusproductidHiddenField"))
                            .Value ==
                        string.Empty)
                    {
                        ShowMessageBox("Fill Out Previous Field");
                        //ClearRow(rowindex);
                    }
                }
            }
            


        }
    }

    private void AddRowInGrid()
    {
        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("IsActive");

        aDataTable.Columns.Add("BonusProductCode");
        aDataTable.Columns.Add("BonusProductName");
        aDataTable.Columns.Add("BonusQuantity");
        aDataTable.Columns.Add("BonusProductId");

        aDataTable.Columns.Add("FromDate");
        aDataTable.Columns.Add("ToDate");
        aDataTable.Columns.Add("FromTime");
        aDataTable.Columns.Add("ToTime");

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
                TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("qtyTextBox");
                
                HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
                dataRow["Quantity"] = quantityTextBox.Text.Trim();
                dataRow["ProductId"] = productidHiddenField.Value.Trim();


                HiddenField bonusproductidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("bonusproductidHiddenField");
                TextBox bonusproductCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("bonusproductCodeTextBox");
                dataRow["BonusProductCode"] = bonusproductCodeTextBox.Text.Trim();
                TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("bonusproductNameTextBox");
                dataRow["BonusProductName"] = bonusproductNameTextBox.Text.Trim();
                TextBox bonusqtyTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("bonusqtyTextBox");
                dataRow["BonusQuantity"] = bonusqtyTextBox.Text.Trim();
                dataRow["BonusProductId"] = bonusproductidHiddenField.Value.Trim();

                CheckBox isActiveCheckBox = (CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox");
                dataRow["IsActive"] = isActiveCheckBox.Checked;


                TextBox fromDateTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("fromDateTextBox");
                dataRow["FromDate"] = fromDateTextBox.Text.Trim();
                TextBox fromtimeTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("fromtimeTextBox");
                dataRow["FromTime"] = fromtimeTextBox.Text.Trim();
                TextBox toDateTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("toDateTextBox");
                dataRow["ToDate"] = toDateTextBox.Text.Trim();
                TextBox totimeTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("totimeTextBox");
                dataRow["ToTime"] = totimeTextBox.Text.Trim();



                aDataTable.Rows.Add(dataRow);
            }
        }
        int sl = aDataTable.Rows.Count;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = Convert.ToString(sl + 1);
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";
        
        aDataTable.Rows.Add(dataRow);


        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        for (int i = 0; i < aDataTable.Rows.Count; i++)
        {
            CheckBox isActiveCheckBox = (CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox");
            isActiveCheckBox.Checked = (aDataTable.Rows[i]["IsActive"].ToString()=="False")?false:true;
        }
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = companyDropDownList.SelectedValue;


            TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("bonusproductNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modala = (AjaxControlToolkit.AutoCompleteExtender)bonusproductNameTextBox.FindControl("bonusproductNameTextBox_AutoCompleteExtendera");
            modala.ContextKey = companyDropDownList.SelectedValue;
        }

        
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btton = (ImageButton)sender;
        GridViewRow currentRow = (GridViewRow)btton.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        DataTable aDataTable = new DataTable();
        aDataTable.Columns.Add("SL");
        aDataTable.Columns.Add("ProductCode");
        aDataTable.Columns.Add("ProductName");
        aDataTable.Columns.Add("Quantity");
        aDataTable.Columns.Add("ProductId");
        aDataTable.Columns.Add("IsActive");

        aDataTable.Columns.Add("BonusProductCode");
        aDataTable.Columns.Add("BonusProductName");
        aDataTable.Columns.Add("BonusQuantity");
        aDataTable.Columns.Add("BonusProductId");

        aDataTable.Columns.Add("FromDate");
        aDataTable.Columns.Add("ToDate");
        aDataTable.Columns.Add("FromTime");
        aDataTable.Columns.Add("ToTime");
        DataRow dataRow;

        if (productGridView.Rows.Count > 0)
        {
            int sl1 = 1;
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                if (i != rowindex)
                {
                    dataRow = aDataTable.NewRow();
                    dataRow["SL"] = Convert.ToString(i + 1);
                    TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");
                    dataRow["ProductCode"] = productCodeTextBox.Text.Trim();
                    TextBox productNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("productNameTextBox");
                    dataRow["ProductName"] = productNameTextBox.Text.Trim();
                    TextBox quantityTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("qtyTextBox");

                    HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("productidHiddenField");
                    dataRow["Quantity"] = quantityTextBox.Text.Trim();
                    dataRow["ProductId"] = productidHiddenField.Value.Trim();


                    HiddenField bonusproductidHiddenField = (HiddenField)productGridView.Rows[i].Cells[0].FindControl("bonusproductidHiddenField");
                    TextBox bonusproductCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("bonusproductCodeTextBox");
                    dataRow["BonusProductCode"] = bonusproductCodeTextBox.Text.Trim();
                    TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[i].Cells[2].FindControl("bonusproductNameTextBox");
                    dataRow["BonusProductName"] = bonusproductNameTextBox.Text.Trim();
                    TextBox bonusqtyTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("bonusqtyTextBox");
                    dataRow["BonusQuantity"] = bonusqtyTextBox.Text.Trim();
                    dataRow["BonusProductId"] = bonusproductidHiddenField.Value.Trim();

                    CheckBox isActiveCheckBox = (CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox");
                    dataRow["IsActive"] = isActiveCheckBox.Checked;


                    TextBox fromDateTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("fromDateTextBox");
                    dataRow["FromDate"] = fromDateTextBox.Text.Trim();
                    TextBox fromtimeTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("fromtimeTextBox");
                    dataRow["FromTime"] = fromtimeTextBox.Text.Trim();
                    TextBox toDateTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("toDateTextBox");
                    dataRow["ToDate"] = toDateTextBox.Text.Trim();
                    TextBox totimeTextBox = (TextBox)productGridView.Rows[i].Cells[4].FindControl("totimeTextBox");
                    dataRow["ToTime"] = totimeTextBox.Text.Trim();
                    aDataTable.Rows.Add(dataRow);
                    sl1 += 1;
                }
            }
        }
        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();
        for (int i = 0; i < aDataTable.Rows.Count; i++)
        {
            CheckBox isActiveCheckBox = (CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox");
            isActiveCheckBox.Checked = (aDataTable.Rows[i]["IsActive"].ToString() == "False") ? false : true;
        }
        if (productGridView.Rows.Count < 1)
        {
            InitialGrid();
        }
        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = companyDropDownList.SelectedValue;


            TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("bonusproductNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modala = (AjaxControlToolkit.AutoCompleteExtender)bonusproductNameTextBox.FindControl("productNameTextBox_AutoCompleteExtendera");
            modala.ContextKey = companyDropDownList.SelectedValue;
        }
        

    }


    protected void bonusproductCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();

        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("bonusproductCodeTextBox");

        string productCode = productCodeTextBox.Text.Trim();
        GetProductInGridBonus(rowindex, productCode);   
    }

    private void LoadDropDownList()
    {
        aCampaignDal.GetDdlCompany(companyDropDownList);
        
    }

    protected void bonusproductNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("bonusproductNameTextBox");
        string productName = productNameTextBox.Text.Trim();

        if (productName.Contains(':'))
        {
            string[] productInfo = productName.Split(':');

            TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("bonusproductCodeTextBox");

            productCodeTextBox.Text = productInfo[0];
            //productNameTextBox.Text = productInfo[1];
            string productCode = productCodeTextBox.Text.Trim();
            GetProductInGridBonus(rowindex, productCode);

        }
        else
        {
            ShowMessageBox("Input Correct Data!!");
            //ClearRow(rowindex);
        }

    }

    public void Save()
    {
        CampaignDAO aCampaignDao = new CampaignDAO()
        {
            EntryBy = Session["LoginName"].ToString(),
            EntryDate = DateTime.Now,
            CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue)
        };
        CampaignBLL aCampaignBll = new CampaignBLL();
        int id = aCampaignBll.SaveCampaignMast(aCampaignDao);
        if (id > 0)
        {
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                CampaignDetailDAO aCampaignDetailDao = new CampaignDetailDAO();
                aCampaignDetailDao.CampgainMasterId = id;
                aCampaignDetailDao.PointProductCode = ((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text;
                aCampaignDetailDao.Quantity = Convert.ToInt32(((TextBox)productGridView.Rows[i].FindControl("qtyTextBox")).Text);

                aCampaignDetailDao.BonusProductCode = ((TextBox)productGridView.Rows[i].FindControl("bonusproductCodeTextBox")).Text;
                aCampaignDetailDao.BonusQuantity = Convert.ToInt32(((TextBox)productGridView.Rows[i].FindControl("bonusQtyTextBox")).Text);

                aCampaignDetailDao.IsActive = Convert.ToBoolean(((CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox")).Checked);

                aCampaignDetailDao.FromDate = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("fromDateTextBox")).Text);
                aCampaignDetailDao.FromTime = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("fromtimeTextBox")).Text).TimeOfDay;

                aCampaignDetailDao.ToDate = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("toDateTextBox")).Text);
                aCampaignDetailDao.ToTime = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("totimeTextBox")).Text).TimeOfDay;
                aCampaignDetailDao.IsSame =
                    (((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text ==
                     ((TextBox)productGridView.Rows[i].FindControl("bonusproductCodeTextBox")).Text)
                        ? true
                        : false;
                aCampaignDal.SaveCampaignDetail(aCampaignDetailDao);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                           "alert",
                           "alert('Opearation Successfully Done...');window.location ='CampaignSetup.aspx';",
                           true);
        }
    }

    
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (masterHiddenField.Value ==string.Empty)
        {
            Save();
       //     InitialGrid();
         //   companyDropDownList.SelectedIndex = 0;
        }
        else
        {
            Update();
            //InitialGrid();
            //companyDropDownList.SelectedIndex = 0;
        }
    }

    public void Update()
    {
        CampaignDAO aCampaignDao = new CampaignDAO()
        {
            CampgainMasterId = Convert.ToInt32(masterHiddenField.Value),
            EntryBy = Session["LoginName"].ToString(),
            EntryDate = DateTime.Now,
            CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue)
        };
        CampaignBLL aCampaignBll = new CampaignBLL();
        bool id = aCampaignDal.UpdateCampaignMaster(aCampaignDao);
        if (id)
        {
            aCampaignDal.DeleteCampaignDetail(masterHiddenField.Value);
            for (int i = 0; i < productGridView.Rows.Count; i++)
            {
                CampaignDetailDAO aCampaignDetailDao = new CampaignDetailDAO();
                aCampaignDetailDao.CampgainMasterId = Convert.ToInt32(masterHiddenField.Value);
                aCampaignDetailDao.PointProductCode = ((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text;
                aCampaignDetailDao.Quantity = Convert.ToInt32(((TextBox)productGridView.Rows[i].FindControl("qtyTextBox")).Text);

                aCampaignDetailDao.BonusProductCode = ((TextBox)productGridView.Rows[i].FindControl("bonusproductCodeTextBox")).Text;
                aCampaignDetailDao.BonusQuantity = Convert.ToInt32(((TextBox)productGridView.Rows[i].FindControl("bonusQtyTextBox")).Text);

                aCampaignDetailDao.IsActive = Convert.ToBoolean(((CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox")).Checked);

                aCampaignDetailDao.FromDate = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("fromDateTextBox")).Text);
                aCampaignDetailDao.FromTime = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("fromtimeTextBox")).Text).TimeOfDay;

                aCampaignDetailDao.ToDate = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("toDateTextBox")).Text);
                aCampaignDetailDao.ToTime = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("totimeTextBox")).Text).TimeOfDay;
                aCampaignDetailDao.IsSame =
                    (((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text ==
                     ((TextBox)productGridView.Rows[i].FindControl("bonusproductCodeTextBox")).Text)
                        ? true
                        : false;
                aCampaignDal.SaveCampaignDetail(aCampaignDetailDao);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(),
                          "alert",
                          "alert('Opearation Successfully Done...');window.location ='CampaignSetup.aspx';",
                          true);
        }   
    }

    protected void companyDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyDropDownList.SelectedValue != "")
        {
            foreach (GridViewRow row in productGridView.Rows)
            {
                TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
                AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
                modal.ContextKey = companyDropDownList.SelectedValue;

               
            }

            foreach (GridViewRow row in productGridView.Rows)
            {
                TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("bonusproductNameTextBox");
                AjaxControlToolkit.AutoCompleteExtender modala = (AjaxControlToolkit.AutoCompleteExtender)bonusproductNameTextBox.FindControl("bonusproductNameTextBox_AutoCompleteExtendera");
                modala.ContextKey = companyDropDownList.SelectedValue;
            }

            

        }
    }

    protected void areaInfoNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CampaignView.aspx");
    }
}
