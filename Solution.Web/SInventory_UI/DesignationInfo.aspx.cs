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

public partial class SInventory_UI_DesignationInfo : System.Web.UI.Page
{
   DesignationBll aDesignationBll = new DesignationBll();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["DESGId"] != null)
            {
                GetOneRecord(Session["DESGId"].ToString());
                Session["ASMId"] = null;
            }
        }

    }

    private void GetOneRecord(string desigId)
    {
        var aDepartment = new Designation();

        aDepartment = aDesignationBll.DesignationEditLoad(desigId);
        designationNameTextBox.Text = aDepartment.DesignationName;
        nameHiddenField.Value = aDepartment.DesignationId.ToString(CultureInfo.InvariantCulture);

        desigHiddenField.Value = aDepartment.DesignationId.ToString(CultureInfo.InvariantCulture);
        submitButton.Text = "Update";
    }

    private void Clear()
    {
        designationNameTextBox.Text = string.Empty;
        submitButton.Text = "Submit";
    }

    protected void submitButton_Click(object sender, EventArgs e)
    {
        if (desigHiddenField.Value != "")
        {
            if (designationNameTextBox.Text != "")
            {
                var aDepartment = new Designation()
                {
                    DesignationId = Convert.ToInt32(desigHiddenField.Value),
                    DesignationName = designationNameTextBox.Text.Trim()
                };

                if (aDesignationBll.UpdateDataForDesignation(aDepartment))
                {
                    ShowMessageBox("Information Updated Successfully!!!");
                    Clear();
                }
                
            }
            else
            {
                ShowMessageBox("Designation Name is Required!!!");
            }
        }
        else
        {
            if (designationNameTextBox.Text != "")
            {
                var aDepartment = new Designation()
                {
                    DesignationName = designationNameTextBox.Text.Trim()
                };

                ShowMessageBox(aDesignationBll.SaveDataForDesignation(aDepartment));
                Clear();
            }
            else
            {
                ShowMessageBox("Designation Name is Required!!!");
            }
        }
           
    }


    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("DesignationId") != Convert.ToInt32(desigHiddenField.Value))
                {
                    count++;
                }

                if (count > 0)
                {
                    textBox.Text = presentData.Value;
                    ShowMessageBox(msg);
                    break;
                }
            }

        }
    }


    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void departmentListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DesignationView.aspx");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }

    protected void departmentNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aDesignationBll.NameExistOrNot(designationNameTextBox.Text.Trim());

        if (desigHiddenField.Value != "")
        {
            GotoUpdatePermissionSetUp(info, designationNameTextBox,nameHiddenField,"Designation Info already Exist !!!");
        }
        else
        {
            GotoPermissionSetUp(info, designationNameTextBox, "Designation Info already Exist !!!");
        }
        
    }


    private void GotoPermissionSetUp(DataTable aTable, TextBox textBox, string msg)
    {
        if (aTable.Rows.Count > 0)
        {
            textBox.Text = "";
            ShowMessageBox(msg);
        }
    }
}