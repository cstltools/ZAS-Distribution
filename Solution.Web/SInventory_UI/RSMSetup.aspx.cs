using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared.Json;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_RSMSetup : System.Web.UI.Page
{
    RSMSetupBll aBll = new RSMSetupBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            SetIsActiveStatus();

            if (Session["RSMId"] != null)
            {
                GetOneRecord(Session["RSMId"].ToString());
                Session["RSMId"] = null;
            }
            SetPageMode();
            LoadGridView();
        }
    }

    private void GetOneRecord(string rsmId)
    {
        DataTable aTable = aBll.LoadRSMInfoByID(rsmId);

        if (aTable.Rows.Count > 0)
        {
            rsmIdHiddenField.Value = aTable.Rows[0].Field<Int32>("RSMId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.Enabled = false;

            aBll.LoadRegionDropDownList(regionDropDownList);
            regionDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("RegionId").ToString(CultureInfo.InvariantCulture);
            regionDropDownList.Enabled = false;

            if (companyNameDropDownList.SelectedValue != "")
            {
                aBll.LoadEmployeeDropDownList(employeeDropDownList, companyNameDropDownList.SelectedValue);

            }
            else
            {
                employeeDropDownList.Items.Clear();
            }

            aBll.LoadEmployeeDropDownList(employeeDropDownList);

            employeeDropDownList.SelectedValue =
                    aTable.Rows[0].Field<Int32>("EmployeeId").ToString(CultureInfo.InvariantCulture);
            employeeDropDownList.Enabled = false;
            

            if (employeeDropDownList.SelectedValue != "")
            {
                vacantCheckBox.Enabled = false;
            }

            if (aTable.Rows[0].Field<bool>("IsActive"))
            {
                isActiveCheckBox.Checked = true;
            }
            else
            {
                activeDateTextBox.Enabled = false;
                isActiveCheckBox.Checked = false;
                inActive.Visible = true;
                inActiveDateTextBox.Text = aTable.Rows[0].Field<DateTime>("InActiveDate").ToString("dd-MMM-yyyy");   
            }

            updateStatusHiddenField.Value = aTable.Rows[0].Field<bool>("IsActive").ToString();

            if (aTable.Rows[0].Field<string>("Vacant") != "")
            {
                vacantCheckBox.Checked = true;
                vacantCheckBox.Enabled = false;
            }

            activeDateTextBox.Text = aTable.Rows[0].Field<DateTime>("ActiveDate").ToString("dd-MMM-yyyy");
            submitButton.Text = "Update";
        }
    }

    private void LoadGridView()
    {
        DataTable aTable = aBll.LoadGridviewData();

        if (aTable.Rows.Count > 0)
        {
            loadGridView.DataSource = aTable;
            loadGridView.DataBind();
        }
    }

    private void SetPageMode()
    {
        if (rsmIdHiddenField.Value == "")
        {
            modeLabel.Text = "Entry";
        }

        if (rsmIdHiddenField.Value != "")
        {
            modeLabel.Text = "Edit";
        }
    }

    private void SetIsActiveStatus()
    {
        activeDiv.Visible = true;

        if (!isActiveCheckBox.Checked)
        {
            isActiveCheckBox.Checked = true; 
        }
    }

    private void LoadDropdownList()
    {
        aBll.LoadCompanyDropdownList(companyNameDropDownList);
        //aBll.LoadRegionDropDownList(regionDropDownList);
       // aBll.LoadEmployeeDropDownList(employeeDropDownList);
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (rsmIdHiddenField.Value != "")
            {
                var rsmInfo = new RSMSetupDao();

                rsmInfo.RSMId = Convert.ToInt32(rsmIdHiddenField.Value);

                if (!isActiveCheckBox.Checked)
                {
                    rsmInfo.InActiveDate = Convert.ToDateTime(inActiveDateTextBox.Text);
                }
                
                rsmInfo.InActiveBy = Session["LoginName"].ToString();
                rsmInfo.IsActive = isActiveCheckBox.Checked;
                rsmInfo.UpdateBy = Session["LoginName"].ToString();
                rsmInfo.UpdateDate = DateTime.Now;

                if (!aBll.CheckAssignedOrNot(companyNameDropDownList.SelectedValue, regionDropDownList.SelectedValue, Convert.ToInt32(rsmIdHiddenField.Value)))
                {
                    aBll.UpdateRsmInfo(rsmInfo);
                    LoadGridView();
                    rsmIdHiddenField.Value = "";
                    Clear();
                }
                else
                {
                    ShowMessageBox("Another Allocation Exist!!!");
                }

            }
            else
            {
                var aInformationDao = new RSMSetupDao();

                aInformationDao.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
                aInformationDao.RegionId = Convert.ToInt32(regionDropDownList.SelectedValue);
 
                aInformationDao.EmployeeId = Convert.ToInt32(employeeDropDownList.SelectedValue);

                aInformationDao.IsActive = isActiveCheckBox.Checked;

                if (isActiveCheckBox.Checked)
                {
                    aInformationDao.ActiveDate = Convert.ToDateTime(activeDateTextBox.Text.Trim());
                }

                if (!isActiveCheckBox.Checked)
                {
                    aInformationDao.InActiveDate = Convert.ToDateTime(inActiveDateTextBox.Text.Trim());
                    aInformationDao.InActiveBy = Session["LoginName"].ToString();
                }

                aInformationDao.EntryBy = Session["LoginName"].ToString();
                aInformationDao.EntryDate = DateTime.Now;


                if (aBll.LoadRSMInfoForSave(aInformationDao))
                {
                    
                    LoadGridView();
                    Clear();
                    ShowMessageBox("Saved Successfully!!!");
                }
                else
                {
                    ShowMessageBox("Save Failed!!!");
                }
            }
        }
    }

    private void Clear()
    {
        companyNameDropDownList.SelectedValue = "";
        
        if (companyNameDropDownList.SelectedValue == "")
        {
            regionDropDownList.Items.Clear();
            employeeDropDownList.Items.Clear();
        }

        
        SetIsActiveStatus();
        rsmIdHiddenField.Value = "";


        if (vacantCheckBox.Checked)
        {
            vacantCheckBox.Checked = false;
        }
       
        submitButton.Text = "Save";

        activeDateTextBox.Text = "";

        companyNameDropDownList.Enabled = true;
        regionDropDownList.Enabled = true;
        employeeDropDownList.Enabled = true;
        vacantCheckBox.Enabled = true;
        inActive.Visible = false;
        
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (rsmIdHiddenField.Value != "")
        {
            if (updateStatusHiddenField.Value == isActiveCheckBox.Checked.ToString())
            {
                ShowMessageBox("There is no change for update!!");
                return false;
            }
        }

        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select a Company!!");
            return false;
        }

        if (regionDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select a Region!!");
            return false;
        }

        if (!vacantCheckBox.Checked && rsmIdHiddenField.Value == "")
        {
            if (employeeDropDownList.SelectedValue == "")
            {
                ShowMessageBox("Please Select an employee!!");
                return false;
            }
        }
               
        if (isActiveCheckBox.Checked)
        {
            if (activeDateTextBox.Text == "")
            {
                ShowMessageBox("Active Date is required!!");
                return false;
            }
            
        }

        if (rsmIdHiddenField.Value != "")
        {
            
                if (inActiveDateTextBox.Text == "")
                {
                    ShowMessageBox("In Active Date is required!!");
                    return false;
                }
        }
        

        return true;
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (companyNameDropDownList.SelectedValue != "")
        {
            aBll.LoadRegionDropdownList(regionDropDownList,companyNameDropDownList.SelectedValue);
            aBll.LoadEmployeeDropDownList(employeeDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            regionDropDownList.Items.Clear();
            employeeDropDownList.Items.Clear();
        }
    }

    protected void isActiveCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rsmIdHiddenField.Value != "")
        {
            if (!isActiveCheckBox.Checked)
            {
                inActive.Visible = true;
                inActiveDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                activeDateTextBox.Enabled = true;
                inActive.Visible = false;
            }
        }

        else
        {
            if (!isActiveCheckBox.Checked)
            {
                if (activeDateTextBox.Text != "")
                {
                    activeDiv.Visible = false;
                    activeDateTextBox.Text = "";
                }
                else
                {
                    activeDiv.Visible = true;
                }
            }
            else
            {
                activeDiv.Visible = true;
            }
            
        }
        
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            var dataKey = loadGridView.DataKeys[rowindex];

            if (loadGridView.Rows[rowindex].Cells[4].Text.Trim() != "False")
            {
                if (dataKey != null)
                {
                    string rsmId = dataKey[0].ToString();

                    Session["RSMId"] = "";
                    Session["RSMId"] = rsmId;

                    Response.Redirect("RSMSetup.aspx");
                }
            }
            else
            {
                ShowMessageBox("Edit is not possible!!!");
                LoadGridView();
            }
        }
    }

    protected void vacantCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rsmIdHiddenField.Value == "")
        {
            if (vacantCheckBox.Checked)
            {
                employeeDropDownList.SelectedValue = Convert.ToString(1);
            }
            else
            {
                employeeDropDownList.SelectedValue = "";
            }
            
        }

        if (rsmIdHiddenField.Value != "")
        {
            vacantCheckBox.Enabled = false;
        }

    }

    protected void loadGridView_OnRowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[5].Text == "False")
            {

            } 
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void employeeDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (employeeDropDownList.SelectedValue != "")
        {
            if (vacantCheckBox.Checked)
            {
                employeeDropDownList.SelectedValue = Convert.ToString(1);
            }
        }
    }

}