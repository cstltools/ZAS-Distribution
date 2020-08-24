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

public partial class SInventory_UI_ASMInfo : System.Web.UI.Page
{
    ASMInfoBll aBll = new ASMInfoBll();

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
        if (asmIdHiddenField.Value == "")
        {
            modeLabel.Text = "Entry";
        }

        if (asmIdHiddenField.Value != "")
        {
            modeLabel.Text = "Edit";
        }
    }

    private void GetOneRecord(string rsmId)
    {
        DataTable aTable = aBll.LoadASMInfoByID(rsmId);

        if (aTable.Rows.Count > 0)
        {
            asmIdHiddenField.Value = aTable.Rows[0].Field<Int32>("ASMId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.Enabled = false;

            //if (companyNameDropDownList.SelectedValue != "")
            //{
            //    aBll.LoadRegionDropdownList(regionDropDownList, companyNameDropDownList.SelectedValue);

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
                aBll.LoadEmployeeDropDownList(employeeDropDownList);
                employeeDropDownList.SelectedValue =
                    aTable.Rows[0].Field<Int32>("EmployeeId").ToString(CultureInfo.InvariantCulture);
                employeeDropDownList.Enabled = false;
            }
            else
            {
                employeeDropDownList.Items.Clear();
            }

            if (regionDropDownList.SelectedValue != "")
            {
                aBll.LoadAreaDropDownList(areaDropDownList, regionDropDownList.SelectedValue);
                areaDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("AreaId").ToString(CultureInfo.InvariantCulture);
                areaDropDownList.Enabled = false;
                
            }
            else
            {
                areaDropDownList.Items.Clear();
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
       // aBll.LoadEmployeeDropDownList(employeeDropDownList);

    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (asmIdHiddenField.Value != "")
            {
                var asmInfo = new ASMInfoDao();

                asmInfo.ASMId = Convert.ToInt32(asmIdHiddenField.Value);

                if (!isActiveCheckBox.Checked)
                {
                    asmInfo.InActiveDate = Convert.ToDateTime(inActiveDateTextBox.Text);
                }

                asmInfo.InActiveBy = Session["LoginName"].ToString();
                asmInfo.IsActive = isActiveCheckBox.Checked;
                asmInfo.UpdateBy = Session["LoginName"].ToString();
                asmInfo.UpdateDate = DateTime.Now;

                if (!aBll.CheckAssignedOrNot(companyNameDropDownList.SelectedValue, areaDropDownList.SelectedValue, Convert.ToInt32(asmIdHiddenField.Value)))
                {
                    aBll.UpdateAsmInfo(asmInfo);
                    LoadGridView();
                    asmIdHiddenField.Value = "";
                    Clear();
                }
                else
                {
                    ShowMessageBox("Another Allocation Exist!!!");
                }

            }
            else
            {
                var aInformationDao = new ASMInfoDao();

                aInformationDao.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue);
                aInformationDao.AreaId = Convert.ToInt32(areaDropDownList.SelectedValue);

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

        //if (companyNameDropDownList.SelectedValue == "")
        //{
            regionDropDownList.SelectedValue = "";
        //}
        regionDropDownList.Enabled = true;

        if (companyNameDropDownList.SelectedValue == "")
        {
            employeeDropDownList.Items.Clear();
        }

        if (regionDropDownList.SelectedValue == "")
        {
            areaDropDownList.Items.Clear();
        }
        areaDropDownList.Enabled = true;
        vacantCheckBox.Enabled = true;

        employeeDropDownList.SelectedValue = "";
        employeeDropDownList.Enabled = true;
        SetIsActiveStatus();
        asmIdHiddenField.Value = "";


        if (vacantCheckBox.Checked)
        {
            vacantCheckBox.Checked = false;
        }

        submitButton.Text = "Submit";
        SetPageMode();

        activeDateTextBox.Text = "";
        inActiveDateTextBox.Text = "";
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

        if (asmIdHiddenField.Value != "")
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

        if (areaDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please Select a Area!!");
            return false;
        }

        if (!vacantCheckBox.Checked && asmIdHiddenField.Value == "")
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

        if (asmIdHiddenField.Value != "")
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
            regionDropDownList.Items.Clear();
            aBll.LoadRegionDropdownList(regionDropDownList, companyNameDropDownList.SelectedValue);
            areaDropDownList.Items.Clear();
            employeeDropDownList.Items.Clear();
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
        if (asmIdHiddenField.Value != "")
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

            if (loadGridView.Rows[rowindex].Cells[5].Text.Trim() != "False")
            {
                if (dataKey != null)
                {
                    string asmId = dataKey[0].ToString();

                    Session["ASMId"] = "";
                    Session["ASMId"] = asmId;

                    Response.Redirect("ASMInfo.aspx");
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
        if (asmIdHiddenField.Value == "")
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

        if (asmIdHiddenField.Value != "")
        {
            vacantCheckBox.Enabled = false;
        }

    }

    protected void regionDropDownList_OnTextChanged(object sender, EventArgs e)
    {
        if (regionDropDownList.SelectedValue != "")
        {
            areaDropDownList.Items.Clear();
            aBll.LoadAreaDropDownList(areaDropDownList, regionDropDownList.SelectedValue,companyNameDropDownList.SelectedValue);
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