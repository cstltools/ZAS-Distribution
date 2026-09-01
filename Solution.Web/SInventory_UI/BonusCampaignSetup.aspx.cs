using System;
using System.Activities.Statements;
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

    BonusCampaignDal aCampaignDal = new BonusCampaignDal();

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
                saveButton.Text = "Update";
                LoadData(masterHiddenField.Value);
            }
        }
    }

    public void LoadData(string id)
    {
        DataTable dtdata = aCampaignDal.LoadBonusCampaignDetails(id);

        if (dtdata.Rows.Count > 0)
        {
            ddlWingsName.SelectedValue = dtdata.Rows[0]["CompanyId"].ToString();
            ddlCompany_OnSelectedIndexChanged(null,null);

            ddlBonusProductName.SelectedValue = dtdata.Rows[0]["BonusProductId"].ToString();
            tbxBonusQuantity.Text = dtdata.Rows[0]["BonusProductQuantity"].ToString();

            fromDateTextBox.Text = Convert.ToDateTime(dtdata.Rows[0]["FromDate"]).ToString("yyyy-MM-ddTHH:mm");
            toDateTextBox.Text = Convert.ToDateTime(dtdata.Rows[0]["Todate"]).ToString("yyyy-MM-ddTHH:mm");

            tbxCampaignName.Text = dtdata.Rows[0]["CampaignName"].ToString();
            tbxCampaignDescription.Text = dtdata.Rows[0]["CampaignDesc"].ToString();

            cbxIsActive.Checked = dtdata.Rows[0]["IsActive"] != DBNull.Value && Convert.ToBoolean(dtdata.Rows[0]["IsActive"]);


            productGridView.DataSource = dtdata;
            productGridView.DataBind();

            //for (int i = 0; i < dtdata.Rows.Count; i++)
            //{
            //    CheckBox isActiveCheckBox = (CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox");
            //    isActiveCheckBox.Checked = (dtdata.Rows[i]["IsActive"].ToString() == "False") ? false : true;
            //}
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

        DataRow dataRow;

        dataRow = aDataTable.NewRow();

        dataRow["SL"] = "1";
        dataRow["ProductCode"] = "";
        dataRow["ProductName"] = "";

        

        aDataTable.Rows.Add(dataRow);

        productGridView.DataSource = null;
        productGridView.DataBind();
        productGridView.DataSource = aDataTable;
        productGridView.DataBind();


        if (ddlWingsName.SelectedValue != "")
        {
            foreach (GridViewRow row in productGridView.Rows)
            {
                TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
                AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
                modal.ContextKey = ddlWingsName.SelectedValue;
              
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
        //for (int i = 0; i < productGridView.Rows.Count; i++)
        //{
        //    var productCodeTextBox = (TextBox)productGridView.Rows[i].Cells[1].FindControl("productCodeTextBox");

        //    if (productCodeTextBox.Text.Trim() == productCode && i != rowindex)
        //    {
        //        return false;
        //    }
        //}

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
                aDataTable = _aDcStockReceiveBll.ProductInfo(productCode, ddlWingsName.SelectedValue);

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
                //aDataTable = _aDcStockReceiveBll.ProductInfo(productCode, companyDropDownList.SelectedValue);

                if (aDataTable.Rows.Count > 0)
                {
                    //HiddenField productidHiddenField =
                    //    (HiddenField)productGridView.Rows[rowindex].Cells[0].FindControl("bonusproductidHiddenField");
                    //TextBox productNameTextBox =
                    //    (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("bonusproductNameTextBox");
                    //productNameTextBox.Text = aDataTable.Rows[0]["ProductName"].ToString();


                    //productidHiddenField.Value = aDataTable.Rows[0]["ProductId"].ToString();





                }
                else
                {
                    ShowMessageBox("Product Not Found");
                    //ClearRow(rowindex);

                }
                if (rowindex != 0)
                {
                    //if (
                    //    ((HiddenField)productGridView.Rows[rowindex - 1].Cells[0].FindControl("bonusproductidHiddenField"))
                    //        .Value ==
                    //    string.Empty)
                    //{
                    //    ShowMessageBox("Fill Out Previous Field");
                    //    //ClearRow(rowindex);
                    //}
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


        

        foreach (GridViewRow row in productGridView.Rows)
        {
            TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
            modal.ContextKey = ddlWingsName.SelectedValue;
           
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
            modal.ContextKey = ddlWingsName.SelectedValue;
        }
        

    }


    protected void bonusproductCodeTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DCStoreBLL _aDcStockReceiveBll = new DCStoreBLL();

        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        //TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("bonusproductCodeTextBox");

        //string productCode = productCodeTextBox.Text.Trim();
        //GetProductInGridBonus(rowindex, productCode);   
    }

    private void LoadDropDownList()
    {
        aCampaignDal.LoadCompanyInfo(ddlWingsName);
        
    }

    protected void bonusproductNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox TextBox = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)TextBox.Parent.Parent;
        int rowindex = 0;
        rowindex = currentRow.RowIndex;

        //TextBox productNameTextBox = (TextBox)productGridView.Rows[rowindex].Cells[2].FindControl("bonusproductNameTextBox");
        //string productName = productNameTextBox.Text.Trim();

        //if (productName.Contains(':'))
        //{
        //    string[] productInfo = productName.Split(':');

        //    TextBox productCodeTextBox = (TextBox)productGridView.Rows[rowindex].Cells[1].FindControl("bonusproductCodeTextBox");

        //    productCodeTextBox.Text = productInfo[0];
        //    //productNameTextBox.Text = productInfo[1];
        //    string productCode = productCodeTextBox.Text.Trim();
        //    GetProductInGridBonus(rowindex, productCode);

        //}
        //else
        //{
        //    ShowMessageBox("Input Correct Data!!");
        //    //ClearRow(rowindex);
        //}

    }

    

    
    protected void Button1_Click(object sender, EventArgs e)
    {

        bool status = false;

        if (masterHiddenField.Value == string.Empty)
        {
            if (Validation())
            {
                // Master 

                BonusCampaignMasterDao aCampaignDao = new BonusCampaignMasterDao();

                aCampaignDao.CompanyId = Convert.ToInt32(ddlWingsName.SelectedValue);
                aCampaignDao.BonusProductId = Convert.ToInt32(ddlBonusProductName.SelectedValue);
                aCampaignDao.BonusProductQuantity = Convert.ToDecimal(tbxBonusQuantity.Text);
                aCampaignDao.FromDate = Convert.ToDateTime(fromDateTextBox.Text);
                aCampaignDao.Todate = Convert.ToDateTime(toDateTextBox.Text);
                aCampaignDao.IsActive = cbxIsActive.Checked;
                aCampaignDao.CampaignName = tbxCampaignName.Text;
                aCampaignDao.CampaignDesc = tbxCampaignDescription.Text;
                aCampaignDao.EntryBy = Session["LoginName"].ToString();

                // Details

                List<BonusCampaignDetailsDao> aDetailsDaos = new List<BonusCampaignDetailsDao>();

                for (int i = 0; i < productGridView.Rows.Count; i++)
                {
                    BonusCampaignDetailsDao aDao = new BonusCampaignDetailsDao();

                    HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].FindControl("productidHiddenField");
                    TextBox qtyTextBox = (TextBox)productGridView.Rows[i].FindControl("qtyTextBox");

                    aDao.PointProductId = Convert.ToInt32(productidHiddenField.Value);
                    aDao.Quantity = Convert.ToDecimal(qtyTextBox.Text);

                    aDetailsDaos.Add(aDao);
                }

                // Call Save Method

               status = aCampaignDal.SaveBonusCampaign(aCampaignDao, aDetailsDaos);
            }

            if (status)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                           "alert",
                           "alert('Opearation Successfully Done...');window.location ='BonusCampaignSetup.aspx';",
                           true);
            }
            else
            {
                ShowMessageBox("❌ Failed to save data. Please try again.");
            }
           
        }
        else
        {
            if (Validation())
            {
                // Master 

                BonusCampaignMasterDao aCampaignDao = new BonusCampaignMasterDao();

                aCampaignDao.CampgainMasterId = Convert.ToInt32(masterHiddenField.Value);
                aCampaignDao.CompanyId = Convert.ToInt32(ddlWingsName.SelectedValue);
                aCampaignDao.BonusProductId = Convert.ToInt32(ddlBonusProductName.SelectedValue);
                aCampaignDao.BonusProductQuantity = Convert.ToDecimal(tbxBonusQuantity.Text);
                aCampaignDao.FromDate = Convert.ToDateTime(fromDateTextBox.Text);
                aCampaignDao.Todate = Convert.ToDateTime(toDateTextBox.Text);
                aCampaignDao.IsActive = cbxIsActive.Checked;
                aCampaignDao.CampaignName = tbxCampaignName.Text;
                aCampaignDao.CampaignDesc = tbxCampaignDescription.Text;
                aCampaignDao.EntryBy = Session["LoginName"].ToString();

                // Details

                List<BonusCampaignDetailsDao> aDetailsDaos = new List<BonusCampaignDetailsDao>();

                for (int i = 0; i < productGridView.Rows.Count; i++)
                {
                    BonusCampaignDetailsDao aDao = new BonusCampaignDetailsDao();

                    HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].FindControl("productidHiddenField");
                    TextBox qtyTextBox = (TextBox)productGridView.Rows[i].FindControl("qtyTextBox");

                    aDao.PointProductId = Convert.ToInt32(productidHiddenField.Value);
                    aDao.Quantity = Convert.ToDecimal(qtyTextBox.Text);

                    aDetailsDaos.Add(aDao);
                }

                // Call Save Method

                status = aCampaignDal.SaveBonusCampaign(aCampaignDao, aDetailsDaos);
            }

            if (status)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                           "alert",
                           "alert('Opearation Successfully Done...');window.location ='BonusCampaignSetup.aspx';",
                           true);
            }
            else
            {
                ShowMessageBox("❌ Failed to save data. Please try again.");
            }
        }
    }

    private bool Validation()
    {
        if (ddlWingsName.SelectedValue == "")
        {
            ShowMessageBox("Please Select Wings Name !!");
            return false;
        } 
        
        if (ddlBonusProductName.SelectedValue == "")
        {
            ShowMessageBox("Please Select Bonus Product Name !!");
            return false;
        }

        if (tbxBonusQuantity.Text.Trim() == "")
        {
            ShowMessageBox("Please Select Bonus Quantity !!");
            return false;
        }

        if (tbxBonusQuantity.Text.Trim() == "")
        {
            ShowMessageBox("Please Select Bonus Quantity !!");
            return false;
        }

        if (fromDateTextBox.Text.Trim() == "")
        {
            ShowMessageBox("Please Select From Date !!");
            return false;
        }

        if (toDateTextBox.Text.Trim() == "")
        {
            ShowMessageBox("Please Select To Date !!");
            return false;
        }

        if (tbxCampaignName.Text.Trim() == "")
        {
            ShowMessageBox("Please Select Campaign Name !!");
            return false;
        }

        for (int i = 0; i < productGridView.Rows.Count; i++)
        {
           TextBox productCodeTextBox = (TextBox)productGridView.Rows[i].FindControl("productCodeTextBox");
           HiddenField productidHiddenField = (HiddenField)productGridView.Rows[i].FindControl("productidHiddenField");
           TextBox qtyTextBox = (TextBox)productGridView.Rows[i].FindControl("qtyTextBox");

           if (productidHiddenField.Value.Trim() == "")
           {
               ShowMessageBox("Please Select Point Poroduct !!");
               return false;
           }

           if (qtyTextBox.Text == "" || Convert.ToDecimal(qtyTextBox.Text) == 0)
           {
               ShowMessageBox("Please Select Point Poroduct Quantity !!");
               return false;
           }
        }

        return true;
    }

    public void Update()
    {
        //CampaignDAO aCampaignDao = new CampaignDAO()
        //{
        //    CampgainMasterId = Convert.ToInt32(masterHiddenField.Value),
        //    EntryBy = Session["LoginName"].ToString(),
        //    EntryDate = DateTime.Now,
        //    CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue)
        //};
        //CampaignBLL aCampaignBll = new CampaignBLL();
        //bool id = aCampaignDal.UpdateCampaignMaster(aCampaignDao);
        //if (id)
        //{
        //    aCampaignDal.DeleteCampaignDetail(masterHiddenField.Value);
        //    for (int i = 0; i < productGridView.Rows.Count; i++)
        //    {
        //        CampaignDetailDAO aCampaignDetailDao = new CampaignDetailDAO();
        //        aCampaignDetailDao.CampgainMasterId = Convert.ToInt32(masterHiddenField.Value);
        //        aCampaignDetailDao.PointProductCode = ((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text;
        //        aCampaignDetailDao.Quantity = Convert.ToInt32(((TextBox)productGridView.Rows[i].FindControl("qtyTextBox")).Text);

        //        aCampaignDetailDao.BonusProductCode = ((TextBox)productGridView.Rows[i].FindControl("bonusproductCodeTextBox")).Text;
        //        aCampaignDetailDao.BonusQuantity = Convert.ToInt32(((TextBox)productGridView.Rows[i].FindControl("bonusQtyTextBox")).Text);

        //        aCampaignDetailDao.IsActive = Convert.ToBoolean(((CheckBox)productGridView.Rows[i].FindControl("isActiveCheckBox")).Checked);

        //        aCampaignDetailDao.FromDate = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("fromDateTextBox")).Text);
        //        aCampaignDetailDao.FromTime = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("fromtimeTextBox")).Text).TimeOfDay;

        //        aCampaignDetailDao.ToDate = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("toDateTextBox")).Text);
        //        aCampaignDetailDao.ToTime = Convert.ToDateTime(((TextBox)productGridView.Rows[i].FindControl("totimeTextBox")).Text).TimeOfDay;
        //        aCampaignDetailDao.IsSame =
        //            (((TextBox)productGridView.Rows[i].FindControl("productCodeTextBox")).Text ==
        //             ((TextBox)productGridView.Rows[i].FindControl("bonusproductCodeTextBox")).Text)
        //                ? true
        //                : false;
        //        aCampaignDal.SaveCampaignDetail(aCampaignDetailDao);
        //    }

        //    ScriptManager.RegisterStartupScript(this, this.GetType(),
        //                  "alert",
        //                  "alert('Opearation Successfully Done...');window.location ='CampaignSetup.aspx';",
        //                  true);
        //}   
    }

    protected void companyDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //if (companyDropDownList.SelectedValue != "")
        //{
        //    foreach (GridViewRow row in productGridView.Rows)
        //    {
        //        TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
        //        AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
        //        modal.ContextKey = companyDropDownList.SelectedValue;


        //    }

        //    foreach (GridViewRow row in productGridView.Rows)
        //    {
        //        TextBox bonusproductNameTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("bonusproductNameTextBox");
        //        AjaxControlToolkit.AutoCompleteExtender modala = (AjaxControlToolkit.AutoCompleteExtender)bonusproductNameTextBox.FindControl("bonusproductNameTextBox_AutoCompleteExtendera");
        //        modala.ContextKey = companyDropDownList.SelectedValue;
        //    }



        //}
    }

    protected void areaInfoNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("BonusCampaignSetupView.aspx");
    }

    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWingsName.SelectedValue != "")
        {
            foreach (GridViewRow row in productGridView.Rows)
            {
                TextBox productTextBox = (TextBox)productGridView.Rows[row.RowIndex].Cells[3].FindControl("productNameTextBox");
                AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)productTextBox.FindControl("productNameTextBox_AutoCompleteExtender");
                modal.ContextKey = ddlWingsName.SelectedValue;


            }
            aCampaignDal.LoadProductInfo(ddlBonusProductName, Convert.ToInt32(ddlWingsName.SelectedValue));
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("BonusCampaignSetup.aspx");
    }
}
