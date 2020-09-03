using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using System.Data;
using Library.DAO.SInventory_Entities;
using System.Globalization;

public partial class SInventory_UI_DcWiseAreaAlocation : System.Web.UI.Page
{
    DcWiseAreaAloBll aDcWiseBll = new DcWiseAreaAloBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            LoadArea();
        }
    }

    private void LoadDropdownList()
    {
        aDcWiseBll.LoadCompanyDropdownList(companyDropDownList);
    }

    private void LoadArea()
    {
        DataTable dt = aDcWiseBll.LoadAreaView();

        areaCheckBoxList.DataValueField = "AreaId";
        areaCheckBoxList.DataTextField = "AreaName";
        areaCheckBoxList.DataSource = dt;
        areaCheckBoxList.DataBind();
    }


    private void Clear()
    {
        companyDropDownList.SelectedValue = "";
        distributionDropDownList.Items.Clear(); 
        UncheckAllCheckBox();
    }


    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private bool Validation()
    {

        if (distributionDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please select a DC !!!");
            return false;
        }

        int count = 0;

        for (int i = 0; i < areaCheckBoxList.Items.Count; i++)
        {
            if (areaCheckBoxList.Items[i].Selected == false)
            {
                count++;
            }

            if (count > 0)
            {
                break;
            }
        }

        if (count == areaCheckBoxList.Items.Count)
        {
            ShowMessageBox("Please check at least one area !!!");
            return false;
        }
        return true;
    }




    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            List<DcWiseAreaAloDao> areaListForSave = new List<DcWiseAreaAloDao>();
            List<DcWiseAreaAloDao> areaListForUpdate = new List<DcWiseAreaAloDao>();

            DataTable aDataTable =
                aDcWiseBll.LoadDcWiseAreaAloInfo(Convert.ToInt32(distributionDropDownList.SelectedValue));

            for (int i = 0; i < areaCheckBoxList.Items.Count; i++)
            {
                if (!AllocatedOrNot(areaCheckBoxList.Items[i].Value, aDataTable))
                {
                    DcWiseAreaAloDao adcwise;
                    if (areaCheckBoxList.Items[i].Selected)
                    {
                        adcwise = new DcWiseAreaAloDao();

                        adcwise.DCId = Convert.ToInt32(distributionDropDownList.SelectedValue);
                        adcwise.AreaId = Convert.ToInt32(areaCheckBoxList.Items[i].Value);
                        adcwise.IsActive = true;
                        adcwise.EntryBy = Session["LoginName"].ToString();
                        adcwise.EntryDate = DateTime.Now;

                        areaListForSave.Add(adcwise);
                    }
                }

                else
                {
                    DcWiseAreaAloDao adcwise;

                    if (!areaCheckBoxList.Items[i].Selected)
                    {
                        adcwise = new DcWiseAreaAloDao();

                        for (int j = 0; j < aDataTable.Rows.Count; j++)
                        {
                            if (aDataTable.Rows[j].Field<Int32>("AreaId") == Convert.ToInt32(areaCheckBoxList.Items[i].Value))
                            {
                                adcwise.DcWiseAreaId = aDataTable.Rows[j].Field<Int32>("DcWiseAreaId");
                            }
                        }

                        adcwise.CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue);
                        adcwise.DCId = Convert.ToInt32(distributionDropDownList.SelectedValue);
                        adcwise.AreaId = Convert.ToInt32(areaCheckBoxList.Items[i].Value);
                        adcwise.IsActive = false;
                        adcwise.UpdateBy = Session["LoginName"].ToString();
                        adcwise.UpdateDate = DateTime.Now;

                        areaListForUpdate.Add(adcwise);
                    }
                }
            }

           bool save = aDcWiseBll.SaveDcWiseAreaInfo(areaListForSave);
           bool update = aDcWiseBll.UpdateDcWiseAreaInfo(areaListForUpdate);

            if (save && update)
            {
                ShowMessageBox("Data Saved Successfully!!!");
                Clear();
            }
        }
    }

    private bool AllocatedOrNot(string value, DataTable aDataTable)
    {

        if (aDataTable.Rows.Count > 0)
        {

            for (int i = 0; i < aDataTable.Rows.Count; i++)
            {
                if (aDataTable.Rows[i].Field<Int32>("AreaId") == Convert.ToInt32(value))
                {
                    return true;
                }
            }

        }

        return false;
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    //under
    protected void companyDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        aDcWiseBll.LoadDC(distributionDropDownList, companyDropDownList.SelectedValue);
        UncheckAllCheckBox();
    }

    //Select Value Checked Method

    protected void distributionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UncheckAllCheckBox();

        if (distributionDropDownList.SelectedValue != "") 
        {
            DataTable aDataTable = aDcWiseBll.LoadDcWiseAreaAloByDc(distributionDropDownList.SelectedValue);
           
            if (aDataTable.Rows.Count > 0) 
            {
                for (int i = 0; i < areaCheckBoxList.Items.Count; i++)
                {

                    for (int j = 0; j < aDataTable.Rows.Count; j++)
                    {
                        if (areaCheckBoxList.Items[i].Value == aDataTable.Rows[j].Field<Int32>("AreaId").ToString(CultureInfo.InvariantCulture))
                        {
                            areaCheckBoxList.Items[i].Selected = true;
                        }
                    }
                }
            }
        }
    }

    private void UncheckAllCheckBox()
    {
       
        for (int i = 0; i < areaCheckBoxList.Items.Count; i++)
        {
            areaCheckBoxList.Items[i].Selected = false;
        }
    }
}