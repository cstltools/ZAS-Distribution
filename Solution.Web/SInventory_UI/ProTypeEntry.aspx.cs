using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ProTypeEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void Clear()
    {
        proTypeNameTextBox.Text = string.Empty;
        
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (proTypeNameTextBox.Text != "")
        {
            ProType aProType= new ProType()
            {
                ProTypeName = proTypeNameTextBox.Text,
                
            };
            ProTypeBLL aProTypeBll = new ProTypeBLL();

            MessageLabel.Text = aProTypeBll.SaveProType(aProType);
            Clear();
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }
    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ProTypeView.aspx");
    }
}