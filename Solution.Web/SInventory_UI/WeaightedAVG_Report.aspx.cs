using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_WeaightedAVG_Report : System.Web.UI.Page
{
    DataTable aDataTable = new DataTable();
    AreaBLL areaBll = new AreaBLL();
    AreaDAL aAreaDAL = new AreaDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetCompany();
            LoadDropdownList(itemGroupDropDownList);
            //tbxFromDate.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            //tbxToDate.Text = DateTime.Now.ToString("dd MMMM, yyyy");
            tbxFromDate.Text = string.Empty;
            tbxToDate.Text = string.Empty;

        }
    }


    public void LoadDropdownList(DropDownList ddl)
    {

        areaBll.LoadDDLBll_Zas(ddl);
    }

    public string GenerateParameter()
    {
        string pram = "";

        if (unitViewDropDownList.SelectedValue != "")
        {
            pram = pram + " AND PD.CompanyId =" + unitViewDropDownList.SelectedValue;
        }

        //if (locationDropDownList.SelectedValue != "")
        //{
        //    pram = pram + " AND WG.Warehouse_Id =" + locationDropDownList.SelectedValue;
        //}

        //if (itemGroupDropDownList.SelectedValue != "")
        //{
        //    pram = pram + " AND ID.Item_Id =" + itemGroupDropDownList.SelectedValue;
        //}

        //if (tbxFromDate.Text != "" && tbxToDate.Text != "")
        //{
        //    pram = pram + " AND CONVERT(DATE, WG.Trans_Date) BETWEEN '" + Convert.ToDateTime(tbxFromDate.Text.Trim()).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(tbxToDate.Text.Trim()).ToString("yyyy-MM-dd") + "'";

        //}

        return pram;
    }


    public string GenerateClosingParameter()
    {
        string pram = "";

        if (unitViewDropDownList.SelectedValue != "")
        {
            pram = pram + " AND WG.Company_Id =" + unitViewDropDownList.SelectedValue;
        }

        if (locationDropDownList.SelectedValue != "")
        {
            pram = pram + " AND WG.Warehouse_Id =" + locationDropDownList.SelectedValue;
        }

        if (itemGroupDropDownList.SelectedValue != "")
        {
            pram = pram + " AND ID.ProductId =" + itemGroupDropDownList.SelectedValue;
        }

        if (tbxToDate.Text != "")
        {
            pram = pram + " AND CONVERT(DATE,WG.Trans_Date) <= '" + Convert.ToDateTime(tbxToDate.Text.Trim()).ToString("yyyy-MM-dd") + "'";
        }

        return pram;
    }


    public string GenerateOpeningParameter()
    {
        string pram = "";

        if (unitViewDropDownList.SelectedValue != "")
        {
            pram = pram + " AND WG.Company_Id =" + unitViewDropDownList.SelectedValue;
        }

        if (locationDropDownList.SelectedValue != "")
        {
            pram = pram + " AND WG.Warehouse_Id =" + locationDropDownList.SelectedValue;
        }

        if (itemGroupDropDownList.SelectedValue != "")
        {
            pram = pram + " AND ID.ProductId =" + itemGroupDropDownList.SelectedValue;
        }

        if (tbxFromDate.Text != "")
        {
            pram = pram + " AND CONVERT(DATE,WG.Trans_Date) < '" + Convert.ToDateTime(tbxFromDate.Text.Trim()).ToString("yyyy-MM-dd") + "'";
        }

        return pram;
    }



    private void LoadStockValuationReport()
    {

        if (Validation())
        {
            gvReport.DataSource = null;
            gvReport.DataBind();

            //string Param = "";
            //DataTable aTable = aReportDal.GetStockValuationReport(GenerateParameter(), GenerateOpeningParameter(), GenerateClosingParameter());
            DataTable aTable = aAreaDAL.GetStockValuationReport_Zas(GenerateParameter(), GenerateOpeningParameter(), GenerateClosingParameter());
            if (aTable.Rows.Count > 0)
            {
                gvReport.DataSource = aTable;
                gvReport.DataBind();
            }

            //aAreaDAL.CloseAllConnection_DAL();
        }

    }

    public bool Validation()
    {
        if (unitViewDropDownList.SelectedValue == "")
        {
            showMessageBox("Please Select Business Unit !!");
            return false;
        }

        //if (tbxFromDate.Text.Trim() == "")
        //{
        //    showMessageBox("Please Select From Date !!");
        //    return false;
        //}

        //if (tbxToDate.Text.Trim() == "")
        //{
        //    showMessageBox("Please Select To Date !!");
        //    return false;
        //}

        return true;
    }

    public void GetCompany()
    {
        try
        {
            // Load From Common

            AreaDAL aAreaDAL = new AreaDAL();

            string userId = HttpContext.Current.Session["UserId"].ToString();
            //string moduleId = HttpContext.Current.Session["ModuleId"].ToString();
            string moduleId = "0";

            //aFunctionDal.LoadUnitByPermission(stockInUnitDropDownList, Convert.ToInt32(userId != "" ? userId : "0"));
            //aFunctionDal.LoadUnitByPermissionByModule(unitViewDropDownList, Convert.ToInt32(userId != "" ? userId : "0"), Convert.ToInt32(moduleId != "" ? moduleId : "0"));
            aAreaDAL.LoadUnitByPermissionByModule_Zas(unitViewDropDownList, Convert.ToInt32(userId != "" ? userId : "0"), Convert.ToInt32(moduleId != "" ? moduleId : "0"));


            if (unitViewDropDownList.Items.Count == 2)
            {
                unitViewDropDownList.SelectedIndex = 1;
            }

        }
        catch (Exception)
        {

            Response.Redirect("StockValuationReport.aspx");
        }

    }



    protected void showMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if ((gv.ShowHeader == true && gv.Rows.Count > 0)
            || (gv.ShowHeaderWhenEmpty == true))
        {
            //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        //confirms that an HtmlForm control is rendered for the
        //specified ASP.NET server control at run time.
    }


    protected void cancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("StockValuationReport.aspx");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        // Ensure the grid has data
        if (gvReport.Rows.Count > 0)
        {
            // Rebind the latest data (optional but recommended)
            LoadStockValuationReport();

            // Clear previous response
            Response.Clear();
            Response.Buffer = true;

            // Set header to force download as Excel file
            string fileName = "ProductPriceReport_" + DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_tt") + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = Encoding.Unicode;
            Response.BinaryWrite(Encoding.Unicode.GetPreamble());

            // Remove paging (if any)
            gvReport.AllowPaging = false;

            // Style to ensure text wrapping and proper header visibility
            gvReport.GridLines = GridLines.Both;
            gvReport.HeaderStyle.BackColor = System.Drawing.Color.LightGray;

            // Render the GridView to an HTML string
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gvReport.RenderControl(hw);
                    string style = @"<style> 
                                    .textmode { mso-number-format:\@; } 
                                    table, th, td { border:1px solid black; border-collapse:collapse; }
                                 </style>";

                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup",
                "alert('No Data Found!');", true);
        }
    }

    protected void loadGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void searchTextBox_TextChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void searchButtonSO_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadStockValuationReport();
    }

    protected void unitViewDropDownList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (unitViewDropDownList.SelectedValue != "")
        {
            try
            {
                // Load From Common

                AreaDAL aAreaDAL = new AreaDAL();


                //aFunctionDal.LoadUnitByPermission(stockInUnitDropDownList, Convert.ToInt32(userId != "" ? userId : "0"));
                aAreaDAL.LoadUnitWiseLocation_Zas(locationDropDownList, unitViewDropDownList.SelectedValue);

                if (unitViewDropDownList.Items.Count == 2)
                {
                    unitViewDropDownList.SelectedIndex = 1;
                }

            }
            catch (Exception)
            {

                Response.Redirect("StockValuationReport.aspx");
            }
        }
    }
}