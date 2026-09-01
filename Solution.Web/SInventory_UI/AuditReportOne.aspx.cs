using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;

public partial class SInventory_UI_AuditReportOne : System.Web.UI.Page
{
    AuditReportOneBll aOrderReportBll = new AuditReportOneBll();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdownlist();
        }
    }

    private void LoadDropdownlist()
    {
        aOrderReportBll.LoadSalesCenter(comUnitNameDropDownList);
        //aOrderReportBll.LoadCustomer(customerDropDownList);
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }


    private void Popup()
    {
            
        string rptType = "0";

        if (nationalCheckBox.Checked == false)
        {
            rptType = "1";
        }

        Session["Excel"] = "";
        Session["Excel"] = "N";

        Session["parameter"] = "";
        Session["parameter"] = GenerateParameter();

        string url = "../SInventory_RPTVIEW/AuditReportOneReportViewer.aspx?rptType=" + rptType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true); ;


    }

    private string GenerateParameter()
    {

        string parameter = " WHERE ";

        if (comUnitNameDropDownList.Text != "")
        {
            if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
            {
                parameter = parameter + "OD.ComUnitCode = '" + comUnitNameDropDownList.Text.Trim() +
                            "' AND OD.DeleteDate BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND '" +
                            toDateTextBox.Text.Trim() + "' AND '";
            }

            else
            {
                parameter = parameter + "OD.ComUnitCode = '" + comUnitNameDropDownList.Text.Trim() + "' AND '";
            }

        }

        if (comUnitNameDropDownList.Text == "")
        {
            if (fromDateTextBox.Text != "" && toDateTextBox.Text != "")
            {
                parameter = parameter + " CONVERT(date, OD.DeleteDate) BETWEEN '" + fromDateTextBox.Text.Trim() + "' AND'" +
                            toDateTextBox.Text.Trim() +"' AND '";
            }
            else
            {
                parameter = "";
            }           
        }

        string finalParameter = "";

        if (parameter != "")
        {
            finalParameter = parameter.Remove(parameter.Length - 5);
        }

        return finalParameter;
    }

    protected void cancelButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        MessageLabel.Text = "";
    }

    private void DateValidationCheck(TextBox dateTextBox)
    {
        DateTime temp;
        if (!DateTime.TryParse(dateTextBox.Text, out temp))
        {
            ShowMessageBox("Please select valid date!!!");
            dateTextBox.Text = "";
        }
    }

    protected void submitButton_Click1(object sender, EventArgs e)
    {
        if (comUnitNameDropDownList.SelectedValue != "" || nationalCheckBox.Checked)
        {
            if (Validation())
            {
                Popup();
                Clear();
            }
        }
        else
        {
             ShowMessageBox("Please select national report or a sales center!!!");
        }
    }

    private bool Validation()
    {
        if (fromDateTextBox.Text != "")
        {
            if (toDateTextBox.Text == "")
            {
                ShowMessageBox("Please select to date");
                return false;
            }
        }
        else if (toDateTextBox.Text != "")
        {
            if (fromDateTextBox.Text == "")
            {
                ShowMessageBox("Please select from date");
                return false;
            }
        }

        return true;
    }

    protected void nationalCheckBox_OnCheckedChanged(object sender, EventArgs e)
    {
        if (nationalCheckBox.Checked)
        {
            comUnitNameDropDownList.SelectedValue = "";
            comUnitNameDropDownList.Enabled = false;
        }
        else
        {
            comUnitNameDropDownList.Enabled = true;
        }
    }

    protected void excelButton_OnClick(object sender, EventArgs e)
    {
        if (Validation())
        {
            ExcelPopUp();
        }
        
    }

    private void ExcelPopUp()
    {
        string rptType = "0";

        if (nationalCheckBox.Checked == false)
        {
            rptType = "1";
        }

        Session["Excel"] = "";
        Session["Excel"] = "Y";

        Session["parameter"] = "";
        Session["parameter"] = GenerateParameter();

        string url = "../SInventory_RPTVIEW/DeleteOrderReportViewer.aspx?rptType=" + rptType;
        string fullURL = "var Mleft = (screen.width/2)-(950/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no, scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";
        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true); ;
    }

    protected void fromDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateValidationCheck(fromDateTextBox);
    }


    protected void toDateTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DateValidationCheck(toDateTextBox);
    }

   
}