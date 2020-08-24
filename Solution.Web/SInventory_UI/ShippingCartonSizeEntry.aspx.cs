using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ShippingCartonSizeEntry : System.Web.UI.Page
{
    ShippingCartonSizeBLL aShippingCartonSizeBll = new ShippingCartonSizeBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // aShippingCartonSizeBll.LoadProduct(productNameDropDownList);
        }
    }
    protected void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }
    private void Clear()
    {
        caseQtyTextBox.Text = string.Empty;
        pcsPerCaseTextBox.Text = string.Empty;
       // productNameDropDownList.SelectedIndex = 0;

    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (caseQtyTextBox.Text != "")
        {
            ShippingCartonSize aShippingCartonSize= new ShippingCartonSize()
            {
              //  ProductCode = productNameDropDownList.SelectedValue,
                CaseQty = caseQtyTextBox.Text,
                PcsPerCase = pcsPerCaseTextBox.Text,
                
            };
            ShippingCartonSizeBLL aShippingCartonSizeBll = new ShippingCartonSizeBLL();
            
            if (aShippingCartonSizeBll.SaveShippingCartonSize(aShippingCartonSize))
            {
                ShowMessageBox("Data Save Successfully");
                Clear();
            }
            else
            {
                ShowMessageBox("Case for Product Already Exist");
            }
            
            
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }
    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ShippingCartonSizeView.aspx");
    }
}