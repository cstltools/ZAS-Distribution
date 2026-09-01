using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerCreditView : System.Web.UI.Page
{
    CreditLimitDAL aCashBll = new CreditLimitDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoaddropdownList();
        }
    }

    private void LoaddropdownList()
    {

        aCashBll.GetCompanyDropdownList(CompanyDropDown);

    }



    protected void custCodeTextBox_TextChanged(object sender, EventArgs e)
    {

        string Perticular = custCodeTextBox.Text.Trim();


        if (Perticular.Contains(':'))
        {
            string[] emp = Perticular.Split(':');
            //  TextBox txtPerticular = (TextBox)itemGridView.Rows[rowindex].Cells[1].FindControl("txtPerticular");
            custCodeTextBox.Text = emp[1];

            CustomerIdHiddenField.Value = emp[3];



        }
        else
        {

            custCodeTextBox.Text = "";
            CustomerIdHiddenField.Value = "";
            //showMessageBox("Input Correct Data !");
        }
    }


    private String GenerateParam()
    {
        string param = "";


        if (CompanyDropDown.SelectedValue != "")
        {
            param = param + "And tblCustomerCreditLimit.CompanyId ='" + CompanyDropDown.SelectedValue + "' ";
        }


        if (CustomerIdHiddenField.Value != "")
        {
            param = param + " And tblCustomerCreditLimit.CustomerMasterId ='" + CustomerIdHiddenField.Value + "' ";
        }

        return param;
    }


    protected void viewRptButton_Click(object sender, EventArgs e)
    {

        DataTable aTable = aCashBll.GetCustomerCreditInfo(GenerateParam());

        if (aTable.Rows.Count > 0)
        {
            invoiceGridView.DataSource = aTable;
            invoiceGridView.DataBind();
        }
        else
        {
            invoiceGridView.DataSource = null;
            invoiceGridView.DataBind();
        }


    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "EditData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            var dataKey = invoiceGridView.DataKeys[rowindex];


            if (Session["UserId"] != null && Session["UserId"].ToString() != "")
            {
                DashboardDal aDal = new DashboardDal();
                DataTable dtTable = aDal.GetIsCreditApprovalInfo(Session["UserId"].ToString());

                if (dtTable.Rows.Count > 0)
                {
                    if (dataKey != null)
                    {
                        string creditId = dataKey[0].ToString();

                        Session["CreditId"] = "";
                        Session["CreditId"] = creditId;

                        Response.Redirect("CustomerCreditLimitEntry.aspx");
                    }
                }
                else
                {
                    ShowMessageBox("You don't have authorization of this operation !!!");
                    viewRptButton_Click(null, null);
                    
                    
                }
            }
            
        }

        if (e.CommandName == "DeleteData")
        {
            int rowindex = Convert.ToInt32(e.CommandArgument);
            var dataKey = invoiceGridView.DataKeys[rowindex];

            if (Session["UserId"] != null && Session["UserId"].ToString() != "")
            {
                DashboardDal aDal = new DashboardDal();
                DataTable dtTable = aDal.GetIsCreditApprovalInfo(Session["UserId"].ToString());

                if (dtTable.Rows.Count > 0)
                {
                    if (dataKey != null)
                    {
                        string creditId = dataKey[0].ToString();


                        if (aDal.DeleteCredit(creditId))
                        {
                            ShowMessageBox("Deleted Successfully !!!");
                            viewRptButton_Click(null, null);
                        }

                        
                    }
                }
                else
                {
                    ShowMessageBox("You don't have authorization of this operation !!!");
                    viewRptButton_Click(null, null);


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
    protected void clearButton_Click(object sender, EventArgs e)
    {
        LoaddropdownList();
        custCodeTextBox.Text = "";
        CustomerIdHiddenField.Value = "";
    }

    protected void CustMasterNewImageButton_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("CustomerCreditLimitEntry.aspx");
    }

    protected void CustMasterReloadImageButton_Click(object sender, ImageClickEventArgs e)
    {
        viewRptButton_Click(null,null);
    }
}