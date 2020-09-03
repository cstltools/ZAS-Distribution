using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_AreaInformationView : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    CampaignDAL aCampaignDal= new CampaignDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDownList();
        }
    }


    private void LoadDropDownList()
    {
        aCampaignDal.GetDdlCompany(companyDropDownList);

    }

    private string generateParam()
    {
        string Param = " ";


        if (companyDropDownList.SelectedValue != "")
        {
            Param = Param + " AND tblBonusCampaignMaster.CompanyId='" + companyDropDownList.SelectedValue + "' ";
        }



        if (fromDateTextBox.Text != string.Empty && toDateTextBox.Text != string.Empty)
        {
            Param = Param + " AND tblBonusCampaignMaster.EntryDate BETWEEN '" + fromDateTextBox.Text + "' AND '" + toDateTextBox.Text + "' ";
        }
        if (fromDateTextBox.Text != string.Empty && toDateTextBox.Text == string.Empty)
        {
            Param = Param + " AND tblBonusCampaignMaster.EntryDate BETWEEN '" + fromDateTextBox.Text + "' AND '" + DateTime.Now.ToString("dd-MMM-yyyy") + "' ";
        }

        if (fromDateTextBox.Text == string.Empty && toDateTextBox.Text != string.Empty)
        {
            Param = Param + " AND tblBonusCampaignMaster.EntryDate BETWEEN '" + toDateTextBox.Text + "' AND '" + toDateTextBox.Text + "' ";
        }
        return Param;
    }

    private void AreaLoad()
    {
        if (companyDropDownList.SelectedValue!="")
        {
            aDataTable = aCampaignDal.LoadAllData(generateParam());
            loadGridView.DataSource = aDataTable;
            loadGridView.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                             "alert",
                             "alert('Please Select a Company');",
                             true);
        }
      
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            string areaId = loadGridView.DataKeys[rowindex][0].ToString();
            Session["MasterId"] = areaId;
            Response.Redirect("CampaignSetup.aspx");
        }
    }

    private void PopUp(string Id)
    {
        string url = "AreaInformationEdit.aspx?ID=" + Id;
        string fullURL = "window.open('" + url + "', '_blank', 'height=600,width=700,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }

    protected void areaInfoNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CampaignSetup.aspx");
    }
    protected void areaReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        AreaLoad();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        AreaLoad();
      
    }
}