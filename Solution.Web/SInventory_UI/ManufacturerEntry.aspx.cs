using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ManufacturerEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void Clear()
    {
        manufacturerNameTextBox.Text = string.Empty;
        manufacturerAddressTextBox.Text = string.Empty;
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (manufacturerNameTextBox.Text != "" && manufacturerAddressTextBox.Text !="")
        {
            Manufacturer aManufacturer = new Manufacturer()
            {
                ManufacName = manufacturerNameTextBox.Text,
                ManufacAddress= manufacturerAddressTextBox.Text,
                EntryBy = Session["LoginName"].ToString(),
                EntryDate = DateTime.Now
            };

            ManufacturerBLL aManufacturerBll= new ManufacturerBLL();
            MessageLabel.Text = aManufacturerBll.SaveManufacturer(aManufacturer);
            Clear();
        }
        else
        {
            MessageLabel.Text = "Please input data in all Textbox";
        }
    }
    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("ManufacturerView.aspx");
    }
}