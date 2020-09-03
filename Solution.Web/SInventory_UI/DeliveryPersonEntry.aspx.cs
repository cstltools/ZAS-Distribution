using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_DeliveryPersonEntry : System.Web.UI.Page
{
    DeliveryPersonBLL aDeliveryPersonBll=new DeliveryPersonBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DropDownList();
            SetActiveStatus();

            if (Session["DeliveryManId"] != null)
            {
                delInfoIdHiddenField.Value = Session["DeliveryManId"].ToString();
                GetDate(Session["DeliveryManId"].ToString());
                Session["DeliveryManId"] = null;
            }    
        }
        
    }

    private void SetActiveStatus()
    {
        if (!isActiveCheckBox.Checked)
        {
            isActiveCheckBox.Checked = true;
        }
    }

    private void DropDownList()
    {
        //aDeliveryPersonBll.LoadCompanyDropdownList(companyNameDropDownList);
        aDeliveryPersonBll.EmpLoad(empDropDownList);

    }

    private void GetDate(string id)
    {
        DataTable dt = aDeliveryPersonBll.LoadDelivaryManById(id);

        if (dt.Rows.Count > 0)
        {
            //companyNameDropDownList.SelectedValue = dt.Rows[0]["CompanyId"].ToString();


            //if (companyNameDropDownList.SelectedValue != "")
            //{
                aDeliveryPersonBll.EmpLoad(empDropDownList);
                empDropDownList.SelectedValue = dt.Rows[0]["EmpInfoId"].ToString();
            //}
            //else
            //{
            //    companyNameDropDownList.Items.Clear();
            //}

            if (empDropDownList.SelectedValue != "")
            {
                delNIDTextBox.Text = dt.Rows[0]["DeliveryManId"].ToString();
                empIdHiddenField.Value = dt.Rows[0]["EmpInfoId"].ToString();
                delNIDTextBox.Text = dt.Rows[0]["NationalIdNo"].ToString();
                delNameTextBox.Text = dt.Rows[0]["EmpName"].ToString();
                delPhoneTextBox.Text = dt.Rows[0]["CellNumber"].ToString();
                delEmailTextBox.Text = dt.Rows[0]["Email"].ToString();
                delContactNoTextBox.Text = dt.Rows[0]["EmrgContactNo"].ToString();
                delrefNameTextBox.Text = dt.Rows[0]["RefName"].ToString();
                delrefphoneTextBox.Text = dt.Rows[0]["RefContactNo"].ToString();
                deladdTextBox.Text = dt.Rows[0]["AddressPresent"].ToString();
                presentCityLabel.Text = dt.Rows[0]["PreCity"].ToString();
                remarksTextBox.Text = dt.Rows[0]["Remarks"].ToString();
                isActiveCheckBox.Checked = dt.Rows[0].Field<bool>("IsActive");
            }  
        }

        Button1.Text = "Update";
    }

    private void Clear()
    {
        delNIDTextBox.Text = string.Empty;
        delNameTextBox.Text = string.Empty;
        delPhoneTextBox.Text = string.Empty;
        delEmailTextBox.Text = string.Empty;
        delContactNoTextBox.Text = string.Empty;
        delrefNameTextBox.Text = string.Empty;
        delrefphoneTextBox.Text = string.Empty;
        deladdTextBox.Text = string.Empty;
        presentCityLabel.Text = string.Empty;
        delInfoIdHiddenField.Value = string.Empty;

        //companyNameDropDownList.SelectedValue = "";
        empDropDownList.SelectedValue = "";
        SetActiveStatus();
        remarksTextBox.Text = "";
        Button1.Text = "Submit";


    }
    private void ShowMessageBox(string message)
    {
        message = message.Replace("'", "\'");
        string sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    private bool Validation()
    {
        if (delNameTextBox.Text==string.Empty)
        {
            ShowMessageBox("Please Give Delivery Man Name!!");
            return false;
        }

        if (delNIDTextBox.Text == string.Empty)
        {
            ShowMessageBox("Please Give Delivery Man NID !!");
            return false;
        }
        if (empDropDownList.SelectedIndex ==0)
        {
            ShowMessageBox("Please Select an Employee !!");
            return false;
        }
        
        return true;
    }

    private void Save()
    {
        if (Validation())
        {
            if (empDropDownList.SelectedValue != "")
            {
                if (ExistOrNot("insert"))
                {
                    var aDeliveryPersonDao = new DeliveryPersonDAO()
                    {
                        IsActive = isActiveCheckBox.Checked,
                        Remarks = remarksTextBox.Text.Trim(),
                        EmpInfoId = Convert.ToInt32(empDropDownList.SelectedValue),
                        EntryBy = Session["LoginName"].ToString(),
                        EntryDate = DateTime.Now
                    };
                    if (aDeliveryPersonBll.SaveDeliveryPerson(aDeliveryPersonDao))
                    {
                        ShowMessageBox("Data Saved Successfully");
                        Clear();
                    }
                }
                else
                {
                    ShowMessageBox("Information Already Exist!!");
                }
            }
            else
            {
                ShowMessageBox("Please Select an Employee!!");
            }

            
        }
    }

    private void Update()
    {
        if (Validation())
        {

            if (empDropDownList.SelectedValue != "")
            {
                if (ExistOrNot("update"))
                {
                    var aDeliveryPersonDao = new DeliveryPersonDAO()
                    {
                        DeliveryManId = Convert.ToInt32(delInfoIdHiddenField.Value),
                        IsActive = isActiveCheckBox.Checked,
                        Remarks = remarksTextBox.Text.Trim(),
                        EmpInfoId = Convert.ToInt32(empDropDownList.SelectedValue),
                        UpdateBy = Session["LoginName"].ToString(),
                        UpdateDate = DateTime.Now
                    };

                    if (aDeliveryPersonBll.UpdateDeliveryInfo(aDeliveryPersonDao))
                    {
                        ShowMessageBox("Data Updated Successfully");
                        Clear();
                    }
                }
                else
                {
                    ShowMessageBox("Information Already Exist!!");
                }
            }
            else
            {
                ShowMessageBox("Please Select an Employee!!");
            }
            
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (delInfoIdHiddenField.Value==string.Empty)
        {
            Save();    
        }
        else
        {
            Update();
        }
        
    }
    protected void departmentListImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("DeliveryPersonView.aspx");
    }

    protected void companyNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (companyNameDropDownList.SelectedValue != "")
        {
            aDeliveryPersonBll.EmpLoad(empDropDownList, companyNameDropDownList.SelectedValue);
        }
        else
        {
            companyNameDropDownList.Items.Clear();
        }
    }

    protected void empDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (empDropDownList.SelectedValue != "")
        {
            if (delInfoIdHiddenField.Value == "")
            {
                if (ExistOrNot("textchanged"))
                {
                    DataTable dt = aDeliveryPersonBll.LoadEmpInfoById(empDropDownList.SelectedValue);

                    if (dt.Rows.Count > 0)
                    {
                        delNIDTextBox.Text = dt.Rows[0]["NationalIdNo"].ToString();
                        delNameTextBox.Text = dt.Rows[0]["EmpName"].ToString();
                        delPhoneTextBox.Text = dt.Rows[0]["CellNumber"].ToString();
                        delEmailTextBox.Text = dt.Rows[0]["Email"].ToString();
                        delContactNoTextBox.Text = dt.Rows[0]["EmrgContactNo"].ToString();
                        delrefNameTextBox.Text = dt.Rows[0]["RefName"].ToString();
                        delrefphoneTextBox.Text = dt.Rows[0]["RefContactNo"].ToString();
                        deladdTextBox.Text = dt.Rows[0]["AddressPresent"].ToString();
                        presentCityLabel.Text = dt.Rows[0]["PreCity"].ToString();
                    }
                }
                else
                {
                    empDropDownList.SelectedValue = "";
                    ShowMessageBox("Information Already Exist!!");
                }
            }

            if (delInfoIdHiddenField.Value != "")
            {
                if (!ExistOrNot("update"))
                {
                    ShowMessageBox("Information Already Exist!!");
                }
            }
            
        }
        
    }

    private bool ExistOrNot(string mode)
    {
        DataTable dt = aDeliveryPersonBll.LoadSRInfoById(empDropDownList.SelectedValue);

        if (mode != null)
        {
            if (mode == "insert" || mode == "textchanged" )
            {
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
            }

            if (mode == "update")
            {
                Int32 count = 0;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].Field<Int32>("EmpInfoId") != Convert.ToInt32(empIdHiddenField.Value))
                        {
                            count++;
                        }

                        if (count > 0)
                        {
                            break;
                        }
                    }

                    if (count > 0)
                    {
                        return false;
                    }
                    
                }
            }
        }
            

        
        return true;
    }

    protected void clearButton_OnClick(object sender, EventArgs e)
    {
        Clear();
    }
}