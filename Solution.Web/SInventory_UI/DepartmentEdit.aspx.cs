using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Library.BLL.SInventory_BLL;

using Library.DAO.SInventory_Entities;

public partial class HRM_UI_DepartmentEdit : System.Web.UI.Page
{
    DepartmentBLL aDepartmentBll = new DepartmentBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deptIdHiddenField.Value = Request.QueryString["ID"];
            DepartmentLoad(deptIdHiddenField.Value);
        }
    }
    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (departmentNameTextBox.Text != "" )
        { 
            var aDepartment = new Department()
            {
                DepartmentId = Convert.ToInt32(deptIdHiddenField.Value),
                DepartmentName = departmentNameTextBox.Text                
            };
            var aDepartmentBll = new DepartmentBLL();

            if (!aDepartmentBll.UpdateDataForDepartment(aDepartment))
            {
                ShowMessageBox("Data Not Update!!!");
                MessageLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
               ShowMessageBox("Data Update Successfully!!! Please Reload");
                MessageLabel.ForeColor = System.Drawing.Color.Green;
            }
        }
        else
        {
            ShowMessageBox("Please input data in all Textbox");
        }
    }

    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private void DepartmentLoad(string departmentId)
    {
        var aDepartment = new Department();
        aDepartment = aDepartmentBll.DepartmentEditLoad(departmentId);
        departmentNameTextBox.Text = aDepartment.DepartmentName;

        nameHiddenField.Value = aDepartment.DepartmentName;
    }


    protected void closeButton_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(string), "Close", "window.close()", true);
    }

    private void GotoUpdatePermissionSetUp(DataTable aTable, TextBox textBox, HiddenField presentData, string msg)
    {

        if (aTable.Rows.Count > 0)
        {
            Int32 count = 0;

            for (int i = 0; i < aTable.Rows.Count; i++)
            {
                if (aTable.Rows[i].Field<Int32>("DeptId") != Convert.ToInt32(deptIdHiddenField.Value))
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

    protected void departmentNameTextBox_OnTextChanged(object sender, EventArgs e)
    {
        DataTable info = aDepartmentBll.NameExistOrNot(departmentNameTextBox.Text.Trim());
        GotoUpdatePermissionSetUp(info, departmentNameTextBox,nameHiddenField,"Department Name already Exist !!!");
    }
}