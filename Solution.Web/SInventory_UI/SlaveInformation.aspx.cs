using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.CrystalReports.SInventory_DS;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_SlaveInformation : System.Web.UI.Page
{
    SlaveinformationBll aBll = new SlaveinformationBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropDown();
            SetActiveStatus();

            if (Session["SlaveId"] != null)
            {
                GetOneRecord(Session["SlaveId"].ToString());
                Session["SlaveId"] = null;
            }

            SetPageMode();
            SetCompanyWiseStatus();
        }
    }

    private void GetOneRecord(string slaveId)
    {
        DataTable aTable = aBll.LoadSlaveInformationById(slaveId);

        if (aTable.Rows.Count > 0)
        {
            slaveIdHiddenField.Value = aTable.Rows[0].Field<Int32>("SlaveId").ToString(CultureInfo.InvariantCulture);
            companyNameDropDownList.SelectedValue = aTable.Rows[0].Field<Int32>("CompanyId").ToString(CultureInfo.InvariantCulture);
            maxAmountTextBox.Text = aTable.Rows[0].Field<Decimal>("MaxAmmount").ToString(CultureInfo.InvariantCulture);
            minAmmountTextBox.Text = aTable.Rows[0].Field<Decimal>("MinAmmount").ToString(CultureInfo.InvariantCulture);
            disPerTextBox.Text = aTable.Rows[0].Field<Int32>("DisParcentage").ToString(CultureInfo.InvariantCulture);

            isActiveCheckBox.Checked = aTable.Rows[0].Field<bool>("IsActive");
            activeDtTextBox.Text = aTable.Rows[0].Field<DateTime>("ActiveDate").ToString("dd-MMM-yyyy");
            
            activeDtTextBox.Enabled = false;
            maxAmountTextBox.Enabled = false;
            minAmmountTextBox.Enabled = false;
            disPerTextBox.Enabled = false;
            ImageButton.Enabled = false;
        }
    }

    private void SetPageMode()
    {
        if (slaveIdHiddenField.Value == "")
        {
            modeLabel.Text = "Entry";
        }

        if (slaveIdHiddenField.Value != "")
        {
            modeLabel.Text = "Edit";
            submitButton.Text = "Update";
        }
    }

    private void LoadDropDown()
    {
        aBll.LoadCompanyDropdownList(companyNameDropDownList);
    }

    private void SetActiveStatus()
    {
        if (!isActiveCheckBox.Checked)
        {
            isActiveCheckBox.Checked = true;
        }
        else
        {
            newSlave.Visible = false;

        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private bool Validation()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            ShowMessageBox("Please select a company!!");
            return false;
        }

        if (maxAmountTextBox.Text == "")
        {
            ShowMessageBox("Maximum amount is required!!");
            return false;
        }

        if (minAmmountTextBox.Text == "")
        {
            ShowMessageBox("Minimum amount is required!!");
            return false;
        }

        if (disPerTextBox.Text == "")
        {
            ShowMessageBox("Discount Percentage Is Required!!");
            return false;
        }

        if (isActiveCheckBox.Checked && slaveIdHiddenField.Value == "")
        {
            if (activeDtTextBox.Text == "")
            {
                ShowMessageBox("Active Date Is Required!!");
                return false;
            }

        }
        else
        {
            if (inActiveDateTextBox.Text == "")
            {
                ShowMessageBox("In Active Date Is Required!!");
                return false;
            }

            if (newMaxAmountTextBox.Text == "")
            {
                ShowMessageBox("(New)Maximum amount is required!!");
                return false;
            }

            if (newMinAmountTextBox.Text == "")
            {
                ShowMessageBox("(New)Minimum amount is required!!");
                return false;
            }

            if (newDisPerTextBox.Text == "")
            {
                ShowMessageBox("(New)Discount Percentage Is Required!!");
                return false;
            }

            if (newActiveDateTextBox.Text == "")
            {
                ShowMessageBox("(New)Active Date Is Required!!");
                return false;
            }

            if (newActiveDateTextBox.Text == "")
            {
                ShowMessageBox("(New)Active Date Is Required!!");
                return false;
            }
        }

        return true;
    }

    private void Clear()
    {
        slaveIdHiddenField.Value = "";
        companyNameDropDownList.SelectedValue = "";
        maxAmountTextBox.Text = "";
        minAmmountTextBox.Text = "";
        disPerTextBox.Text = "";
        activeDtTextBox.Text = "";
        inActiveDateTextBox.Text = "";

        newMaxAmountTextBox.Text = "";
        newMinAmountTextBox.Text = "";
        newDisPerTextBox.Text = "";
        newActiveDateTextBox.Text = "";

        SetPageMode();
        SetActiveStatus();
        submitButton.Text = "Submit";
        
    }

    protected void isActiveCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        if (!isActiveCheckBox.Checked)
        {
            newSlave.Visible = true;
        }
        else
        {
            newSlave.Visible = false;
        }
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            if (slaveIdHiddenField.Value != "" && !isActiveCheckBox.Checked)
            {
                UpdateSlaveInformation();
            }

            else
            {
                SaveSlaveInformation("Information Saved Successfully !!!");
            }
        }
    }

    private void SaveSlaveInformation(string msg)
    {
        var aInformation = new SlaveInformation();

        aInformation.CompanyId = Convert.ToInt32(companyNameDropDownList.SelectedValue);

        if (slaveIdHiddenField.Value == "")
        {
            aInformation.MaxAmmount = Convert.ToDecimal(maxAmountTextBox.Text);
            aInformation.MinAmmount = Convert.ToDecimal(minAmmountTextBox.Text);
            aInformation.DisParcentage = Convert.ToInt32(disPerTextBox.Text);
            aInformation.ActiveDate = Convert.ToDateTime(activeDtTextBox.Text);
            aInformation.IsActive = isActiveCheckBox.Checked;
            aInformation.ActionStatus = "Posted";
        }
        if (slaveIdHiddenField.Value != "")
        {
            aInformation.IsActive = true;
            aInformation.MaxAmmount = Convert.ToDecimal(newMaxAmountTextBox.Text);
            aInformation.MinAmmount = Convert.ToDecimal(newMinAmountTextBox.Text);
            aInformation.DisParcentage = Convert.ToInt32(newDisPerTextBox.Text);
            aInformation.ActiveDate = Convert.ToDateTime(newActiveDateTextBox.Text);
        }

        
        aInformation.EntryBy = Session["LoginName"].ToString();
        aInformation.EntryDate = DateTime.Now;

        if (aBll.LoadSlaveDataForSave(aInformation))
        {
            ShowMessageBox(msg);
            Clear();
        }
    }

    private void UpdateSlaveInformation()
    {
        if (slaveIdHiddenField.Value != "")
        {
            var aInformation = new SlaveInformation();

            aInformation.IsActive = isActiveCheckBox.Checked;
            aInformation.SlaveId = Convert.ToInt32(slaveIdHiddenField.Value);
            aInformation.InActiveBy = Session["LoginName"].ToString();
            aInformation.InActiveDate = Convert.ToDateTime(inActiveDateTextBox.Text);
            aInformation.UpdateBy = Session["LoginName"].ToString();
            aInformation.UpdateDate = DateTime.Now;

            if (aBll.ChangeStatusToFalse(aInformation))
            {
                SaveSlaveInformation("Slave Information Updated !!!");
                Clear();
            }
        }
    }

    protected void maxAmountTextBox_OnTextChanged(object sender, EventArgs e)
    {
        if (slaveIdHiddenField.Value == "")
        {

            if (companyNameDropDownList.SelectedValue != "")
            {
                if (CheckRange(companyNameDropDownList.SelectedValue, Convert.ToDecimal(maxAmountTextBox.Text), maxAmountTextBox))
                {
                    CheckMAxMin();
                }
            }
            else
            {
                ShowMessageBox("Please Select a Company first !!!");
            }
            
        }
    }

    private void CheckMAxMin()
    {
        if (maxAmountTextBox.Text != "" && minAmmountTextBox.Text != "")
        {
            decimal max = Convert.ToDecimal(maxAmountTextBox.Text);
            decimal min = Convert.ToDecimal(minAmmountTextBox.Text);

            if (min > max)
            {
                ShowMessageBox("Invalid Maximum and Minimum Range !!! ");
                maxAmountTextBox.Text = "";
                minAmmountTextBox.Text = "";
            }

        }

        if (slaveIdHiddenField.Value != "")
        {
            if (newMaxAmountTextBox.Text != "" && newMinAmountTextBox.Text != "")
            {
                decimal max = Convert.ToDecimal(newMaxAmountTextBox.Text);
                decimal min = Convert.ToDecimal(newMinAmountTextBox.Text);

                if (min > max)
                {
                    ShowMessageBox("Invalid Maximum and Minimum Range !!! ");
                    newMaxAmountTextBox.Text = "";
                    newMinAmountTextBox.Text = "";
                }

            }
        }
      
    }

    private bool CheckRange(string companyId, decimal value, TextBox ammountTextBox)
    {
        DataTable aTable = aBll.LoadSlaveInformationByCompanyId(companyId);

        if (aTable.Rows.Count > 0)
        {
            var maxDecimal = aTable.Rows[0].Field<Decimal>("MaxAmmount");
            var minDecimal = aTable.Rows[0].Field<Decimal>("MinAmmount");

            decimal maxValue = maxDecimal + 1;
            decimal minValue = minDecimal - 1;


            if (value != 0)
            {
                if (value > minValue && value < maxValue)
                {
                    ShowMessageBox("Value already exist!");
                    ammountTextBox.Text = "";
                    return false;
                }
            }
            
            
        }

        return true;
    }

    

    protected void minAmmountTextBox_OnTextChanged(object sender, EventArgs e)
    {
       
        if (slaveIdHiddenField.Value == "")
        {
            if (companyNameDropDownList.SelectedValue != "")
            {
                if (CheckRange(companyNameDropDownList.SelectedValue, Convert.ToDecimal(minAmmountTextBox.Text),
                    minAmmountTextBox))
                {
                    CheckMAxMin();
                }
            }
            else
            {
                ShowMessageBox("Please Select a Company first !!!");
            }
            
        }  
    }

    protected void ListImageButton_Click(object sender, ImageClickEventArgs e)
    {
       Response.Redirect("SlaveInformationView.aspx");
    }

    protected void newMinAmountTextBox_OnTextChanged(object sender, EventArgs e)
    {
        CheckMAxMin();
    }

    protected void newMaxAmountTextBox_OnTextChanged(object sender, EventArgs e)
    {
        CheckMAxMin();
    }

    protected void companyNameDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        SetCompanyWiseStatus();
    }

    private void SetCompanyWiseStatus()
    {
        if (companyNameDropDownList.SelectedValue == "")
        {
            maxAmountTextBox.ReadOnly = true;
            minAmmountTextBox.ReadOnly = true;
        }
        else
        {
            maxAmountTextBox.ReadOnly = false;
            minAmmountTextBox.ReadOnly = false;
        }
    }
}