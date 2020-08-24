using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_PackSizeEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void Clear()
    {
        packsizeNameTextBox.Text = string.Empty;
        
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (packsizeNameTextBox.Text != "" )
        {
            PackSize aPackSize= new PackSize()
            {
                PackSizeName = packsizeNameTextBox.Text,
                
            };
            PackSizeBLL aPackSizeBll = new PackSizeBLL();
            MessageLabel.Text = aPackSizeBll.SavePackSize(aPackSize);
            Clear();
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }
    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("PackSizeView.aspx");
    }
}