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

public partial class SInventory_UI_DestroyProduct : System.Web.UI.Page
{
    DestroyProductDal aDal = new DestroyProductDal();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            LoadDropDownList();
            if (Session["DcFreezeId"] != null)
            {
                hfHiddenField.Value = Session["DcFreezeId"].ToString();
                LoadProductList();
            }
            else
            {
                Response.Redirect("DestroyProductList.aspx");
            }

        }

    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void LoadDropDownList()
    {
        aDal.UserDdl(userDropDownList);
    }

    private void LoadProductList()
    {

        if (hfHiddenField.Value != "")
        {
            DataTable aTable = aDal.GetFreezeStockDataByCondition(GenerateParameter());

            if (aTable.Rows.Count > 0)
            {
                loadGridView.DataSource = aTable;
                loadGridView.DataBind();
            }
            else
            {
                loadGridView.DataSource = null;
                loadGridView.DataBind();
            }
        }
        else
        {
            Response.Redirect("DestroyProductList.aspx");
        }
        
    }

    private string GenerateParameter()
    {
        string param = "";


        if (hfHiddenField.Value != "")
        {
            param = param + "  AND F.DCStoreFreezeId = " + hfHiddenField.Value ;
        }

        return param;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (SaveOperation())
            {

                // Post Journal

                //aDal.PostJournal();

                ShowMessageBox("Stock Out Successfully Done !!!!");
                Clear();
            }
        }
    }

    private bool SaveOperation()
    {
        bool retVal = false;
        try
        {
            retVal = aDal.SaveDestroyProductInfo(PrepareDataForSaveNew());

        }
        catch (Exception ex)
        {
            retVal = false;
            throw ex;
        }

        return retVal;
    }

    private DestroyProductDao PrepareDataForSaveNew()
    {
        DestroyProductDao aProductDao = new DestroyProductDao();

        aProductDao.DcFreezeId = Convert.ToInt32(hfHiddenField.Value);
        aProductDao.StockOutDate = Convert.ToDateTime(dateTextBox.Text);
        aProductDao.StockOutBy = Convert.ToInt32(userDropDownList.SelectedValue);
        aProductDao.Reason = stockoutReasonTextBox.Text;

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {

            if (loadGridView.Rows[i].Cells[5].Text.Trim() != "")
            {
                aProductDao.StockOutQty = Convert.ToDecimal(loadGridView.Rows[i].Cells[5].Text.Trim());
            }
            else
            {
                aProductDao.StockOutQty = Convert.ToDecimal(0);
            }

            if (loadGridView.Rows[i].Cells[6].Text.Trim() != "")
            {
                aProductDao.StockOutAmount = Convert.ToDecimal(loadGridView.Rows[i].Cells[6].Text.Trim());
            }
            else
            {
                aProductDao.StockOutAmount = Convert.ToDecimal(0);
            }
            
        }

        
        aProductDao.EntryBy = Session["LoginName"].ToString();
        aProductDao.EntryDate = DateTime.Now;

        return aProductDao;
    }

    private void Clear()
    {
        dateTextBox.Text = "";
        userDropDownList.SelectedValue = "";
        stockoutReasonTextBox.Text = "";
        loadGridView.DataSource = null;
        loadGridView.DataBind();

       
    }

    private bool Validation()
    {
        if (dateTextBox.Text == "")
        {
            ShowMessageBox("Please Insert Stock Out Date !!!!");
            return false;
        }

        if (userDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select Stock Out By (Name) !!!!");
            return false;
        }


        if (stockoutReasonTextBox.Text == "")
        {
            ShowMessageBox("Please Insert Stock Out Reason !!!!");
            return false;
        }

        if (loadGridView.Rows.Count == 0)
        {
            ShowMessageBox("Please add  product on the list !!!!");
            return false;
        }

        return true;
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void backLinkButton_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("DestroyProductList.aspx");
    }
}