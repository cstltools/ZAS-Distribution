using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_MIOSetup : System.Web.UI.Page
{
    MIOSetupBll aBll = new MIOSetupBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownList();
            SetIsActiveStatus();

            if (Session["ASMId"] != null)
            {
                GetOneRecord(Session["ASMId"].ToString());
                Session["ASMId"] = null;
            }

            SetPageMode();
            LoadGridView();
        }
    }

    private void SetPageMode()
    {
        if (mioIdHiddenField.Value == "")
        {
            modeLabel.Text = "Entry";
        }

        if (mioIdHiddenField.Value != "")
        {
            modeLabel.Text = "Edit";
        }
    }

    private void GetOneRecord(string mioId)
    {
        DataTable aTable = aBll.LoadMIOInfoByID(mioId);

        if (aTable.Rows.Count > 0)
        {
            mioIdHiddenField.Value = aTable.Rows[0].Field<Int32>("MIOId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.Enabled = false;

            //if (companyNameDropDownList.SelectedValue != "")
            //{
                //aBll.LoadRegionDropdownList(regionDropDownList, companyNameDropDownList.SelectedValue);
                regionDropDownList.Items.Clear();
                aBll.LoadRegionDropDownList(regionDropDownList);
                regionDropDownList.SelectedValue =
                   aTable.Rows[0].Field<Int32>("RegionId").ToString(CultureInfo.InvariantCulture);
                regionDropDownList.Enabled = false;
            //}
            //else
            //{
            //    regionDropDownList.Items.Clear();
            //    regionDropDownList.Enabled = false;
            //}


            if (companyNameDropDownList.SelectedValue != "")
            {
                employeeDropDownList.Items.Clear();
                aBll.LoadEmployeeDropDownList(employeeDropDownList, companyNameDropDownList.SelectedValue);
                employeeDropDownList.SelectedValue =
                    aTable.Rows[0].Field<Int32>("EmployeeId").ToString(CultureInfo.InvariantCulture);
                employeeDropDownList.Enabled = false;
            }
            else
            {
                employeeDropDownList.Items.Clear();
            }

            //aBll.LoadEmployeeDropDownList(employeeDropDownList, companyNameDropDownList.SelectedValue);
            //employeeDropDownList.SelectedValue =
            //    aTable.Rows[0].Field<Int32>("EmployeeId").ToString(CultureInfo.InvariantCulture);
            //employeeDropDownList.Enabled = false;

            if (regionDropDownList.SelectedValue != "")
            {
                aBll.LoadAreaDropDownList(areaDropDownList, regionDropDownList.SelectedValue);
                areaDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("AreaId").ToString(CultureInfo.InvariantCulture);
                areaDropDownList.Enabled = false;

            }
            else
            {
                areaDropDownList.Items.Clear();
                areaDropDownList.Enabled = false;
            }

            if (areaDropDownList.SelectedValue != "")
            {
                aBll.LoadTerritoryDropDownList(territoryDropDownList, areaDropDownList.SelectedValue);
                territoryDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("TerritoryId").ToString(CultureInfo.InvariantCulture);
                territoryDropDownList.Enabled = false;
            }
            else
            {
                territoryDropDownList.Items.Clear();
                territoryDropDownList.Enabled = false;
            }

            employeeDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("EmployeeId").ToString(CultureInfo.InvariantCulture);
            employeeDropDownList.Enabled = false;

            if (employeeDropDownList.SelectedValue != "")
            {
                vacantCheckBox.Enabled = false;
            }

            updateStatusHiddenField.Value = aTable.Rows[0].Field<bool>("IsActive").ToString();

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
        aBll.LoadRegionDropDownList(regionDropDownList);
        //aBll.LoadEmployeeDropDownList(employeeDropDownList);
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (mioIdHiddenField.Value != "")
            {
                var mioInfo = new MIOSetupDao();

                mioInfo.MIOId = Convert.ToInt32(mioIdHiddenField.Value);

                if (!isActiveCheckBox.Checked)
                {
                    mioInfo.InActiveDate = Convert.ToDateTime(inActiveDateTextBox.Text);
                }

                mioInfo.InActiveBy = Session["LoginName"].ToString();
                mioInfo.IsActive = isActiveCheckBox.Checked;
                mioInfo.UpdateBy = Session["LoginName"].ToString();
                mioInfo.UpdateDate = DateTime.Now;

                if (!aBll.CheckAssignedOrNot(companyNameDropDownList.SelectedValue, territoryDropDownList.SelectedValue, Convert.ToInt32(mioIdHiddenField.Value)))
                {
                    aBll.UpdateMIOInfo(mioInfo);
                    LoadGridView();
                    mioIdHiddenField.Value = "";
                    Clear();
                }
                else
                {
                    ShowMessageBox("Another Allocation Exist!!!");
                }

            }
            else
            {
                var aInformationDao = new MIOSetupDao();

                aInformationDao.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
                aInformationDao.TerritoryId = Convert.ToInt32(territoryDropDownList.SelectedValue);


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


                if (aBll.LoadMIOInfoForSave(aInformationDao))
                {
                    ShowMessageBox("Saved Successfully!!!");
                    LoadGridView();
                    Clear();
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
        LoadDropdownList();

        companyNameDropDownList.SelectedValue = "";
        companyNameDropDownList.Enabled = true;
        regionDropDownList.SelectedValue = "";


        if (regionDropDownList.SelectedValue == "")
        {
            areaDropDownList.Items.Clear();
        }

        if (areaDropDownList.SelectedValue == "")
        {
            territoryDropDownList.Items.Clear();
        }
        
        


        regionDropDownList.Enabled = true;
        territoryDropDownList.Enabled = true;
        areaDropDownList.Enabled = true;
        vacantCheckBox.Enabled = true;

        employeeDropDownList.Items.Clear();
        employeeDropDownList.Enabled = true;
        SetIsActiveStatus();
        mioIdHiddenField.Value = "";


        if (vacantCheckBox.Checked)
        {
            vacantCheckBox.Checked = false;
        }

        submitButton.Text = "Submit";
        SetPageMode();

        inActive.Visible = false;
        activeDateTextBox.Text = "";
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {

        if (mioIdHiddenField.Value != "")
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

        if (territoryDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select a territory!!");
            return false;
        }

        if (!vacantCheckBox.Checked && mioIdHiddenField.Value == "")
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

        if (mioIdHiddenField.Value != "")
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
           // aBll.LoadRegionDropdownList(regionDropDownList, companyNameDropDownList.SelectedValue);
            employeeDropDownList.Items.Clear();
            aBll.LoadEmployeeDropDownList(employeeDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            //regionDropDownList.Items.Clear();
            employeeDropDownList.Items.Clear();
        }

        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();

        regionDropDownList.Items.Clear();
        aBll.LoadRegionDropDownList(regionDropDownList);
        areaDropDownList.Items.Clear();
        territoryDropDownList.Items.Clear();
    }

    protected void isActiveCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        if (mioIdHiddenField.Value != "")
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
                    activeDateTextBox.Text = "";
                }
            }
            else
            {

                if (activeDateTextBox.Text == "")
                {
                    activeDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                }
            }
        }

    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            var dataKey = loadGridView.DataKeys[rowindex];

            if (loadGridView.Rows[rowindex].Cells[6].Text.Trim() != "False")
            {
                if (dataKey != null)
                {
                    string asmId = dataKey[0].ToString();

                    Session["ASMId"] = "";
                    Session["ASMId"] = asmId;

                    Response.Redirect("MIOSetup.aspx");
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
        if (mioIdHiddenField.Value == "")
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

        if (mioIdHiddenField.Value != "")
        {
            vacantCheckBox.Enabled = false;
        }

    }


    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            //aBll.LoadAreaDropDownList(areaDropDownList, regionDropDownList.SelectedValue, companyNameDropDownList.SelectedValue);
            aBll.LoadAreaDropDownList(areaDropDownList, regionDropDownList.SelectedValue);
        }
        else
        {
            areaDropDownList.Items.Clear();
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void areaDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (areaDropDownList.SelectedValue != "")
        {
            aBll.LoadTerritoryDropDownList(territoryDropDownList, areaDropDownList.SelectedValue, companyNameDropDownList.SelectedValue);
        }
        else
        {
            territoryDropDownList.Items.Clear();
        }
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