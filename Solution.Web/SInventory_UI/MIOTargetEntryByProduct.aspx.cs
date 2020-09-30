using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MIOTargetEntryByProduct : System.Web.UI.Page
{
    MiaTargetDAL aReportDal = new MiaTargetDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
        }
    }

    private void LoadDropdownList()
    {

        aReportDal.LoadGroupInfo(groupDropDownList);
        groupDropDownList.SelectedValue = 1.ToString(CultureInfo.InvariantCulture);

        if (groupDropDownList.SelectedValue != "")
        {
            aReportDal.LoadCompanyInfo(companyNameDropDownList, groupDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
            mioDropDownList.Items.Clear();
        }


    }

    private void Clear()
    {
        companyNameDropDownList.SelectedValue = "";
        periodDropDownList.SelectedIndex = 0;
        mioDropDownList.Items.Clear();

        loadGridView.DataSource = null;
        loadGridView.DataBind();
            

    }

    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {

        if (companyNameDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Company!!");
            return false;
        }

        if (mioDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select MIA!!");
            return false;
        }

        
        int count = 0;

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox) loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (chkBoxRows.Checked)
            {
                count++;
            }

            if (count > 0)
            {
                break;
            }
        }

        if (count == 0)
        {
            showMessageBox("You should select at least one product !!!");
            return false;
        }


        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox chkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

            if (chkBoxRows.Checked)
            {
                if (((TextBox)loadGridView.Rows[i].Cells[4].FindControl("returnQtyTextBox")).Text == "")
                {
                    ((TextBox)loadGridView.Rows[i].Cells[4].FindControl("returnQtyTextBox")).Focus();
                    
                    showMessageBox("You should select target quantity !!!");
                    return false;
                }
            }

        }



        return true;
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {

            aReportDal.DeleteExistingData(DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
                    periodDropDownList.SelectedValue, mioDropDownList.SelectedValue);

            MIOTargetProductWise aMiaTarget;

            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox chkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

                if (chkBoxRows.Checked)
                {
                     aMiaTarget = new MIOTargetProductWise();

                    aMiaTarget.MiaId = Convert.ToInt32(mioDropDownList.SelectedValue);
                    aMiaTarget.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue); 
                    aMiaTarget.Period = periodDropDownList.SelectedItem.Text.Trim();
                    aMiaTarget.Year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    aMiaTarget.ProductId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());
                    aMiaTarget.TargetQty = Convert.ToDecimal(((TextBox)loadGridView.Rows[i].Cells[4].FindControl("returnQtyTextBox")).Text);

                    aReportDal.SaveMiaTargetProductWise(aMiaTarget);
                }
            }
            Clear();
        }
        else
        {
            showMessageBox("Please input data in all Textbox !!!");
        }
    }


    protected void miaViewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MiaTargetView.aspx");
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {   
        if (companyNameDropDownList.SelectedValue != "")
        {
            aReportDal.LoadMIOInformation(mioDropDownList, companyNameDropDownList.SelectedValue);

            DataTable aTable = aReportDal.GetActiveProduct(companyNameDropDownList.SelectedValue);

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
            mioDropDownList.Items.Clear();
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        var chkBoxHeader = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            var chkBoxRows = (CheckBox)loadGridView.Rows[i].Cells[0].FindControl("chkSelect");

            chkBoxRows.Checked = chkBoxHeader.Checked == true;
        }
    }

    protected void periodDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (mioDropDownList.SelectedValue != "")
        {
            if (periodDropDownList.SelectedValue != "")
            {
                DataTable aTable = aReportDal.GetExistingData(DateTime.Now.Year.ToString(CultureInfo.InvariantCulture),
                    periodDropDownList.SelectedValue, mioDropDownList.SelectedValue);


                if (aTable.Rows.Count > 0)
                {
                    for (int i = 0; i < loadGridView.Rows.Count; i++)
                    {
                        var chkBoxRows = (CheckBox) loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
                        Int32 productId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString());

                        for (int j = 0; j < aTable.Rows.Count; j++)
                        {
                            if (productId == aTable.Rows[j].Field<int>("ProductId"))
                            {
                                chkBoxRows.Checked = true;
                                ((TextBox) loadGridView.Rows[i].Cells[4].FindControl("returnQtyTextBox")).Text =
                                    aTable.Rows[j].Field<int>("TargetQty").ToString();
                            }

                        }

                    }
                }
                else
                {
                    for (int i = 0; i < loadGridView.Rows.Count; i++)
                    {
                        var chkBoxRows = (CheckBox) loadGridView.Rows[i].Cells[0].FindControl("chkSelect");
                        chkBoxRows.Checked = false;
                        ((TextBox)loadGridView.Rows[i].Cells[4].FindControl("returnQtyTextBox")).Text = "";
                    }
                }
            }

        }
        else
        {
            periodDropDownList.SelectedIndex = 0;
            showMessageBox("Please select MIO !!!");
        }
        
    }
}