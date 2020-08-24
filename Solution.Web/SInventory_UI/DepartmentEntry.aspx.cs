using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DepartmentEntry : System.Web.UI.Page
{

    DepartmentBLL aDepartmentBll = new DepartmentBLL();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void Clear()
    {
       DepartmentNameTextBox.Text = string.Empty;       
    }
   
    protected void submitButton_Click(object sender, EventArgs e)
    {
        if ( DepartmentNameTextBox.Text !="")
        {
            var aDepartment = new Department()
                 {
                    DepartmentName = DepartmentNameTextBox.Text
                 };

            ShowMessageBox(aDepartmentBll.SaveDataForDepartment(aDepartment));
            Clear();
        }
        else
        {
            ShowMessageBox("Department Name is Required!!!");
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
        Response.Redirect("DepartmentView.aspx");
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
       Clear();
    }

    protected void departmentNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aDepartmentBll.NameExistOrNot(DepartmentNameTextBox.Text.Trim());
        GotoPermissionSetUp(info, DepartmentNameTextBox, "Department Name already Exist !!!");
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